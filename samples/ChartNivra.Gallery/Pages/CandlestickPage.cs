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

// Add OHLCV candles
var base_dt = new DateTime(2024, 11, 1);
model.Candlesticks.Data.Add(new FinancialPoint(base_dt,       480, 495, 475, 490, 1_200_000));
model.Candlesticks.Data.Add(new FinancialPoint(base_dt.AddDays(1), 490, 510, 485, 505, 980_000));
// ... more candles

// Add SMA overlay
model.AddSmaOverlay(5, ChartColor.FromHex("#F59E0B"));

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
