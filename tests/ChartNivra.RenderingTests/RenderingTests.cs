namespace ChartNivra.RenderingTests;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Export;
using ChartNivra.Rendering.Abstractions;
using ChartNivra.Rendering.Skia;
using SkiaSharp;
using Xunit;

public class RenderingTests
{
    [Fact]
    public void SkiaChartCanvas_DrawsLine_WithoutExceptions()
    {
        using var surface = SKSurface.Create(new SKImageInfo(400, 300));
        var canvas = new SkiaChartCanvas(surface.Canvas);

        canvas.DrawLine(new ChartPoint(10, 10), new ChartPoint(100, 100), new ChartPen(ChartColor.FromHex("#00C096"), 2f));
        Assert.NotNull(surface);
    }

    [Fact]
    public void ChartImageExporter_ExportsValidPngBytes()
    {
        var model = new CartesianChartModel
        {
            Title = "Offscreen Export Test",
            Theme = ChartTheme.CleanLight
        };
        var series = new LineSeriesModel
        {
            Title = "Test Series",
            Points = new List<ChartPoint> { new(0, 10), new(1, 20), new(2, 15) }
        };
        model.Series.Add(series);

        var renderer = new CartesianChartRenderer(model);
        var bytes = ChartImageExporter.ExportToPng(renderer, 600, 400);

        Assert.NotNull(bytes);
        Assert.True(bytes.Length > 100);
        // Verify PNG signature (first 4 bytes: 137, 80, 78, 71)
        Assert.Equal(0x89, bytes[0]);
        Assert.Equal(0x50, bytes[1]);
        Assert.Equal(0x4E, bytes[2]);
        Assert.Equal(0x47, bytes[3]);
    }
}
