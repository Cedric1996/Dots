using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.DotsNetKit.NetCode;
using UnityEngine;

[DisableAutoCreation]
public class PlayerCommandSendSystem : CommandSendSystem<PlayerCommandData>
{
}

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
[UpdateAfter(typeof(GhostReceiveSystemGroup))]
[UpdateBefore(typeof(PlayerCommandSendSystem))]
public class InputSystem : ComponentSystem
{
    EntityQuery cmdTargetGroup;

    NetworkTimeSystem m_TimeSystem;

    protected override void OnCreate()
    {
        cmdTargetGroup = GetEntityQuery(
            ComponentType.ReadWrite<CommandTargetComponent>(),
            ComponentType.Exclude<NetworkStreamDisconnected>());

        m_TimeSystem = World.GetOrCreateSystem<NetworkTimeSystem>();
    }

    protected override void OnUpdate()
    {
        var entities = cmdTargetGroup.ToEntityArray(Allocator.TempJob);
        if (entities.Length == 1)
        {
            var ent = entities[0];

            if (!EntityManager.HasComponent<PlayerCommandData>(ent))
            {
                PostUpdateCommands.AddBuffer<PlayerCommandData>(ent);

                var ctc = EntityManager.GetComponentData<CommandTargetComponent>(ent);
                ctc.targetEntity = ent;
                PostUpdateCommands.SetComponent(ent, ctc);
            }
            else
            {
                var cmdBuf = EntityManager.GetBuffer<PlayerCommandData>(ent);

                PlayerCommandData cmdData = default;
                cmdData.tick = m_TimeSystem.predictTargetTick;
                cmdData.horizontal = Input.GetAxisRaw("Horizontal");
                cmdData.vertical = Input.GetAxisRaw("Vertical");

                cmdBuf.AddCommandData(cmdData);
            }
        }
        entities.Dispose();
    }
}