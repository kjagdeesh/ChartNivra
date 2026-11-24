namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Circular;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Pie;
using ChartNivra.Rendering.Skia;

public class DonutChartPage : ChartPageBase
{
    private const string Code = """
var model = new DonutChartModel("Revenue Breakdown")
{
    Theme           = ChartTheme.Light,
    HoleRadiusRatio = 0.6f,
    CenterTitle     = "Total",
    CenterSubtitle  = "$1.24M",
};

model.Slices.Add(new PieSliceModel { Label = "Product",  Value = 54, Color = ChartColor.FromHex("#00C096") });
model.Slices.Add(new PieSliceModel { Label = "Services", Value = 28, Color = ChartColor.FromHex("#6366F1") });
model.Slices.Add(new PieSliceModel { Label = "Support",  Value = 18, Color = ChartColor.FromHex("#F59E0B") });

chartView.Renderer = new PieChartRenderer(model);
""";

    public DonutChartPage() : base("Donut Chart", "Donut chart with center label badge", Code) { }

    protected override void ConfigureChart()
    {
        var model = new DonutChartModel("Revenue Breakdown")
        {
            Theme           = ChartTheme.Light,
            HoleRadiusRatio = 0.6f,
            CenterTitle     = "Total",
            CenterSubtitle  = "$1.24M",
        };

        model.Slices.Add(new PieSliceModel { Label = "Product",  Value = 54, Color = ChartColor.FromHex("#00C096") });
        model.Slices.Add(new PieSliceModel { Label = "Services", Value = 28, Color = ChartColor.FromHex("#6366F1") });
        model.Slices.Add(new PieSliceModel { Label = "Support",  Value = 18, Color = ChartColor.FromHex("#F59E0B") });

        UseRenderer(new PieChartRenderer(model));
    }
}
