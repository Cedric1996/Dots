using Unity.Entities;
using Unity.DotsNetKit.Transport;

namespace Unity.DotsNetKit.NetCode
{
    public interface IRpcCollection
    {
        void ExecuteRpc(int type, DataStreamReader reader, ref DataStreamReader.Context ctx, Entity connection, EntityCommandBuffer.Concurrent commandBuffer, int jobIndex);
        void ExecuteRpc(int type, DataStreamReader reader, ref DataStreamReader.Context ctx, Entity connection, EntityCommandBuffer commandBuffer);
        int GetRpcFromType<T>() where T : struct, IRpcCommand;
    }
}