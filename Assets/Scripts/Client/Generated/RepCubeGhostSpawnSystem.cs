using Unity.Entities;
using Unity.Transforms;
using Unity.DotsNetKit.NetCode;

[DisableAutoCreation]
public partial class RepCubeGhostSpawnSystem : DefaultGhostSpawnSystem<RepCubeSnapshotData>
{
    protected override EntityArchetype GetGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepCubeSnapshotData>(),
            ComponentType.ReadWrite<RepCubeComponentData>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>()
        );
    }
    protected override EntityArchetype GetPredictedGhostArchetype()
    {
        return EntityManager.CreateArchetype(
            ComponentType.ReadWrite<RepCubeSnapshotData>(),
            ComponentType.ReadWrite<RepCubeComponentData>(),

            ComponentType.ReadWrite<ReplicatedEntityComponent>(),
            ComponentType.ReadWrite<PredictedEntityComponent>()
        );
    }
}

[DisableAutoCreation]
public partial class RepCubeGhostDestroySystem : DefaultGhostDestroySystem<RepCubeSnapshotData>
{
}
