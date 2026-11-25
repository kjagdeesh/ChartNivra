namespace ChartNivra.Rendering.Skia;

using ChartNivra.Cartesian;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Abstractions;

/// <summary>Renders a compact bullet chart with qualitative ranges and a target marker.</summary>
public sealed class BulletChartRenderer : IChartRenderer
{
    public ChartTheme Theme { get; set; } = ChartTheme.Light;
    private readonly BulletChartModel _model;
    public BulletChartRenderer(BulletChartModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = Theme;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16, true);
        if (_model.Maximum <= 0 || bounds.Width < 100 || bounds.Height < 100) return;
        var leftMargin = 32d;
        var plot = new ChartRect(bounds.Left + leftMargin, bounds.Top + Math.Min(140, bounds.Height * .42), bounds.Width - leftMargin * 2, 38);
        var valueText = $"{_model.Value:0.#}";
        canvas.DrawText(valueText, new ChartPoint(plot.Left, plot.Top - 30), theme.TextColor, 28, true);
        var valueSize = canvas.MeasureText(valueText, 28, true);
        canvas.DrawText($"/ {_model.Maximum:0.#}", new ChartPoint(plot.Left + valueSize.Width + 8, plot.Top - 30), theme.SecondaryTextColor, 12);
        var previous = 0d;
        var bands = Theme.Name == ChartTheme.Dark.Name
            ? new[] { ChartColor.FromHex("#334155"), ChartColor.FromHex("#40526A"), ChartColor.FromHex("#50657D") }
            : new[] { ChartColor.FromHex("#E8EEF5"), ChartColor.FromHex("#D6E2F0"), ChartColor.FromHex("#C5D8EC") };
        canvas.DrawRoundRect(plot, 8, new ChartBrush(bands[0]));
        for (var i = 0; i < _model.QualitativeRanges.Count; i++)
        {
            var end = Math.Clamp(_model.QualitativeRanges[i], previous, _model.Maximum);
            var left = plot.Left + previous / _model.Maximum * plot.Width;
            var width = (end - previous) / _model.Maximum * plot.Width;
            if (width > 0) canvas.DrawRoundRect(new ChartRect(left, plot.Top, width, plot.Height), 5, new ChartBrush(bands[Math.Min(i, bands.Length - 1)]));
            previous = end;
        }
        var actualWidth = Math.Clamp(_model.Value, 0, _model.Maximum) / _model.Maximum * plot.Width;
        if (actualWidth > 0) canvas.DrawRoundRect(new ChartRect(plot.Left, plot.Top + 10, actualWidth, 18), 6, new ChartBrush(_model.ValueColor));
        var targetX = plot.Left + Math.Clamp(_model.Target, 0, _model.Maximum) / _model.Maximum * plot.Width;
        canvas.DrawLine(new ChartPoint(targetX, plot.Top - 7), new ChartPoint(targetX, plot.Bottom + 7), new ChartPen(_model.TargetColor, 3));
        for (var i = 0; i <= 4; i++)
        {
            var x = plot.Left + plot.Width * i / 4;
            var label = (_model.Maximum * i / 4).ToString("0.#");
            var size = canvas.MeasureText(label, 10);
            canvas.DrawLine(new ChartPoint(x, plot.Bottom + 9), new ChartPoint(x, plot.Bottom + 13), new ChartPen(theme.AxisLineColor, 1));
            canvas.DrawText(label, new ChartPoint(x - size.Width / 2, plot.Bottom + 28), theme.SecondaryTextColor, 10);
        }
        var targetText = $"Target  {_model.Target:0.#}";
        var targetSize = canvas.MeasureText(targetText, 11, true);
        canvas.DrawText(targetText, new ChartPoint(Math.Clamp(targetX - targetSize.Width / 2, plot.Left, Math.Max(plot.Left, plot.Right - targetSize.Width)), plot.Bottom + 54), _model.TargetColor, 11, true);
    }
}
