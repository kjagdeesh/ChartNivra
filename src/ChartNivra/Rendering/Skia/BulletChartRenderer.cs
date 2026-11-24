namespace ChartNivra.Rendering.Skia;

using ChartNivra.Cartesian;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Abstractions;

/// <summary>Renders a compact bullet chart with qualitative ranges and a target marker.</summary>
public sealed class BulletChartRenderer : IChartRenderer
{
    private readonly BulletChartModel _model;
    public BulletChartRenderer(BulletChartModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16, true);
        var plot = new ChartRect(bounds.Left + 32, bounds.Top + 82, bounds.Width - 64, 74);
        var previous = 0d;
        var bands = new[] { ChartColor.FromHex("#334155"), ChartColor.FromHex("#475569"), ChartColor.FromHex("#64748B") };
        for (var i = 0; i < _model.QualitativeRanges.Count; i++)
        {
            var end = Math.Clamp(_model.QualitativeRanges[i], previous, _model.Maximum);
            var left = plot.Left + previous / _model.Maximum * plot.Width;
            var width = (end - previous) / _model.Maximum * plot.Width;
            canvas.DrawRectangle(new ChartRect(left, plot.Top, width, plot.Height), new ChartBrush(bands[Math.Min(i, bands.Length - 1)]));
            previous = end;
        }
        var actualWidth = Math.Clamp(_model.Value, 0, _model.Maximum) / _model.Maximum * plot.Width;
        canvas.DrawRectangle(new ChartRect(plot.Left, plot.Top + 20, actualWidth, 34), new ChartBrush(_model.ValueColor));
        var targetX = plot.Left + Math.Clamp(_model.Target, 0, _model.Maximum) / _model.Maximum * plot.Width;
        canvas.DrawLine(new ChartPoint(targetX, plot.Top - 8), new ChartPoint(targetX, plot.Bottom + 8), new ChartPen(_model.TargetColor, 3));
        canvas.DrawText($"{_model.Value:0} / {_model.Maximum:0}", new ChartPoint(plot.Left, plot.Bottom + 30), theme.SecondaryTextColor, 12);
        canvas.DrawText("Target", new ChartPoint(targetX + 6, plot.Top - 12), _model.TargetColor, 11, true);
    }
}
