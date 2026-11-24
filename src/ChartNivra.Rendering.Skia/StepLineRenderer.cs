namespace ChartNivra.Rendering.Skia;

using ChartNivra.Cartesian;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Abstractions;

/// <summary>Renders discrete step-before, step-center, and step-after series.</summary>
public sealed class StepLineRenderer : IChartRenderer
{
    private readonly StepLineSeriesModel _model;

    public StepLineRenderer(StepLineSeriesModel model) => _model = model;

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Dark;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));
        canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16, true);
        if (_model.Points.Count == 0) return;

        var plot = new ChartRect(bounds.Left + 48, bounds.Top + 50, bounds.Width - 68, bounds.Height - 86);
        var minX = _model.Points.Min(p => p.X); var maxX = _model.Points.Max(p => p.X);
        var minY = _model.Points.Min(p => p.Y); var maxY = _model.Points.Max(p => p.Y);
        if (Math.Abs(maxX - minX) < 0.0001) maxX = minX + 1;
        if (Math.Abs(maxY - minY) < 0.0001) maxY = minY + 1;
        ChartPoint Map(ChartPoint point) => new(plot.Left + (point.X - minX) / (maxX - minX) * plot.Width, plot.Bottom - (point.Y - minY) / (maxY - minY) * plot.Height);

        for (var i = 0; i <= 4; i++)
        {
            var y = plot.Top + i * plot.Height / 4;
            canvas.DrawLine(new ChartPoint(plot.Left, y), new ChartPoint(plot.Right, y), new ChartPen(theme.GridLineColor, 1));
        }

        var path = new List<ChartPoint> { Map(_model.Points[0]) };
        for (var i = 1; i < _model.Points.Count; i++)
        {
            var previous = Map(_model.Points[i - 1]);
            var current = Map(_model.Points[i]);
            if (_model.Mode == StepMode.Before)
            {
                path.Add(new ChartPoint(previous.X, current.Y));
            }
            else if (_model.Mode == StepMode.Center)
            {
                var middle = (previous.X + current.X) / 2;
                path.Add(new ChartPoint(middle, previous.Y));
                path.Add(new ChartPoint(middle, current.Y));
            }
            else
            {
                path.Add(new ChartPoint(current.X, previous.Y));
            }
            path.Add(current);
        }
        canvas.DrawLines(path, new ChartPen(_model.Color, _model.StrokeWidth));
        foreach (var point in _model.Points)
            canvas.DrawCircle(Map(point), 3.5f, new ChartBrush(theme.BackgroundColor), new ChartPen(_model.Color, 2));
        canvas.DrawRectangle(plot, null, new ChartPen(theme.AxisLineColor, 1));
    }
}
