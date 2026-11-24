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
    public ChartColor TrackColor { get; set; } = ChartColor.FromHex("#E2E8F0");
    public ChartColor ValueColor { get; set; } = ChartColor.FromHex("#3B82F6");
    public ChartTheme Theme { get; set; } = ChartTheme.Light;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        canvas.DrawRectangle(bounds, new ChartBrush(Theme.BackgroundColor));

        var center = bounds.Center;
        var radius = (float)(Math.Min(bounds.Width, bounds.Height) * 0.38);

        var ratio = Maximum <= Minimum ? 0 : Math.Clamp((Value - Minimum) / (Maximum - Minimum), 0, 1);
        var innerRadius = Math.Max(0, radius - 12f);

        // Full track and the active portion make the KPI state immediately legible.
        canvas.DrawArc(center, radius, innerRadius, -90, 360, new ChartBrush(TrackColor));
        canvas.DrawArc(center, radius, innerRadius, -90, (float)(360 * ratio), new ChartBrush(ValueColor));

        // A small needle is retained for precise reading without obscuring the progress arc.
        var needleAngle = (-90 + 360 * ratio) * Math.PI / 180;
        var needleEnd = new ChartPoint(center.X + radius * .62 * Math.Cos(needleAngle), center.Y + radius * .62 * Math.Sin(needleAngle));
        canvas.DrawLine(center, needleEnd, new ChartPen(NeedleColor, 2f));
        canvas.DrawCircle(center, 4, new ChartBrush(NeedleColor));

        // Draw center title / value
        var text = $"{Value:F0}{Unit}";
        var size = canvas.MeasureText(text, 20f, isBold: true);
        canvas.DrawText(text, new ChartPoint(center.X - size.Width / 2, center.Y + size.Height / 3), Theme.TextColor, 20f, isBold: true);

        if (!string.IsNullOrEmpty(Title))
        {
            var titleSize = canvas.MeasureText(Title, 13f);
            canvas.DrawText(Title, new ChartPoint(center.X - titleSize.Width / 2, center.Y + radius * 0.7), Theme.SecondaryTextColor, 13f);
        }

        var minText = Minimum.ToString("0");
        var maxText = Maximum.ToString("0");
        canvas.DrawText(minText, new ChartPoint(center.X - radius, center.Y + radius + 14), Theme.SecondaryTextColor, 10);
        var maxSize = canvas.MeasureText(maxText, 10);
        canvas.DrawText(maxText, new ChartPoint(center.X + radius - maxSize.Width, center.Y + radius + 14), Theme.SecondaryTextColor, 10);
    }
}
