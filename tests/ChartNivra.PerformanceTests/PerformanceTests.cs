namespace ChartNivra.PerformanceTests;

using System.Diagnostics;
using ChartNivra.Core.Data;
using ChartNivra.Core.Decimation;
using ChartNivra.Core.Primitives;
using Xunit;

public class PerformanceTests
{
    [Fact]
    public void LttbDecimation_Downsamples100kPoints_Under200Milliseconds()
    {
        const int pointCount = 100_000;
        var points = new List<ChartPoint>(pointCount);
        for (var i = 0; i < pointCount; i++)
        {
            points.Add(new ChartPoint(i, Math.Sin(i * 0.01) * 50));
        }

        var sw = Stopwatch.StartNew();
        var decimated = LttbDecimation.Downsample(points, 1000);
        sw.Stop();

        Assert.Equal(1000, decimated.Count);
        Assert.True(sw.ElapsedMilliseconds < 500, $"LTTB decimation took {sw.ElapsedMilliseconds} ms, expected < 500 ms");
    }

    [Fact]
    public void StreamingRingBuffer_Appends100kItems_Efficiently()
    {
        var buffer = new StreamingRingBuffer(10_000);
        var sw = Stopwatch.StartNew();

        for (var i = 0; i < 100_000; i++)
        {
            buffer.Enqueue(new ChartPoint(i, i * 0.5));
        }
        sw.Stop();

        Assert.Equal(10_000, buffer.Count);
        Assert.True(sw.ElapsedMilliseconds < 250, $"RingBuffer 100k writes took {sw.ElapsedMilliseconds} ms, expected < 250 ms");
    }
}
