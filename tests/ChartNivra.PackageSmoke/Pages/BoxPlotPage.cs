namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Skia;
using ChartNivra.Statistical;

public class BoxPlotPage : ChartPageBase
{
    private const string Code = """
var series = new BoxPlotSeriesModel();
series.Data.AddRange(new[]
{
    new BoxPlotPoint("Jan",  5,  12, 20, 28, 45),
    new BoxPlotPoint("Feb",  8,  15, 22, 32, 50),
    new BoxPlotPoint("Mar",  3,  10, 18, 27, 42),
    new BoxPlotPoint("Apr", 10,  18, 26, 35, 55),
    new BoxPlotPoint("May",  6,  14, 24, 30, 48),
    new BoxPlotPoint("Jun", 12,  22, 30, 40, 60),
});

chartView.Renderer = new BoxPlotRenderer(series, ChartTheme.Light);
""";

    public BoxPlotPage() : base("Box Plot", "Five-number summary box-and-whisker chart", Code) { }

    protected override void ConfigureChart()
    {
        var series = new BoxPlotSeriesModel();
        series.Data.AddRange(new[]
        {
            new BoxPlotPoint("Jan",  5, 12, 20, 28, 45),
            new BoxPlotPoint("Feb",  8, 15, 22, 32, 50),
            new BoxPlotPoint("Mar",  3, 10, 18, 27, 42),
            new BoxPlotPoint("Apr", 10, 18, 26, 35, 55),
            new BoxPlotPoint("May",  6, 14, 24, 30, 48),
            new BoxPlotPoint("Jun", 12, 22, 30, 40, 60),
        });

        UseRenderer(new BoxPlotRenderer(series, ChartTheme.Light));
    }
}
