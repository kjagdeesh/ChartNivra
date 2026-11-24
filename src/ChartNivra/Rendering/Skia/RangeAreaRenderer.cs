namespace ChartNivra.Rendering.Skia;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Cartesian;
using ChartNivra.Rendering.Abstractions;

public class RangeAreaRenderer : IChartRenderer
{
    private readonly RangeSeriesModel _model;

    public RangeAreaRenderer(RangeSeriesModel model)
    {
        _model = model;
    }

    public void Render(IChartCanvas canvas, ChartRect bounds)
    {
        var theme = ChartTheme.Light;
        canvas.DrawRectangle(bounds, new ChartBrush(theme.BackgroundColor));

        var topMargin = 20.0;
        canvas.DrawText("Range Area Chart", new ChartPoint(bounds.Left + 20, bounds.Top + 30), theme.TextColor, 16f, isBold: true);
        topMargin = 50.0;

        var data = _model.Points;
        if (data.Count == 0) return;

        var plotArea = new ChartRect(bounds.Left + 60, bounds.Top + topMargin, bounds.Width - 80, bounds.Height - topMargin - 40);

        double minX = double.MaxValue, maxX = double.MinValue;
        double minY = double.MaxValue, maxY = double.MinValue;

        foreach (var p in data)
        {
            if (p.X < minX) minX = p.X;
            if (p.X > maxX) maxX = p.X;
            if (p.Low < minY) minY = p.Low;
            if (p.High > maxY) maxY = p.High;
        }

        var xRange = maxX - minX;
        var yRange = maxY - minY;
        if (xRange < 1e-9) xRange = 1;
        if (yRange < 1e-9) yRange = 1;

        var upperPoints = new List<ChartPoint>();
        var lowerPoints = new List<ChartPoint>();

        for (int i = 0; i < data.Count; i++)
        {
            var p = data[i];
            var x = plotArea.Left + (p.X - minX) / xRange * plotArea.Width;
            var highY = plotArea.Bottom - (p.High - minY) / yRange * plotArea.Height;
            var lowY = plotArea.Bottom - (p.Low - minY) / yRange * plotArea.Height;

            upperPoints.Add(new ChartPoint(x, highY));
            lowerPoints.Add(new ChartPoint(x, lowY));
        }

        for (var grid = 0; grid <= 4; grid++)
        {
            var y = plotArea.Top + grid * plotArea.Height / 4;
            canvas.DrawLine(new ChartPoint(plotArea.Left, y), new ChartPoint(plotArea.Right, y), new ChartPen(theme.GridLineColor, 1));
        }

        var band = new List<ChartPoint>(upperPoints);
        var reversedLower = new List<ChartPoint>(lowerPoints);
        reversedLower.Reverse();
        band.AddRange(reversedLower);
        canvas.DrawPolygon(band, new ChartBrush(_model.Color.WithAlpha(70)));
        canvas.DrawLines(upperPoints, new ChartPen(_model.Color, 2f));
        canvas.DrawLines(lowerPoints, new ChartPen(_model.Color.WithAlpha(180), 2f));
        for (var i = 0; i < data.Count; i++)
        {
            canvas.DrawCircle(upperPoints[i], 3, new ChartBrush(ChartColor.White), new ChartPen(_model.Color, 1.5f));
            canvas.DrawCircle(lowerPoints[i], 3, new ChartBrush(ChartColor.White), new ChartPen(_model.Color, 1.5f));
            var label = $"{data[i].Low:0}–{data[i].High:0}";
            var size = canvas.MeasureText(label, 9);
            canvas.DrawText(label, new ChartPoint(upperPoints[i].X - size.Width / 2, upperPoints[i].Y - 7), theme.SecondaryTextColor, 9);
        }

        canvas.DrawRectangle(plotArea, null, new ChartPen(theme.AxisLineColor, 1f));
    }
}
