namespace ChartNivra.FinancialDemo;

using ChartNivra.Core.Financial;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Financial;
using ChartNivra.Maui.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

public class MainPage : ContentPage
{
    private readonly FinancialChartModel _chartModel = new();
    private readonly ChartView _chartView;
    private readonly Label _priceLabel;
    private readonly Label _changeLabel;

    public MainPage()
    {
        Title = "Financial Studio";
        BackgroundColor = Color.FromArgb("#F8FAFC");

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(24),
            Spacing = 16
        };

        // Header with Ticker and Real-Time Quote
        var header = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        var tickerBlock = new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                new Label { Text = "NVRA / USD • NASDAQ", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                new Label { Text = "Real-Time 1D Candlestick & Technical Indicators", FontSize = 13, TextColor = Color.FromArgb("#64748B") }
            }
        };
        header.Add(tickerBlock, 0, 0);

        var quoteStack = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.End,
            Children =
            {
                (_priceLabel = new Label { Text = "$184.50", FontSize = 24, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#10B981") }),
                (_changeLabel = new Label { Text = "+$6.25 (+3.51%) ▲", FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#10B981") })
            }
        };
        header.Add(quoteStack, 1, 0);
        layout.Children.Add(header);

        // Indicator Toggle Bar
        var toggleBar = new HorizontalStackLayout
        {
            Spacing = 10,
            Children =
            {
                CreateFilterButton("SMA 20", true),
                CreateFilterButton("EMA 50", true),
                CreateFilterButton("Bollinger", false),
                CreateFilterButton("RSI 14", false),
                CreateFilterButton("Volume", true)
            }
        };
        layout.Children.Add(toggleBar);

        // Chart Card
        _chartView = new ChartView
        {
            HeightRequest = 420,
            Title = "NVRA Daily Candlestick",
            Theme = ChartTheme.CleanLight
        };

        var card = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(16),
            Content = _chartView
        };
        layout.Children.Add(card);

        PopulateSampleData();

        Content = new ScrollView { Content = layout };
    }

    private static Border CreateFilterButton(string text, bool isActive)
    {
        return new Border
        {
            Stroke = isActive ? Color.FromArgb("#0EA5E9") : Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            BackgroundColor = isActive ? Color.FromArgb("#F0F9FF") : Colors.White,
            Padding = new Thickness(14, 6),
            Content = new Label
            {
                Text = text,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold,
                TextColor = isActive ? Color.FromArgb("#0EA5E9") : Color.FromArgb("#64748B")
            }
        };
    }

    private void PopulateSampleData()
    {
        var baseDate = DateTime.UtcNow.AddDays(-30);
        var basePrice = 160.0;
        var rand = new Random(42);

        for (var i = 0; i < 30; i++)
        {
            var open = basePrice + rand.NextDouble() * 4 - 2;
            var close = open + rand.NextDouble() * 6 - 2.8;
            var high = Math.Max(open, close) + rand.NextDouble() * 2;
            var low = Math.Min(open, close) - rand.NextDouble() * 2;
            var volume = rand.Next(100_000, 500_000);

            _chartModel.Candlesticks.Data.Add(new FinancialPoint(baseDate.AddDays(i), open, high, low, close, volume));
            basePrice = close;
        }

        // Add 7-day and 20-day SMA overlays
        _chartModel.AddSmaOverlay(7, ChartColor.FromHex("#3B82F6"));
        _chartModel.AddSmaOverlay(20, ChartColor.FromHex("#F59E0B"));

        _chartView.Model.Series.Clear();
        foreach (var overlay in _chartModel.Overlays)
        {
            _chartView.Model.Series.Add(overlay);
        }

        _chartView.Invalidate();
    }
}
