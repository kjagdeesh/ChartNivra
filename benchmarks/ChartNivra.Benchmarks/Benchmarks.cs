namespace ChartNivra.Benchmarks;

using BenchmarkDotNet.Attributes;
using ChartNivra.Core.Axes;
using ChartNivra.Core.Data;
using ChartNivra.Core.Decimation;
using ChartNivra.Core.Primitives;

[MemoryDiagnoser]
public class ChartBenchmarks
{
    private List<ChartPoint> _points = new();
    private StreamingRingBuffer _ringBuffer = new(10_000);

    [GlobalSetup]
    public void Setup()
    {
        _points = new List<ChartPoint>(50_000);
        for (var i = 0; i < 50_000; i++)
        {
            _points.Add(new ChartPoint(i, Math.Sin(i * 0.02) * 100));
        }
    }

    [Benchmark]
    public IReadOnlyList<ChartPoint> BenchmarkLttbDecimation()
    {
        return LttbDecimation.Downsample(_points, 1000);
    }

    [Benchmark]
    public void BenchmarkStreamingRingBuffer()
    {
        for (var i = 0; i < 1000; i++)
        {
            _ringBuffer.Enqueue(new ChartPoint(i, i * 2.0));
        }
    }

    [Benchmark]
    public (double niceMin, double niceMax, double step) BenchmarkNiceScaleCalculator()
    {
        return NiceScaleCalculator.Calculate(-123.45, 9876.54, 8);
    }
}
