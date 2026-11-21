namespace ChartNivra.Demo;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Maui.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

public class MainPage : ContentPage
{
    public MainPage()
    {
        Title = "ChartNivra Showcase";
        BackgroundColor = Colors.Black;

        var chart = new ChartView
        {
            Title = "Monthly Performance & Analytics (2026)",
            Theme = ChartTheme.Dark,
            HeightRequest = 450,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };

        // Add Series 1: Revenue
        var series1 = new LineSeriesModel
        {
            Title = "Revenue",
            Color = ChartColor.FromHex("#3B82F6"),
            StrokeWidth = 3f,
            ShowMarkers = true,
            Points = new List<ChartPoint>
            {
                new(1, 20),
                new(2, 45),
                new(3, 30),
                new(4, 75),
                new(5, 60),
                new(6, 110),
                new(7, 95),
                new(8, 140)
            }
        };

        // Add Series 2: Profit
        var series2 = new LineSeriesModel
        {
            Title = "Profit",
            Color = ChartColor.FromHex("#10B981"),
            StrokeWidth = 3f,
            ShowMarkers = true,
            Points = new List<ChartPoint>
            {
                new(1, 10),
                new(2, 25),
                new(3, 18),
                new(4, 40),
                new(5, 35),
                new(6, 65),
                new(7, 50),
                new(8, 85)
            }
        };

        chart.Model.Series.Add(series1);
        chart.Model.Series.Add(series2);

        var stack = new VerticalStackLayout
        {
            Padding = 20,
            Spacing = 15,
            Children =
            {
                new Label
                {
                    Text = "ChartNivra .NET MAUI Showcase",
                    TextColor = Colors.White,
                    FontSize = 24,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Center
                },
                new Label
                {
                    Text = "High-performance 2D & 3D Charting engine built with SkiaSharp & .NET 10",
                    TextColor = Colors.Gray,
                    FontSize = 14,
                    HorizontalOptions = LayoutOptions.Center
                },
                chart
            }
        };

        Content = new ScrollView { Content = stack };
    }
}
