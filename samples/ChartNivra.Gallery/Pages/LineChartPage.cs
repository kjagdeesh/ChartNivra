namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;

public class LineChartPage : ChartPageBase
{
    private const string Code = """
var model = new CartesianChartModel
{
    Title = "Monthly Revenue",
    Theme = ChartTheme.Light,
};

model.Series.Add(new LineSeriesModel
{
    Title       = "Revenue",
    Color       = ChartColor.FromHex("#00C096"),
    StrokeWidth = 2.5f,
    ShowMarkers = true,
    ShowDataLabels = true,
    Points = new List<ChartPoint>
    {
        new(0, 42), new(1, 58), new(2, 51),
        new(3, 76), new(4, 89), new(5, 95),
        new(6, 82), new(7, 103), new(8, 118),
        new(9, 97), new(10, 134), new(11, 148),
    }
});

chartView.Model.Series.AddRange(model.Series);
""";

    public LineChartPage() : base("Line Chart", "Smooth line series with data points", Code) { }

    protected override void ConfigureChart()
    {
        ChartView.Title = "Monthly Revenue";
        ChartView.Theme = ChartTheme.Light;

        ChartView.Model.Series.Add(new LineSeriesModel
        {
            Title       = "Revenue 2024",
            Color       = ChartColor.FromHex("#00C096"),
            StrokeWidth = 2.5f,
            ShowMarkers = true,
            ShowDataLabels = true,
            Points      = new System.Collections.Generic.List<ChartPoint>
            {
                new(0,42), new(1,58), new(2,51), new(3,76),
                new(4,89), new(5,95), new(6,82), new(7,103),
                new(8,118), new(9,97), new(10,134), new(11,148),
            }
        });
    }
}
