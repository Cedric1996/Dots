using Unity.Entities;

namespace Unity.DotsNetKit.NetCode
{
    public struct CommandTargetComponent : IComponentData
    {
        public Entity targetEntity;
    }
}

