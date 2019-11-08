using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.DotsNetKit.Transport;
using Unity.DotsNetKit.NetCode;
using UnityEngine.SceneManagement;

public struct RpcLoadLevel : IRpcCommand
{
    public string levelName;

    public void Execute(Entity connection, EntityCommandBuffer.Concurrent commandBuffer, int jobIndex)
    {
        SimpleConsole.WriteLine(string.Format("RPC Load level ({0})", levelName));

        SceneManager.LoadScene(levelName);
    }

    public void Execute(Entity connection, EntityCommandBuffer commandBuffer)
    {
        SimpleConsole.WriteLine(string.Format("RPC Load level ({0})", levelName));

        SceneManager.LoadScene(levelName);
    }

    public void Serialize(DataStreamWriter writer)
    {
        writer.WriteUnicodeString(levelName);
    }

    public void Deserialize(DataStreamReader reader, ref DataStreamReader.Context ctx)
    {
        levelName = reader.ReadUnicodeString(ref ctx);
    }
}
