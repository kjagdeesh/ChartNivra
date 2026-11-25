namespace ChartNivra.Demo;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Maui.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

public class MainPage : ContentPage
{
    public MainPage()
    {
        Title = "PulseDashboard";
        // Soft gradient slate background from reference designs
        BackgroundColor = Color.FromArgb("#F1F5F9");

        // Top Header
        var header = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            },
            Margin = new Thickness(0, 0, 0, 15)
        };

        var welcomeStack = new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                new Label
                {
                    Text = "Good morning, Alex! 👋",
                    TextColor = Color.FromArgb("#0F172A"),
                    FontSize = 24,
                    FontAttributes = FontAttributes.Bold
                },
                new Label
                {
                    Text = "Here's your performance and activity overview for today.",
                    TextColor = Color.FromArgb("#64748B"),
                    FontSize = 14
                }
            }
        };

        var dateBadge = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(14, 8),
            Content = new Label
            {
                Text = "May 20, 2026 📅",
                TextColor = Color.FromArgb("#0F172A"),
                FontSize = 13,
                FontAttributes = FontAttributes.Bold
            }
        };

        header.Add(welcomeStack, 0, 0);
        header.Add(dateBadge, 1, 0);

        // 4 KPI Metric Cards
        var kpiGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 16,
            Margin = new Thickness(0, 0, 0, 20)
        };

        kpiGrid.Add(CreateKpiCard("❤️ Heart Rate", "72", "bpm", "Normal", "#00C096"), 0, 0);
        kpiGrid.Add(CreateKpiCard("🌙 Sleep Quality", "85", "/100", "Good", "#6366F1"), 1, 0);
        kpiGrid.Add(CreateKpiCard("👟 Daily Steps", "8,432", "steps", "84% of goal", "#3B82F6"), 2, 0);
        kpiGrid.Add(CreateKpiCard("💧 Water Intake", "6", "glasses", "75% of goal", "#06B6D4"), 3, 0);

        // Weekly Activity Multi-Line Chart (Card)
        var weeklyChart = new ChartView
        {
            Title = "Weekly Progress",
            Theme = ChartTheme.CleanLight,
            HeightRequest = 320,
            HorizontalOptions = LayoutOptions.Fill
        };

        // Series 1: Steps (Teal)
        var stepsSeries = new LineSeriesModel
        {
            Title = "Steps",
            Color = ChartColor.FromHex("#00C096"),
            StrokeWidth = 3.5f,
            ShowMarkers = true,
            MarkerRadius = 4.5f,
            Points = new List<ChartPoint>
            {
                new(1, 45), new(2, 52), new(3, 49), new(4, 78),
                new(5, 68), new(6, 88), new(7, 95)
            }
        };

        // Series 2: Active Minutes (Blue)
        var activeSeries = new LineSeriesModel
        {
            Title = "Active Minutes",
            Color = ChartColor.FromHex("#3B82F6"),
            StrokeWidth = 3f,
            ShowMarkers = true,
            MarkerRadius = 4f,
            Points = new List<ChartPoint>
            {
                new(1, 30), new(2, 40), new(3, 35), new(4, 55),
                new(5, 50), new(6, 70), new(7, 75)
            }
        };

        // Series 3: Calories (Purple)
        var calorieSeries = new LineSeriesModel
        {
            Title = "Calories Burned",
            Color = ChartColor.FromHex("#8B5CF6"),
            StrokeWidth = 2.5f,
            ShowMarkers = true,
            MarkerRadius = 3.5f,
            Points = new List<ChartPoint>
            {
                new(1, 20), new(2, 28), new(3, 22), new(4, 38),
                new(5, 32), new(6, 48), new(7, 52)
            }
        };

        weeklyChart.Model.Series.Add(stepsSeries);
        weeklyChart.Model.Series.Add(activeSeries);
        weeklyChart.Model.Series.Add(calorieSeries);

        var chartCard = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 18 },
            BackgroundColor = Colors.White,
            Padding = 20,
            Content = weeklyChart
        };

        var mainLayout = new VerticalStackLayout
        {
            Padding = 28,
            Spacing = 10,
            Children =
            {
                header,
                kpiGrid,
                chartCard
            }
        };

        Content = new ScrollView { Content = mainLayout };
    }

    private static Border CreateKpiCard(string title, string value, string unit, string subtitle, string colorHex)
    {
        return new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(18, 16),
            Content = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Label { Text = title, FontSize = 13, TextColor = Color.FromArgb("#64748B"), FontAttributes = FontAttributes.Bold },
                    new HorizontalStackLayout
                    {
                        Spacing = 4,
                        Children =
                        {
                            new Label { Text = value, FontSize = 28, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                            new Label { Text = unit, FontSize = 14, TextColor = Color.FromArgb("#94A3B8"), VerticalOptions = LayoutOptions.End, Margin = new Thickness(0, 0, 0, 4) }
                        }
                    },
                    new Label { Text = $"● {subtitle}", FontSize = 12, TextColor = Color.FromArgb(colorHex), FontAttributes = FontAttributes.Bold }
                }
            }
        };
    }
}
