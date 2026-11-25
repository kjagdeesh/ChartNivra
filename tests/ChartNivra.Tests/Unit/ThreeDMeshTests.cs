namespace ChartNivra.UnitTests;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.ThreeD;
using System.Numerics;
using Xunit;

public class ThreeDMeshTests
{
    [Fact]
    public void Column3DMeshGenerator_Generates12Triangles()
    {
        var cuboid = Column3DMeshGenerator.CreateCuboid(
            new Vector3(0, 0, 0),
            new Vector3(2, 5, 2),
            ChartColor.FromHex("#3B82F6")
        );

        Assert.Equal(12, cuboid.Count);
        foreach (var tri in cuboid)
        {
            // Verify surface normals are normalized
            Assert.True(MathF.Abs(tri.Normal.Length() - 1f) < 1e-4);
        }
    }

    [Fact]
    public void Light3D_AppliesDiffuseShadingCorrectly()
    {
        var light = new Light3D();
        var baseColor = new ChartColor(200, 200, 200);

        // Facing light
        var litColor = light.ApplyLighting(baseColor, -light.Direction);
        // Opposing light (backface)
        var shadowColor = light.ApplyLighting(baseColor, light.Direction);

        Assert.True(litColor.R > shadowColor.R);
    }
}
