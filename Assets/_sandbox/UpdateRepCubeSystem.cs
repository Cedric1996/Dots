using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Collections;

#if true
public class UpdateRepCubeSystem : ComponentSystem
{
    EntityQuery cubesQuery;
    // Start is called before the first frame update
    protected override void OnCreate()
    {
        cubesQuery = GetEntityQuery(ComponentType.ReadWrite<RepCube2ComponentData>());
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        var ents = cubesQuery.ToEntityArray(Allocator.TempJob);

        for (int i = 0; i < ents.Length; ++i)
        {
            var ent = ents[i];
            RepCube2ComponentData cubeData = default;
            cubeData.position.x = Time.time;
            PostUpdateCommands.SetComponent(ent, cubeData);
        }

        ents.Dispose();
    }
}
#else
public class UpdateRepCubeSystem : MonoBehaviour
{
    RepCube repCube;
    private void Start()
    {
        repCube = GetComponent<RepCube>();
    }
    void Update()
    {
        RepCubeComponentData cubeData = repCube.Value;
        cubeData.position.x = Time.time;
        repCube.Value = cubeData;
    }
}
#endif