using Unity.Entities;

namespace Unity.DotsNetKit.NetCode
{
    public struct OutgoingRpcDataStreamBufferComponent : IBufferElementData
    {
        public byte Value;
    }
    public struct IncomingRpcDataStreamBufferComponent : IBufferElementData
    {
        public byte Value;
    }
}