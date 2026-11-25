namespace ChartNivra.IntegrationTests;

using ChartNivra.Cartesian;
using ChartNivra.Core.Financial;
using ChartNivra.Core.Primitives;
using ChartNivra.Data;
using ChartNivra.Financial;
using ChartNivra.Themes;
using Xunit;

public class IntegrationTests
{
    [Fact]
    public void StreamingPipeline_IntegratesWithRollingWindow()
    {
        var pipeline = new StreamingDataPipeline(50);
        var flushedCount = 0;

        pipeline.DataFlushed += snapshot =>
        {
            flushedCount = snapshot.Count;
        };

        for (var i = 0; i < 75; i++)
        {
            pipeline.Push(i, i * 2.0);
        }

        pipeline.Flush();

        var current = pipeline.GetCurrentData();
        Assert.Equal(50, current.Count);
        Assert.Equal(50, flushedCount);
        Assert.Equal(25, current[0].X); // Oldest item after rolling
    }

    [Fact]
    public void FinancialPipeline_CalculatesIndicatorsAndOverlays()
    {
        var chart = new FinancialChartModel();
        var date = DateTime.Today;

        for (var i = 0; i < 30; i++)
        {
            chart.Candlesticks.Data.Add(new FinancialPoint(date.AddDays(i), 100 + i, 105 + i, 98 + i, 102 + i, 1000));
        }

        chart.AddSmaOverlay(10, ChartColor.FromHex("#3B82F6"));
        Assert.Single(chart.Overlays);
        Assert.Equal(21, chart.Overlays[0].Points.Count); // 30 - 10 + 1
    }

    [Fact]
    public void ExtendedPalettes_ResolveColorsCorrectly()
    {
        var coral = ExtendedPalettes.Coral;
        Assert.Equal("Coral", coral.Name);
        Assert.Equal(5, coral.Colors.Count);

        var c0 = coral.GetColor(0);
        var c5 = coral.GetColor(5); // Should wrap around to 0
        Assert.Equal(c0, c5);
    }
}
