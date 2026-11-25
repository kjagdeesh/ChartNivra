namespace ChartNivra.UnitTests;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Decimation;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Transforms;
using Xunit;

public class CoreTests
{
    [Fact]
    public void NiceScaleCalculator_GeneratesReasonableTicks()
    {
        var (min, max, step) = NiceScaleCalculator.Calculate(0, 95, 5);
        Assert.True(min <= 0);
        Assert.True(max >= 95);
        Assert.True(step > 0);
    }

    [Fact]
    public void CartesianTransform_CorrectlyTransformsCoordinates()
    {
        var xRange = new DataRange(0, 100);
        var yRange = new DataRange(0, 50);
        var plotArea = new ChartRect(0, 0, 200, 100);

        var transform = new CartesianTransform(xRange, yRange, plotArea);

        // Origin (0, 0) in Cartesian maps to bottom-left (0, 100) on screen
        var screenOrigin = transform.DataToPixel(0, 0);
        Assert.Equal(0, screenOrigin.X);
        Assert.Equal(100, screenOrigin.Y);

        // Max (100, 50) maps to top-right (200, 0) on screen
        var screenMax = transform.DataToPixel(100, 50);
        Assert.Equal(200, screenMax.X);
        Assert.Equal(0, screenMax.Y);

        // Roundtrip PixelToData
        var dataPt = transform.PixelToData(100, 50);
        Assert.Equal(50, dataPt.X);
        Assert.Equal(25, dataPt.Y);
    }

    [Fact]
    public void LttbDecimation_ReducesPointsAccurately()
    {
        var points = new List<ChartPoint>();
        for (var i = 0; i < 1000; i++)
        {
            points.Add(new ChartPoint(i, Math.Sin(i * 0.1)));
        }

        var downsampled = LttbDecimation.Downsample(points, 50);
        Assert.Equal(50, downsampled.Count);
        Assert.Equal(points[0], downsampled[0]);
        Assert.Equal(points[^1], downsampled[^1]);
    }
}
