namespace ChartNivra.CrossPlatformTests;

using System.Numerics;
using ChartNivra.Core.Axes;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.ThreeD;
using ChartNivra.Core.Transforms;
using Xunit;

public class CrossPlatformTests
{
    [Theory]
    [InlineData(320, 480)]   // Mobile portrait
    [InlineData(800, 600)]   // Tablet
    [InlineData(1920, 1080)] // Desktop FHD
    [InlineData(3840, 2160)] // Desktop 4K
    public void CartesianTransform_ComputesValidCoordinates_AcrossAspectRatios(double width, double height)
    {
        var viewport = new ChartRect(0, 0, width, height);
        var xRange = new DataRange(0, 100);
        var yRange = new DataRange(0, 500);

        var transform = new CartesianTransform(xRange, yRange, viewport);

        var screenOrigin = transform.DataToPixel(0, 0);
        var screenMax = transform.DataToPixel(100, 500);

        Assert.True(screenOrigin.X >= 0 && screenOrigin.X <= width);
        Assert.True(screenOrigin.Y >= 0 && screenOrigin.Y <= height);
        Assert.True(screenMax.X >= 0 && screenMax.X <= width);
        Assert.True(screenMax.Y >= 0 && screenMax.Y <= height);
    }

    [Fact]
    public void Camera3D_ProjectsValidPoints_Headlessly()
    {
        var camera = new Camera3D
        {
            Position = new Vector3(0f, 5f, 10f),
            Target = Vector3.Zero
        };

        var viewport = new ChartRect(0, 0, 800, 600);
        var projected = camera.Project(new Vector3D(0, 0, 0), viewport);

        Assert.False(projected.IsEmpty);
        Assert.True(projected.X > 0 && projected.X < 800);
        Assert.True(projected.Y > 0 && projected.Y < 600);
    }
}
