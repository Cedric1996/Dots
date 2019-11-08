using Unity.Entities;
using Unity.Mathematics;

namespace Unity.DotsNetKit.NetCode
{
    public struct CurrentSimulatedPosition : IComponentData
    {
        public float3 Value;
    }
    public struct PreviousSimulatedPosition : IComponentData
    {
        public float3 Value;
    }
    public struct CurrentSimulatedRotation : IComponentData
    {
        public quaternion Value;
    }
    public struct PreviousSimulatedRotation : IComponentData
    {
        public quaternion Value;
    }
}