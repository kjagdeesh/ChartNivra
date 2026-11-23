namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Gauges;

public class GaugePage : ChartPageBase
{
    private const string Code = """
var gauge = new RadialGaugeModel
{
    Title      = "CPU Usage",
    Value      = 72,
    Minimum    = 0,
    Maximum    = 100,
    Unit       = "%",
    Theme      = ChartTheme.Dark,
    NeedleColor = ChartColor.FromHex("#EF4444"),
    TrackColor  = ChartColor.FromHex("#334155"),
    ValueColor  = ChartColor.FromHex("#00C096"),
};

chartView.Renderer = gauge;
""";

    public GaugePage() : base("Radial Gauge", "Circular progress / KPI gauge", Code) { }

    protected override void ConfigureChart()
    {
        UseRenderer(new RadialGaugeModel
        {
            Title       = "CPU Usage",
            Value       = 72,
            Minimum     = 0,
            Maximum     = 100,
            Unit        = "%",
            Theme       = ChartTheme.Dark,
            NeedleColor = ChartColor.FromHex("#EF4444"),
            TrackColor  = ChartColor.FromHex("#334155"),
            ValueColor  = ChartColor.FromHex("#00C096"),
        });
    }
}
