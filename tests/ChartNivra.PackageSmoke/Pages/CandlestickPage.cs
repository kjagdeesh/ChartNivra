namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Financial;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Financial;
using ChartNivra.Rendering.Skia;

public class CandlestickPage : ChartPageBase
{
    private const string Code = """
var model = new FinancialChartModel
{
    Title         = "NVDA – 30 Day Chart",
    BullishColor  = ChartColor.FromHex("#10B981"),
    BearishColor  = ChartColor.FromHex("#EF4444"),
    ShowVolume    = true,
};

// Exact deterministic OHLCV data generator used by this sample
var rng = new Random(7);
var date = new DateTime(2024, 11, 1);
double price = 480;
for (var i = 0; i < 30; i++)
{
    var open = price;
    var close = open + (rng.NextDouble() * 20 - 10);
    var high = Math.Max(open, close) + rng.NextDouble() * 8;
    var low = Math.Min(open, close) - rng.NextDouble() * 8;
    var volume = 800_000 + rng.Next(0, 600_000);
    model.Candlesticks.Data.Add(new FinancialPoint(date.AddDays(i), open, high, low, close, volume));
    price = close;
}

model.AddSmaOverlay(5, ChartColor.FromHex("#F59E0B"));
model.AddSmaOverlay(10, ChartColor.FromHex("#06B6D4"));

chartView.Renderer = new FinancialChartRenderer(model);
""";

    public CandlestickPage() : base("Candlestick Chart", "OHLCV candlestick chart with SMA overlays", Code) { }

    protected override void ConfigureChart()
    {
        var model = new FinancialChartModel
        {
            Title        = "NVDA – 30 Day Chart",
            BullishColor = ChartColor.FromHex("#10B981"),
            BearishColor = ChartColor.FromHex("#EF4444"),
            ShowVolume   = true,
        };

        // Generate 30 days of synthetic OHLCV data
        var rng   = new System.Random(7);
        var date  = new System.DateTime(2024, 11, 1);
        double px = 480;
        for (int i = 0; i < 30; i++)
        {
            var open  = px;
            var close = open + (rng.NextDouble() * 20 - 10);
            var high  = System.Math.Max(open, close) + rng.NextDouble() * 8;
            var low   = System.Math.Min(open, close) - rng.NextDouble() * 8;
            var vol   = 800_000 + rng.Next(0, 600_000);
            model.Candlesticks.Data.Add(new FinancialPoint(date.AddDays(i), open, high, low, close, vol));
            px = close;
        }

        model.AddSmaOverlay(5,  ChartColor.FromHex("#F59E0B"));
        model.AddSmaOverlay(10, ChartColor.FromHex("#06B6D4"));

        UseRenderer(new FinancialChartRenderer(model));
    }
}
