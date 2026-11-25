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

        var ratio = Maximum <= Minimum ? 0 : Math.Clamp((Value - Minimum) / (Maximum - Minimum), 0, 1);
        var radius = (float)Math.Max(20, Math.Min(bounds.Width * .32, bounds.Height * .33));
        var center = new ChartPoint(bounds.Center.X, bounds.Top + bounds.Height * .53);
        const float start = 135;
        const float sweep = 270;
        const float thickness = 14;
        canvas.DrawArc(center, radius, 0, start, sweep, null, new ChartPen(TrackColor, thickness));
        if (ratio > 0)
            canvas.DrawArc(center, radius, 0, start, (float)(sweep * ratio), null, new ChartPen(ValueColor, thickness));

        var endAngle = (start + sweep * ratio) * Math.PI / 180;
        var endPoint = new ChartPoint(center.X + radius * Math.Cos(endAngle), center.Y + radius * Math.Sin(endAngle));
        canvas.DrawCircle(endPoint, 4, new ChartBrush(Theme.BackgroundColor), new ChartPen(ValueColor, 2));
        var pointerInner = new ChartPoint(center.X + (radius - 24) * Math.Cos(endAngle), center.Y + (radius - 24) * Math.Sin(endAngle));
        canvas.DrawLine(pointerInner, endPoint, new ChartPen(NeedleColor, 2));

        var text = $"{Value:F0}{Unit}";
        var fontSize = bounds.Width < 260 ? 25f : 32f;
        var size = canvas.MeasureText(text, fontSize, isBold: true);
        canvas.DrawText(text, new ChartPoint(center.X - size.Width / 2, center.Y + 3), Theme.TextColor, fontSize, isBold: true);

        if (!string.IsNullOrEmpty(Title))
        {
            var titleSize = canvas.MeasureText(Title, 13f);
            canvas.DrawText(Title, new ChartPoint(center.X - titleSize.Width / 2, center.Y + 26), Theme.SecondaryTextColor, 13f);
        }

        var minText = Minimum.ToString("0");
        var maxText = Maximum.ToString("0");
        var endOffset = radius * .707;
        var minSize = canvas.MeasureText(minText, 10);
        canvas.DrawText(minText, new ChartPoint(center.X - endOffset - minSize.Width / 2, center.Y + endOffset + 24), Theme.SecondaryTextColor, 10);
        var maxSize = canvas.MeasureText(maxText, 10);
        canvas.DrawText(maxText, new ChartPoint(center.X + endOffset - maxSize.Width / 2, center.Y + endOffset + 24), Theme.SecondaryTextColor, 10);
    }
}
