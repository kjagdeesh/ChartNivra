namespace ChartNivra.Gauges;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Abstractions;

/// <summary>
/// Model and renderer for radial and circular progress gauges.
/// </summary>
public class RadialGaugeModel : IChartRenderer
{
    public string Title { get; set; } = string.Empty;
    public double Value { get; set; } = 65;
    public double Minimum { get; set; } = 0;
    public double Maximum { get; set; } = 100;
    public string Unit { get; set; } = "%";

    public ChartColor NeedleColor { get; set; } = ChartColor.FromHex("#EF4444");
    public ChartColor TrackColor { get; set; } = ChartColor.FromHex("#334155");
    public ChartColor ValueColor { get; set; } = ChartColor.FromHex("#3B82F6");
    public ChartTheme Theme { get; set; } = ChartTheme.Light;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        canvas.DrawRectangle(bounds, new ChartBrush(Theme.BackgroundColor));

        var center = bounds.Center;
        var radius = (float)(Math.Min(bounds.Width, bounds.Height) * 0.38);

        // Draw track ring
        canvas.DrawCircle(center, radius, null, new ChartPen(TrackColor, 12f));

        // Draw center title / value
        var text = $"{Value:F0}{Unit}";
        var size = canvas.MeasureText(text, 20f, isBold: true);
        canvas.DrawText(text, new ChartPoint(center.X - size.Width / 2, center.Y + size.Height / 3), Theme.TextColor, 20f, isBold: true);

        if (!string.IsNullOrEmpty(Title))
        {
            var titleSize = canvas.MeasureText(Title, 13f);
            canvas.DrawText(Title, new ChartPoint(center.X - titleSize.Width / 2, center.Y + radius * 0.7), Theme.SecondaryTextColor, 13f);
        }
    }
}
