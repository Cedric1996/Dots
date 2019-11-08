using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct RepCube2ComponentData : IComponentData
{
    public int networkId;
    public float3 position;
    public float3 color;
}

public class RepCube2 : ComponentDataProxy<RepCube2ComponentData>
{ }