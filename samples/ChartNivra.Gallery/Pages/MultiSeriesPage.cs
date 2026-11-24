namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;

public class MultiSeriesPage : ChartPageBase
{
    private const string Code = """
chartView.Title = "Multi-Series Comparison";
chartView.Theme = ChartTheme.Dark;

var revenue  = new[] { 42, 58, 51, 76, 89, 95, 82, 103, 118, 97, 134, 148 };
var expenses = new[] { 30, 42, 38, 55, 60, 65, 58,  72,  80, 68,  90, 100 };
var revenuePoints = new List<ChartPoint>();
var expensePoints = new List<ChartPoint>();
var profitPoints  = new List<ChartPoint>();

for (var i = 0; i < revenue.Length; i++)
{
    revenuePoints.Add(new ChartPoint(i, revenue[i]));
    expensePoints.Add(new ChartPoint(i, expenses[i]));
    profitPoints.Add(new ChartPoint(i, revenue[i] - expenses[i]));
}

chartView.Model.Series.Add(new LineSeriesModel { Title = "Revenue", Color = ChartColor.FromHex("#00C096"), StrokeWidth = 2.5f, ShowMarkers = true, Points = revenuePoints });
chartView.Model.Series.Add(new LineSeriesModel { Title = "Expenses", Color = ChartColor.FromHex("#EF4444"), StrokeWidth = 2f, ShowMarkers = true, Points = expensePoints });
chartView.Model.Series.Add(new LineSeriesModel { Title = "Profit", Color = ChartColor.FromHex("#6366F1"), StrokeWidth = 2f, Points = profitPoints });
""";

    public MultiSeriesPage() : base("Multi-Series", "Multiple line series on a single chart", Code) { }

    protected override void ConfigureChart()
    {
        ChartView.Title = "Multi-Series Comparison";
        ChartView.Theme = ChartTheme.Dark;

        var months = 12;
        var rev    = new[] { 42, 58, 51, 76, 89, 95, 82, 103, 118, 97, 134, 148 };
        var exp    = new[] { 30, 42, 38, 55, 60, 65, 58, 72,  80,  68,  90, 100 };

        var revPts = new System.Collections.Generic.List<ChartPoint>();
        var expPts = new System.Collections.Generic.List<ChartPoint>();
        var proPts = new System.Collections.Generic.List<ChartPoint>();

        for (int i = 0; i < months; i++)
        {
            revPts.Add(new ChartPoint(i, rev[i]));
            expPts.Add(new ChartPoint(i, exp[i]));
            proPts.Add(new ChartPoint(i, rev[i] - exp[i]));
        }

        ChartView.Model.Series.Add(new LineSeriesModel
        {
            Title = "Revenue", Color = ChartColor.FromHex("#00C096"),
            StrokeWidth = 2.5f, ShowMarkers = true, Points = revPts,
        });
        ChartView.Model.Series.Add(new LineSeriesModel
        {
            Title = "Expenses", Color = ChartColor.FromHex("#EF4444"),
            StrokeWidth = 2f, ShowMarkers = true, Points = expPts,
        });
        ChartView.Model.Series.Add(new LineSeriesModel
        {
            Title = "Profit", Color = ChartColor.FromHex("#6366F1"),
            StrokeWidth = 2f, Points = proPts,
        });
    }
}
