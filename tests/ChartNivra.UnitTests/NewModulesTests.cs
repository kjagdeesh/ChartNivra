namespace ChartNivra.UnitTests;

using ChartNivra.Export;
using ChartNivra.Gauges;
using ChartNivra.Statistical;
using Xunit;

public class NewModulesTests
{
    [Fact]
    public void HistogramCalculator_ComputesCorrectBins()
    {
        var data = new List<double> { 1, 2, 2, 3, 5, 8, 9, 10 };
        var (width, bins, min, max) = HistogramCalculator.ComputeBins(data, 3);

        Assert.Equal(3, bins.Length);
        Assert.Equal(8, bins.Sum());
    }

    [Fact]
    public void RadialGaugeModel_InstantiatesWithDefaults()
    {
        var gauge = new RadialGaugeModel { Value = 75 };
        Assert.Equal(75, gauge.Value);
    }

    [Fact]
    public void ChartImageExporter_ExportsValidPng()
    {
        var gauge = new RadialGaugeModel { Value = 50 };
        var pngBytes = ChartImageExporter.ExportToPng(gauge, 200, 200);

        Assert.NotNull(pngBytes);
        Assert.True(pngBytes.Length > 0);
        // Verify PNG magic header bytes: 0x89 0x50 0x4E 0x47
        Assert.Equal(0x89, pngBytes[0]);
        Assert.Equal(0x50, pngBytes[1]);
        Assert.Equal(0x4E, pngBytes[2]);
        Assert.Equal(0x47, pngBytes[3]);
    }
}
