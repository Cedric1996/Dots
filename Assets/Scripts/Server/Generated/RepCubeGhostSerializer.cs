using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Transforms;
using Unity.DotsNetKit.NetCode;

public struct RepCubeGhostSerializer : IGhostSerializer<RepCubeSnapshotData>
{
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    private ComponentType componentTypeRepCubeComponentData;
    [NativeDisableContainerSafetyRestriction] private ArchetypeChunkComponentType<RepCubeComponentData> ghostRepCubeComponentDataType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public bool WantsPredictionDelta => true;

    public int SnapshotSize => UnsafeUtility.SizeOf<RepCubeSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeRepCubeComponentData = ComponentType.ReadWrite<RepCubeComponentData>();
        ghostRepCubeComponentDataType = system.GetArchetypeChunkComponentType<RepCubeComponentData>();

    }

    public bool CanSerialize(EntityArchetype arch)
    {
        var components = arch.GetComponentTypes();
        int matches = 0;
        for (int i = 0; i < components.Length; ++i)
        {
            if (components[i] == componentTypeRepCubeComponentData)
                ++matches;

        }
        return (matches == 1);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref RepCubeSnapshotData snapshot)
    {
        snapshot.tick = tick;
        var chunkDataRepCubeComponentData = chunk.GetNativeArray(ghostRepCubeComponentDataType);
        snapshot.SetRepCubeComponentDatanetworkId(chunkDataRepCubeComponentData[ent].networkId);
        snapshot.SetRepCubeComponentDataposition(chunkDataRepCubeComponentData[ent].position);
        snapshot.SetRepCubeComponentDatacolor(chunkDataRepCubeComponentData[ent].color);

    }
}
