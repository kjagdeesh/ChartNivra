namespace ChartNivra.Export;

using ChartNivra.Core.Primitives;
using ChartNivra.Rendering.Abstractions;
using ChartNivra.Rendering.Skia;
using SkiaSharp;

/// <summary>
/// Off-screen raster image exporter (PNG / JPEG).
/// </summary>
public static class ChartImageExporter
{
    public static byte[] ExportToPng(IChartRenderer renderer, int width, int height)
    {
        using var surface = SKSurface.Create(new SKImageInfo(width, height, SKColorType.Rgba8888, SKAlphaType.Premul));
        using (var canvas = new SkiaChartCanvas(surface.Canvas))
        {
            renderer.Render(canvas, new ChartRect(0, 0, width, height));
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }
}
