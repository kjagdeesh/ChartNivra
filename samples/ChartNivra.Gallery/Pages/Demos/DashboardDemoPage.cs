namespace ChartNivra.DashboardDemo;

using ChartNivra.Core.Axes;
using ChartNivra.Core.Charts;
using ChartNivra.Core.Circular;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Gauges;
using ChartNivra.Maui.Controls;
using ChartNivra.Themes;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

public class MainPage : ContentPage
{
    private ChartPalette _currentPalette = ChartPalette.Clean;
    private readonly ChartView _revenueChart;
    private readonly ChartView _activityChart;
    private readonly ChartView _gaugeView;

    public MainPage()
    {
        Title = "ChartNivra Executive Dashboard";
        BackgroundColor = Color.FromArgb("#F8FAFC");

        var mainLayout = new VerticalStackLayout
        {
            Padding = new Thickness(24),
            Spacing = 20
        };

        // Header with Title and Palette Switcher
        var header = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        var titleBlock = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                new Label
                {
                    Text = "Enterprise Analytics Suite",
                    FontSize = 26,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#0F172A")
                },
                new Label
                {
                    Text = "Interactive real-time KPI overview with customizable dynamic color palettes",
                    FontSize = 14,
                    TextColor = Color.FromArgb("#64748B")
                }
            }
        };
        header.Add(titleBlock, 0, 0);

        // Palette Selector Picker
        var palettePicker = new Picker
        {
            Title = "Choose Palette",
            ItemsSource = new[] { "Clean SaaS", "Coral Sunset", "Emerald Forest", "Midnight Purple", "Arctic Frost" },
            SelectedIndex = 0,
            WidthRequest = 180,
            BackgroundColor = Colors.White,
            TextColor = Color.FromArgb("#0F172A")
        };
        palettePicker.SelectedIndexChanged += OnPaletteChanged;
        header.Add(palettePicker, 1, 0);
        mainLayout.Children.Add(header);

        // Grid for Chart Cards
        var chartGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = GridLength.Auto }
            },
            ColumnSpacing = 16,
            RowSpacing = 16
        };

        // Chart 1: Revenue Line Chart
        _revenueChart = new ChartView
        {
            HeightRequest = 260,
            Title = "Monthly Recurring Revenue (MRR)",
            Theme = ChartTheme.CleanLight
        };
        chartGrid.Add(CreateCard("Revenue Growth ($k)", _revenueChart), 0, 0);

        // Chart 2: User Activity Line Chart
        _activityChart = new ChartView
        {
            HeightRequest = 260,
            Title = "Active Platform Engagements",
            Theme = ChartTheme.CleanLight
        };
        chartGrid.Add(CreateCard("Engagement Trends", _activityChart), 1, 0);

        // Chart 3: Service Health Radial Gauge
        _gaugeView = new ChartView
        {
            HeightRequest = 240,
            Title = "System SLA Target (99.8%)",
            Theme = ChartTheme.CleanLight
        };
        chartGrid.Add(CreateCard("Uptime & Health", _gaugeView), 0, 1);

        // Chart 4: Resource Allocation Summary Card
        var statsCard = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(20),
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Children =
                {
                    new Label { Text = "Operational Metrics", FontSize = 18, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                    CreateStatRow("API Throughput", "42,800 req/s", "#00C096"),
                    CreateStatRow("Average Latency", "14.2 ms", "#3B82F6"),
                    CreateStatRow("Memory Footprint", "184 MB", "#8B5CF6"),
                    CreateStatRow("Active Render Frames", "60 FPS Fixed", "#F59E0B")
                }
            }
        };
        chartGrid.Add(statsCard, 1, 1);

        mainLayout.Children.Add(chartGrid);

        Content = new ScrollView { Content = mainLayout };

        RebuildCharts();
    }

    private static Border CreateCard(string title, View content)
    {
        return new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(16),
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    new Label { Text = title, FontSize = 16, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                    content
                }
            }
        };
    }

    private static Grid CreateStatRow(string label, string val, string colorHex)
    {
        var g = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };
        g.Add(new Label { Text = label, TextColor = Color.FromArgb("#64748B"), FontSize = 14 }, 0, 0);
        g.Add(new Label { Text = val, TextColor = Color.FromArgb(colorHex), FontAttributes = FontAttributes.Bold, FontSize = 14 }, 1, 0);
        return g;
    }

    private void OnPaletteChanged(object? sender, EventArgs e)
    {
        if (sender is not Picker picker) return;
        _currentPalette = picker.SelectedIndex switch
        {
            1 => ExtendedPalettes.Coral,
            2 => ExtendedPalettes.Forest,
            3 => ExtendedPalettes.Midnight,
            4 => ExtendedPalettes.Arctic,
            _ => ChartPalette.Clean
        };
        RebuildCharts();
    }

    private void RebuildCharts()
    {
        // 1. Revenue
        _revenueChart.Model.Series.Clear();
        var revSeries = new LineSeriesModel
        {
            Title = "Revenue ($k)",
            Color = _currentPalette.GetColor(0),
            StrokeWidth = 3f,
            Points = new List<ChartPoint>
            {
                new(1, 45), new(2, 58), new(3, 72), new(4, 68), new(5, 89), new(6, 110)
            }
        };
        _revenueChart.Model.Series.Add(revSeries);

        // 2. Activity Line
        _activityChart.Model.Series.Clear();
        var lineSeries = new LineSeriesModel
        {
            Title = "Daily Active Users",
            Color = _currentPalette.GetColor(1),
            StrokeWidth = 3f,
            Points = new List<ChartPoint>
            {
                new(1, 1200), new(2, 1450), new(3, 1380), new(4, 1890), new(5, 2300), new(6, 2750)
            }
        };
        _activityChart.Model.Series.Add(lineSeries);

        // 3. Radial Gauge
        var gauge = new RadialGaugeModel
        {
            Title = "Service SLA",
            Value = 99.8,
            Minimum = 0,
            Maximum = 100,
            ValueColor = _currentPalette.GetColor(0),
            Theme = ChartTheme.CleanLight
        };
        _gaugeView.Renderer = gauge;

        _revenueChart.Invalidate();
        _activityChart.Invalidate();
        _gaugeView.Invalidate();
    }
}
