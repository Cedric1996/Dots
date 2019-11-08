using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities;

namespace Unity.DotsNetKit.NetCode
{
    public class SimpleServer
    {
        public static void Start(string appId)
        {
            Debug.Log("ClientServerSystemManager.CollectAllSystems()");
            ClientServerSystemManager.CollectAllSystems();

            Debug.Log("ReplicatedPrefabMgr.Initialize()");
            ReplicatedPrefabMgr.Initialize();

            ClientServerSystemManager.InitServerSystems();

            Unity.DotsNetKit.Transport.NetworkEndPoint ep = Unity.DotsNetKit.Transport.NetworkEndPoint.AnyIpv4;
            ep.Port = 12345;
            World serverWorld = ClientServerSystemManager.serverWorld;
            var nsrs = serverWorld.GetExistingSystem<NetworkStreamReceiveSystem>();
            nsrs.Listen(ep);

            //Console.WriteLine(string.Format("Server is listening on port ({0})", ep.Port));

            string levelName = "Level1";
            SceneManager.LoadScene(levelName);
            //Console.WriteLine(string.Format("Load level ({0})", levelName));
        }
    }
}
