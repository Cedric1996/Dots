using Unity.Mathematics;
using Unity.DotsNetKit.Transport;
using Unity.DotsNetKit.NetCode;

public struct RepCubeSnapshotData : ISnapshotData<RepCubeSnapshotData>
{
    public uint tick;
    int RepCubeComponentDatanetworkId;
    int RepCubeComponentDatapositionX;
    int RepCubeComponentDatapositionY;
    int RepCubeComponentDatapositionZ;
    int RepCubeComponentDatacolorX;
    int RepCubeComponentDatacolorY;
    int RepCubeComponentDatacolorZ;


    public uint Tick => tick;
    public int GetRepCubeComponentDatanetworkId()
    {
        return RepCubeComponentDatanetworkId;
    }
    public void SetRepCubeComponentDatanetworkId(int val)
    {
        RepCubeComponentDatanetworkId = val;
    }
    public float3 GetRepCubeComponentDataposition()
    {
        return new float3(RepCubeComponentDatapositionX, RepCubeComponentDatapositionY, RepCubeComponentDatapositionZ) * 0.01f;
    }
    public void SetRepCubeComponentDataposition(float3 val)
    {
        RepCubeComponentDatapositionX = (int)(val.x * 100);
        RepCubeComponentDatapositionY = (int)(val.y * 100);
        RepCubeComponentDatapositionZ = (int)(val.z * 100);
    }
    public float3 GetRepCubeComponentDatacolor()
    {
        return new float3(RepCubeComponentDatacolorX, RepCubeComponentDatacolorY, RepCubeComponentDatacolorZ) * 0.001f;
    }
    public void SetRepCubeComponentDatacolor(float3 val)
    {
        RepCubeComponentDatacolorX = (int)(val.x * 1000);
        RepCubeComponentDatacolorY = (int)(val.y * 1000);
        RepCubeComponentDatacolorZ = (int)(val.z * 1000);
    }


    public void PredictDelta(uint tick, ref RepCubeSnapshotData baseline1, ref RepCubeSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        RepCubeComponentDatanetworkId = predictor.PredictInt(RepCubeComponentDatanetworkId, baseline1.RepCubeComponentDatanetworkId, baseline2.RepCubeComponentDatanetworkId);
        RepCubeComponentDatapositionX = predictor.PredictInt(RepCubeComponentDatapositionX, baseline1.RepCubeComponentDatapositionX, baseline2.RepCubeComponentDatapositionX);
        RepCubeComponentDatapositionY = predictor.PredictInt(RepCubeComponentDatapositionY, baseline1.RepCubeComponentDatapositionY, baseline2.RepCubeComponentDatapositionY);
        RepCubeComponentDatapositionZ = predictor.PredictInt(RepCubeComponentDatapositionZ, baseline1.RepCubeComponentDatapositionZ, baseline2.RepCubeComponentDatapositionZ);
        RepCubeComponentDatacolorX = predictor.PredictInt(RepCubeComponentDatacolorX, baseline1.RepCubeComponentDatacolorX, baseline2.RepCubeComponentDatacolorX);
        RepCubeComponentDatacolorY = predictor.PredictInt(RepCubeComponentDatacolorY, baseline1.RepCubeComponentDatacolorY, baseline2.RepCubeComponentDatacolorY);
        RepCubeComponentDatacolorZ = predictor.PredictInt(RepCubeComponentDatacolorZ, baseline1.RepCubeComponentDatacolorZ, baseline2.RepCubeComponentDatacolorZ);

    }

    public void Serialize(ref RepCubeSnapshotData baseline, DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        writer.WritePackedIntDelta(RepCubeComponentDatanetworkId, baseline.RepCubeComponentDatanetworkId, compressionModel);
        writer.WritePackedIntDelta(RepCubeComponentDatapositionX, baseline.RepCubeComponentDatapositionX, compressionModel);
        writer.WritePackedIntDelta(RepCubeComponentDatapositionY, baseline.RepCubeComponentDatapositionY, compressionModel);
        writer.WritePackedIntDelta(RepCubeComponentDatapositionZ, baseline.RepCubeComponentDatapositionZ, compressionModel);
        writer.WritePackedIntDelta(RepCubeComponentDatacolorX, baseline.RepCubeComponentDatacolorX, compressionModel);
        writer.WritePackedIntDelta(RepCubeComponentDatacolorY, baseline.RepCubeComponentDatacolorY, compressionModel);
        writer.WritePackedIntDelta(RepCubeComponentDatacolorZ, baseline.RepCubeComponentDatacolorZ, compressionModel);

    }

    public void Deserialize(uint tick, ref RepCubeSnapshotData baseline, DataStreamReader reader, ref DataStreamReader.Context ctx,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        RepCubeComponentDatanetworkId = reader.ReadPackedIntDelta(ref ctx, baseline.RepCubeComponentDatanetworkId, compressionModel);
        RepCubeComponentDatapositionX = reader.ReadPackedIntDelta(ref ctx, baseline.RepCubeComponentDatapositionX, compressionModel);
        RepCubeComponentDatapositionY = reader.ReadPackedIntDelta(ref ctx, baseline.RepCubeComponentDatapositionY, compressionModel);
        RepCubeComponentDatapositionZ = reader.ReadPackedIntDelta(ref ctx, baseline.RepCubeComponentDatapositionZ, compressionModel);
        RepCubeComponentDatacolorX = reader.ReadPackedIntDelta(ref ctx, baseline.RepCubeComponentDatacolorX, compressionModel);
        RepCubeComponentDatacolorY = reader.ReadPackedIntDelta(ref ctx, baseline.RepCubeComponentDatacolorY, compressionModel);
        RepCubeComponentDatacolorZ = reader.ReadPackedIntDelta(ref ctx, baseline.RepCubeComponentDatacolorZ, compressionModel);

    }
    public void Interpolate(ref RepCubeSnapshotData target, float factor)
    {
        SetRepCubeComponentDataposition(math.lerp(GetRepCubeComponentDataposition(), target.GetRepCubeComponentDataposition(), factor));
        SetRepCubeComponentDatacolor(math.lerp(GetRepCubeComponentDatacolor(), target.GetRepCubeComponentDatacolor(), factor));

    }
}