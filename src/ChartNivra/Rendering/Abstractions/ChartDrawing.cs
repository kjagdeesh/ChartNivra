namespace ChartNivra.Rendering.Abstractions;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;

/// <summary>Shared drawing helpers used by the built-in chart renderers.</summary>
public static class ChartDrawing
{
    /// <summary>Draws the chart title at the top-left and returns the vertical space it consumed.</summary>
    public static double DrawTitle(IChartCanvas canvas, string? title, ChartRect bounds, ChartTheme theme)
    {
        if (string.IsNullOrEmpty(title)) return 16;
        canvas.DrawText(title, new ChartPoint(bounds.Left + 16, bounds.Top + 26), theme.TextColor, 15f, isBold: true);
        return 44;
    }

    /// <summary>Draws a centered legend along the bottom edge of <paramref name="bounds"/>.</summary>
    public static void DrawLegend(IChartCanvas canvas, IReadOnlyList<(string Label, ChartColor Color)> items, ChartRect bounds, ChartTheme theme)
    {
        if (items.Count == 0) return;

        const float font = 12f;
        const double swatch = 10, gap = 6, spacing = 18;

        var widths = new double[items.Count];
        double total = 0;
        for (var i = 0; i < items.Count; i++)
        {
            widths[i] = swatch + gap + canvas.MeasureText(items[i].Label, font).Width;
            total += widths[i] + (i < items.Count - 1 ? spacing : 0);
        }

        var x = bounds.Left + Math.Max(8, (bounds.Width - total) / 2);
        var y = bounds.Bottom - 14;
        for (var i = 0; i < items.Count; i++)
        {
            canvas.DrawCircle(new ChartPoint(x + swatch / 2, y - 4), (float)(swatch / 2), new ChartBrush(items[i].Color));
            canvas.DrawText(items[i].Label, new ChartPoint(x + swatch + gap, y), theme.SecondaryTextColor, font);
            x += widths[i] + spacing;
        }
    }

    /// <summary>Catmull-Rom spline through the given screen points.</summary>
    public static List<ChartPoint> Smooth(IReadOnlyList<ChartPoint> p, int segments = 10)
    {
        var result = new List<ChartPoint>();
        if (p.Count < 3)
        {
            result.AddRange(p);
            return result;
        }

        for (var i = 0; i < p.Count - 1; i++)
        {
            var p0 = p[Math.Max(i - 1, 0)];
            var p1 = p[i];
            var p2 = p[i + 1];
            var p3 = p[Math.Min(i + 2, p.Count - 1)];

            for (var s = 0; s < segments; s++)
            {
                var t = s / (double)segments;
                var t2 = t * t;
                var t3 = t2 * t;
                var x = 0.5 * (2 * p1.X + (-p0.X + p2.X) * t + (2 * p0.X - 5 * p1.X + 4 * p2.X - p3.X) * t2 + (-p0.X + 3 * p1.X - 3 * p2.X + p3.X) * t3);
                var y = 0.5 * (2 * p1.Y + (-p0.Y + p2.Y) * t + (2 * p0.Y - 5 * p1.Y + 4 * p2.Y - p3.Y) * t2 + (-p0.Y + 3 * p1.Y - 3 * p2.Y + p3.Y) * t3);
                result.Add(new ChartPoint(x, y));
            }
        }

        result.Add(p[^1]);
        return result;
    }
}
