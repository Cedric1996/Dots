using Unity.Collections;
using Unity.Entities;
using Unity.DotsNetKit.Transport;

namespace Unity.DotsNetKit.NetCode
{
    public interface IGhostSerializerCollection
    {
        int FindSerializer(EntityArchetype arch);

        void BeginSerialize(ComponentSystemBase system);

        int CalculateImportance(int serializer, ArchetypeChunk chunk);

        bool WantsPredictionDelta(int serializer);
        int GetSnapshotSize(int serializer);

        unsafe int Serialize(int serializer, ArchetypeChunk chunk, int startIndex, uint currentTick,
            Entity* currentSnapshotEntity, void* currentSnapshotData,
            GhostSystemStateComponent* ghosts, NativeArray<Entity> ghostEntities,
            NativeArray<int> baselinePerEntity, NativeList<SnapshotBaseline> availableBaselines,
            DataStreamWriter dataStream, NetworkCompressionModel compressionModel);

    }
}