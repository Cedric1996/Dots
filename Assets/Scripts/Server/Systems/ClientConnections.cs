using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.DotsNetKit.NetCode;
using UnityEngine.SceneManagement;

public struct LevelLoadingTag : IComponentData { }

[DisableAutoCreation]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateBefore(typeof(DotsNetKit193GhostSendSystem))]
public class LoadRemoteLevelSystem : ComponentSystem
{
    EntityQuery m_NetworkConnection;
    protected override void OnCreate()
    {
        m_NetworkConnection = GetEntityQuery(
            ComponentType.ReadOnly<NetworkIdComponent>(),
            ComponentType.Exclude<LevelLoadingTag>(),
            ComponentType.Exclude<NetworkStreamInGame>());
        Debug.Log("Create LoadRemoteLevelSys.");
    }

    protected override void OnUpdate()
    {
        var entities = m_NetworkConnection.ToEntityArray(Allocator.TempJob);
        var networkIds = m_NetworkConnection.ToComponentDataArray<NetworkIdComponent>(Allocator.TempJob);

        for (int i = 0; i < entities.Length; ++i)
        {
            var ent = entities[i];
            var networkId = networkIds[i];

            SimpleConsole.WriteLine(string.Format("New client(NetworkId={0}) connected.", networkId.Value));

            // Load level RPC
            var rpcLoadLevelQueue = ClientServerSystemManager.serverWorld.GetOrCreateSystem<DotsNetKit193RpcSystem>().GetRpcQueue<RpcLoadLevel>();
            var rpcBuf = EntityManager.GetBuffer<OutgoingRpcDataStreamBufferComponent>(ent);
            rpcLoadLevelQueue.Schedule(rpcBuf, new RpcLoadLevel { levelName = SceneManager.GetActiveScene().name });

            PostUpdateCommands.AddComponent(ent, new LevelLoadingTag());
        }

        entities.Dispose();
        networkIds.Dispose();
    }
}

// TODO: LZ:
//      we may want to only enter game, after the level is loaded.
//      would be nice to have an async response of a RPC
[DisableAutoCreation]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateBefore(typeof(AddNetworkIdSystem))] // TODO: LZ: fix this!!!
[UpdateBefore(typeof(DotsNetKit193GhostSendSystem))]
public class EnterGameSystem : ComponentSystem
{
    EntityQuery m_NetworkConnection;
    protected override void OnCreate()
    {
        m_NetworkConnection = GetEntityQuery(
            ComponentType.ReadOnly<NetworkIdComponent>(),
            ComponentType.ReadWrite<LevelLoadingTag>(),
            ComponentType.Exclude<NetworkStreamInGame>());
    }

    protected override void OnUpdate()
    {
        var entities = m_NetworkConnection.ToEntityArray(Allocator.TempJob);
        var networkIds = m_NetworkConnection.ToComponentDataArray<NetworkIdComponent>(Allocator.TempJob);
        for (int i = 0; i < entities.Length; ++i)
        {
            var ent = entities[i];
            var networkId = networkIds[i];
            PostUpdateCommands.RemoveComponent<LevelLoadingTag>(ent);
            PostUpdateCommands.AddComponent(ent, new NetworkStreamInGame());
            PlayerManager.CreatePlayer(ent, networkId.Value);
        }
        entities.Dispose();
        networkIds.Dispose();
    }
}

[DisableAutoCreation]
[UpdateInGroup(typeof(ServerSimulationSystemGroup))]
[UpdateAfter(typeof(NetworkStreamReceiveSystem))]
public class HandleDisconnect : ComponentSystem
{
    EntityQuery m_NetworkConnection;
    protected override void OnCreate()
    {
        m_NetworkConnection = GetEntityQuery(
            ComponentType.ReadWrite<NetworkIdComponent>(),
            ComponentType.ReadWrite<CommandTargetComponent>(),
            ComponentType.ReadWrite<NetworkStreamDisconnected>());
    }

    protected override void OnUpdate()
    {
        var networkIds = m_NetworkConnection.ToComponentDataArray<NetworkIdComponent>(Allocator.TempJob);
        var ctcs = m_NetworkConnection.ToComponentDataArray<CommandTargetComponent>(Allocator.TempJob);

        for (int i = 0; i < networkIds.Length; ++i)
        {
            var networkId = networkIds[i];
            var ctc = ctcs[i];
            SimpleConsole.WriteLine(string.Format("Client(NetworkId={0}) disconnected.", networkId.Value));

            if (ctc.targetEntity != Entity.Null)
            {
                var transform = EntityManager.GetComponentObject<Transform>(ctc.targetEntity);
                Object.Destroy(transform.gameObject);
            }
        }

        networkIds.Dispose();
        ctcs.Dispose();
    }
}
