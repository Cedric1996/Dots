using System;
using Unity.Entities;
using Unity.DotsNetKit.Transport;
using Unity.DotsNetKit.NetCode;

public struct RpcCollection : IRpcCollection
{
    static Type[] s_RpcTypes = new Type[]
    {
        typeof(RpcLoadLevel),

    };
    public void ExecuteRpc(int type, DataStreamReader reader, ref DataStreamReader.Context ctx, Entity connection, EntityCommandBuffer.Concurrent commandBuffer, int jobIndex)
    {
        switch (type)
        {
            case 0:
            {
                var tmp = new RpcLoadLevel();
                tmp.Deserialize(reader, ref ctx);
                tmp.Execute(connection, commandBuffer, jobIndex);
                break;
            }

        }
    }

    public void ExecuteRpc(int type, DataStreamReader reader, ref DataStreamReader.Context ctx, Entity connection, EntityCommandBuffer commandBuffer)
    {
        switch (type)
        {
            case 0:
            {
                var tmp = new RpcLoadLevel();
                tmp.Deserialize(reader, ref ctx);
                tmp.Execute(connection, commandBuffer);
                break;
            }

        }
    }

    public int GetRpcFromType<T>() where T : struct, IRpcCommand
    {
        for (int i = 0; i < s_RpcTypes.Length; ++i)
        {
            if (s_RpcTypes[i] == typeof(T))
                return i;
        }

        return -1;
    }
}

public class DotsNetKit193RpcSystem : RpcSystem<RpcCollection>
{
}
