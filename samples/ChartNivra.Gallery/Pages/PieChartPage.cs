namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Circular;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Abstractions;
using ChartNivra.Rendering.Skia;

public class PieChartPage : ChartPageBase
{
    private const string Code = """
var model = new PieChartModel
{
    Title = "Market Share",
    Theme = ChartTheme.Dark,
    HoleRadiusRatio = 0f, // 0 = Pie, 0.6 = Donut
};

model.Slices.Add(new PieSliceModel { Label = "Mobile",   Value = 42, Color = ChartColor.FromHex("#00C096") });
model.Slices.Add(new PieSliceModel { Label = "Desktop",  Value = 35, Color = ChartColor.FromHex("#6366F1") });
model.Slices.Add(new PieSliceModel { Label = "Tablet",   Value = 15, Color = ChartColor.FromHex("#F59E0B") });
model.Slices.Add(new PieSliceModel { Label = "Other",    Value = 8,  Color = ChartColor.FromHex("#EF4444") });

// Render via PieChartRenderer
var renderer = new PieChartRenderer(model);
chartView.Renderer = renderer;
""";

    public PieChartPage() : base("Pie Chart", "Classic pie chart with percentage slices", Code) { }

    protected override void ConfigureChart()
    {
        var model = new PieChartModel
        {
            Title = "Market Share",
            Theme = ChartTheme.Dark,
        };

        model.Slices.Add(new PieSliceModel { Label = "Mobile",  Value = 42, Color = ChartColor.FromHex("#00C096") });
        model.Slices.Add(new PieSliceModel { Label = "Desktop", Value = 35, Color = ChartColor.FromHex("#6366F1") });
        model.Slices.Add(new PieSliceModel { Label = "Tablet",  Value = 15, Color = ChartColor.FromHex("#F59E0B") });
        model.Slices.Add(new PieSliceModel { Label = "Other",   Value = 8,  Color = ChartColor.FromHex("#EF4444"), IsExploded = true });

        UseRenderer(new PieChartRenderer(model));
    }
}
