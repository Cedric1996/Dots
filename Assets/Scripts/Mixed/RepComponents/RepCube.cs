using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct RepCubeComponentData : IComponentData
{
    public int networkId;
    public float3 position;
    public float3 color;
}
