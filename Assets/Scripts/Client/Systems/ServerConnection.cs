using Unity.Entities;
using Unity.Collections;
using Unity.DotsNetKit.NetCode;
using UnityEngine;
using UnityEngine.Ucg.Matchmaking;

[DisableAutoCreation]
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class ServerConnection : ComponentSystem
{
    public static int sNetworkId = -1;
    private string endpoint;
    private Matchmaker matchmaker;

    EntityQuery connectionQuery;
    protected override void OnCreate()
    {
        connectionQuery = GetEntityQuery(
            typeof(NetworkIdComponent),
            ComponentType.Exclude<NetworkStreamInGame>()
        );
        endpoint = "172.17.129.6:30593";
        Debug.Log("Start Running.");
        matchmaker = new Matchmaker(endpoint);
    }

    protected override async void OnStartRunning()
    {
        Debug.Log("Start Requesting Match.");
        matchmaker.RequestMatch("user1");
    }

    protected override void OnUpdate()
    {
        var connectionEntities = connectionQuery.ToEntityArray(Allocator.TempJob);
        var networkIdComps = connectionQuery.ToComponentDataArray<NetworkIdComponent>(Allocator.TempJob);

        if (connectionEntities.Length == 1)
        {
            var connectionEntity = connectionEntities[0];
            var networkIdComp = networkIdComps[0];

            sNetworkId = networkIdComp.Value;
            SimpleConsole.WriteLine(string.Format("NetworkId({0}) Assigned .", sNetworkId));

            EntityManager.AddComponentData(connectionEntity, new NetworkStreamInGame());
        }

        connectionEntities.Dispose();
        networkIdComps.Dispose();
    }
}
