using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

namespace Unity.DotsNetKit.NetCode
{
    public class SimpleClient
    {
        public static bool Connect(string appId)
        {
            Debug.Log("ClientServerSystemManager.CollectAllSystems()");
            ClientServerSystemManager.CollectAllSystems();

            Debug.Log("ReplicatedPrefabMgr.Initialize()");
            ReplicatedPrefabMgr.Initialize();

            ClientServerSystemManager.InitClientSystems();

            Unity.DotsNetKit.Transport.NetworkEndPoint ep = Unity.DotsNetKit.Transport.NetworkEndPoint.Parse("127.0.0.1",
                12345);
            World clientWorld = ClientServerSystemManager.clientWorld;
            Entity ent = clientWorld.GetExistingSystem<NetworkStreamReceiveSystem>().Connect(ep);

            // Console.WriteLine("Connecting to server ...");

            return true;
        }
    }
}
