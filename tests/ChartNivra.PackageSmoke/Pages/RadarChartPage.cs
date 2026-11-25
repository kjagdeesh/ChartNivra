namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Skia;
using ChartNivra.Statistical;

public class RadarChartPage : ChartPageBase
{
    private const string Code = """
var model = new RadarChartModel
{
    Title    = "Team Skills",
    MaxScale = 100,
    GridRings = 5,
    Axes     = new List<string> { "Design", "Frontend", "Backend", "DevOps", "Data" },
};

model.Series.Add(new RadarSeriesModel("Team A", ChartColor.FromHex("#00C096"))
{
    Values      = new List<double> { 85, 70, 90, 65, 75 },
    FillOpacity = 0.3,
});
model.Series.Add(new RadarSeriesModel("Team B", ChartColor.FromHex("#6366F1"))
{
    Values      = new List<double> { 60, 85, 65, 80, 90 },
    FillOpacity = 0.3,
});

chartView.Renderer = new RadarChartRenderer(model);
""";

    public RadarChartPage() : base("Radar Chart", "Spider / radar chart for multi-axis comparison", Code) { }

    protected override void ConfigureChart()
    {
        var model = new RadarChartModel
        {
            Title     = "Team Skills",
            MaxScale  = 100,
            GridRings = 5,
            Axes      = new System.Collections.Generic.List<string>
                { "Design", "Frontend", "Backend", "DevOps", "Data" },
        };

        model.Series.Add(new RadarSeriesModel("Team A", ChartColor.FromHex("#00C096"))
            { Values = new System.Collections.Generic.List<double> { 85, 70, 90, 65, 75 }, FillOpacity = 0.3 });
        model.Series.Add(new RadarSeriesModel("Team B", ChartColor.FromHex("#6366F1"))
            { Values = new System.Collections.Generic.List<double> { 60, 85, 65, 80, 90 }, FillOpacity = 0.3 });

        UseRenderer(new RadarChartRenderer(model));
    }
}
