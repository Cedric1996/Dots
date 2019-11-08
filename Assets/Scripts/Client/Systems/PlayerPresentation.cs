using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.DotsNetKit.NetCode;

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientPresentationSystemGroup))]
public class PlayerPresentation : ComponentSystem
{
    EntityQuery createCubeGoQuery;
    EntityQuery updateCubeGoQuery;

    protected override void OnCreate()
    {
        createCubeGoQuery = GetEntityQuery(
            ComponentType.ReadOnly<RepCubeComponentData>(),
            ComponentType.Exclude<RepCubeGoCreated>());
        updateCubeGoQuery = GetEntityQuery(ComponentType.ReadOnly<RepCubeGoCreated>());
    }

    protected override void OnUpdate()
    {
        var cubeEntities = createCubeGoQuery.ToEntityArray(Allocator.TempJob);
        for (int i = 0; i < cubeEntities.Length; ++i)
        {
            var cubeEnt = cubeEntities[i];
            ReplicatedPrefabMgr.LoadPrefabIntoEntity("assets_prefabs_cube", World, cubeEnt);
            EntityManager.AddComponentData(cubeEnt, default(RepCubeGoCreated));

            // TODO: LZ:
            //      the value of the first snapshot seems to be buggy, so the following lines don't work well
            //      we need to fix this!
#if false
            var renderer = EntityManager.GetComponentObject<MeshRenderer>(cubeEnt);
            var cubeData = EntityManager.GetComponentData<RepCubeComponentData>(cubeEnt);
            renderer.material.color = new Color(cubeData.color.x, cubeData.color.y, cubeData.color.z);

            if (cubeData.networkId == ServerConnection.sNetworkId)
                Console.SetColor(renderer.material.color);

            Console.WriteLine(string.Format("Player(NetworkId={0}) joined.", cubeData.networkId));
#endif
        }
        cubeEntities.Dispose();

        var goEntities = updateCubeGoQuery.ToEntityArray(Allocator.TempJob);
        for (int i = 0; i < goEntities.Length; ++i)
        {
            var goEnt = goEntities[i];

            var transform = EntityManager.GetComponentObject<Transform>(goEnt);
            var cubeData = EntityManager.GetComponentData<RepCubeComponentData>(goEnt);
            transform.position = cubeData.position;

            var renderer = EntityManager.GetComponentObject<MeshRenderer>(goEnt);
            renderer.material.color = new Color(cubeData.color.x, cubeData.color.y, cubeData.color.z);
            if (cubeData.networkId == ServerConnection.sNetworkId)
                SimpleConsole.SetColor(renderer.material.color);
        }
        goEntities.Dispose();
    }
}

public struct RepCubeGoCreated : IComponentData { }
