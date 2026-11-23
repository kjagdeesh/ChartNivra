namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Statistical;
using ChartNivra.Rendering.Abstractions;

public class HeatmapRenderer : IChartRenderer
{
    private readonly HeatmapModel _model;

    public HeatmapRenderer(HeatmapModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Dark;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        if (!string.IsNullOrEmpty(_model.Title))
        {
            canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
            topMargin = 50.0;
        }

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 80, bounds.Height - topMargin - 40);

        if (_model.Rows <= 0 || _model.Columns <= 0) return;

        var cellWidth = plotArea.Width / _model.Columns;
        var cellHeight = plotArea.Height / _model.Rows;
        var valueRange = _model.MaxValue - _model.MinValue;
        if (valueRange < 1e-9) valueRange = 1;

        foreach (var cell in _model.Cells)
        {
            var x = plotArea.Left + cell.Column * cellWidth;
            var y = plotArea.Top + cell.Row * cellHeight;

            var t = (cell.Value - _model.MinValue) / valueRange;
            t = Math.Clamp(t, 0, 1);
            var color = LerpColor(_model.MinColor, _model.MaxColor, (float)t);

            canvas.DrawRectangle(new ChartRect(x, y, cellWidth - 1, cellHeight - 1), new ChartBrush(color));
        }
    }

    private static ChartColor LerpColor(ChartColor a, ChartColor b, float t)
    {
        return ChartColor.FromArgb(
            (byte)Math.Round(a.A + (b.A - a.A) * t),
            (byte)Math.Round(a.R + (b.R - a.R) * t),
            (byte)Math.Round(a.G + (b.G - a.G) * t),
            (byte)Math.Round(a.B + (b.B - a.B) * t));
    }
}