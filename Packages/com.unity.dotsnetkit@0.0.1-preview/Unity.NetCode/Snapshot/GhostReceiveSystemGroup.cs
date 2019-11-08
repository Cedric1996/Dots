using Unity.Collections;
using Unity.Entities;

namespace Unity.DotsNetKit.NetCode
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    [UpdateAfter(typeof(NetworkStreamReceiveSystem))]
    public class GhostReceiveSystemGroup : ComponentSystemGroup
    {
        // having the group own the ghost map is a bit of a hack to solve a problem with accessing the receiver system from the default spawn system (because it is generic)
        protected override void OnCreate()
        {
            m_ghostEntityMap = new NativeHashMap<int, GhostEntity>(2048, Allocator.Persistent);
        }

        protected override void OnDestroy()
        {
            m_ghostEntityMap.Dispose();
        }
        internal NativeHashMap<int, GhostEntity> GhostEntityMap => m_ghostEntityMap;
        private NativeHashMap<int, GhostEntity> m_ghostEntityMap;

    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(GhostReceiveSystemGroup))]
    public class GhostUpdateSystemGroup : ComponentSystemGroup
    {
    }

    [DisableAutoCreation]
    [UpdateInGroup(typeof(ClientSimulationSystemGroup))]
    public class GhostSpawnSystemGroup : ComponentSystemGroup
    { }
}
