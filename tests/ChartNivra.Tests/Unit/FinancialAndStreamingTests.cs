namespace ChartNivra.UnitTests;

using ChartNivra.Core.Data;
using ChartNivra.Core.Financial;
using ChartNivra.Core.Primitives;
using Xunit;

public class FinancialAndStreamingTests
{
    [Fact]
    public void TechnicalIndicators_SMA_ComputesCorrectAverages()
    {
        var data = new List<FinancialPoint>
        {
            new(DateTime.UtcNow, 10, 15, 8, 12),
            new(DateTime.UtcNow, 12, 16, 11, 14),
            new(DateTime.UtcNow, 14, 18, 13, 16),
            new(DateTime.UtcNow, 16, 20, 15, 18),
            new(DateTime.UtcNow, 18, 22, 17, 20)
        };

        // 3-period SMA of Close: [12, 14, 16] -> 14, [14, 16, 18] -> 16, [16, 18, 20] -> 18
        var sma = TechnicalIndicators.CalculateSma(data, 3);
        Assert.Equal(3, sma.Count);
        Assert.Equal(14.0, sma[0].Y);
        Assert.Equal(16.0, sma[1].Y);
        Assert.Equal(18.0, sma[2].Y);
    }

    [Fact]
    public void StreamingRingBuffer_RespectsCapacityAndFIFO()
    {
        var buffer = new StreamingRingBuffer(3);
        buffer.Enqueue(new ChartPoint(1, 10));
        buffer.Enqueue(new ChartPoint(2, 20));
        buffer.Enqueue(new ChartPoint(3, 30));
        buffer.Enqueue(new ChartPoint(4, 40)); // Evicts point 1

        var snapshot = buffer.ToSnapshot();
        Assert.Equal(3, snapshot.Count);
        Assert.Equal(2, snapshot[0].X);
        Assert.Equal(3, snapshot[1].X);
        Assert.Equal(4, snapshot[2].X);
    }
}
