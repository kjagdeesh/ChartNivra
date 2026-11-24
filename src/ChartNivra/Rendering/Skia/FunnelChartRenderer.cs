namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Cartesian;
using ChartNivra.Rendering.Abstractions;

public class FunnelChartRenderer : IChartRenderer
{
    private readonly FunnelSeriesModel _model;

    public FunnelChartRenderer(FunnelSeriesModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        canvas.DrawText(_model.Title, new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
        topMargin = 50.0;

        var stages = _model.Stages;
        if (stages.Count == 0) return;

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 120, bounds.Height - topMargin - 40);
        var maxValue = stages.Max(s => s.Value);

        var centerX = plotArea.Left + plotArea.Width / 2;
        var stageHeight = plotArea.Height / stages.Count;

        for (int i = 0; i < stages.Count; i++)
        {
            var stage = stages[i];
            var width = plotArea.Width * (stage.Value / maxValue);
            var left = centerX - width / 2;
            var top = plotArea.Top + i * stageHeight;
            var height = stageHeight * (1 - _model.NeckHeightRatio);
            var color = stage.Color ?? theme.DefaultPalette.GetColor(i);

            var points = new List<ChartPoint>
            {
                new(left, top),
                new(left + width, top),
                new(centerX + width * _model.NeckWidthRatio / 2, top + height),
                new(centerX - width * _model.NeckWidthRatio / 2, top + height)
            };
            points.Add(points[0]);

            canvas.DrawPolygon(points, new ChartBrush(color), new ChartPen(theme.BackgroundColor, 2f));

            // Stage and conversion labels are centered on the filled segment for immediate scanning.
            var conversion = i == 0 ? 100 : stage.Value / stages[i - 1].Value * 100;
            var label = $"{stage.Name}  {stage.Value:N0} · {conversion:0}%";
            var labelSize = canvas.MeasureText(label, 11f);
            canvas.DrawText(label, new ChartPoint(centerX - labelSize.Width / 2, top + height / 2 + 4), theme.TextColor, 11f);
        }
    }
}
