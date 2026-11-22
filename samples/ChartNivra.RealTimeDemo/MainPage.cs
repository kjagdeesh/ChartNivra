namespace ChartNivra.RealTimeDemo;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using ChartNivra.Data;
using ChartNivra.Maui.Controls;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

public class MainPage : ContentPage
{
    private readonly StreamingDataPipeline _pipeline = new(500);
    private readonly CartesianChartModel _chartModel = new();
    private readonly LineSeriesModel _lineSeries;
    private readonly ChartView _chartView;
    private readonly Label _fpsLabel;
    private readonly Label _pointsLabel;
    private readonly Button _toggleButton;
    private IDispatcherTimer? _timer;
    private bool _isRunning;
    private double _time;
    private long _totalPointsProduced;

    public MainPage()
    {
        Title = "High-Frequency Real-Time Streaming";
        BackgroundColor = Color.FromArgb("#F8FAFC");

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(24),
            Spacing = 16
        };

        // Header
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
            Spacing = 2,
            Children =
            {
                new Label { Text = "Real-Time Telemetry Stream", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                new Label { Text = "60 FPS rendering pipeline with zero-allocation ring buffer", FontSize = 13, TextColor = Color.FromArgb("#64748B") }
            }
        };
        header.Add(titleBlock, 0, 0);

        _toggleButton = new Button
        {
            Text = "Pause Stream",
            BackgroundColor = Color.FromArgb("#EF4444"),
            TextColor = Colors.White,
            FontAttributes = FontAttributes.Bold,
            CornerRadius = 12,
            HeightRequest = 42,
            Padding = new Thickness(16, 0)
        };
        _toggleButton.Clicked += OnToggleClicked;
        header.Add(_toggleButton, 1, 0);
        layout.Children.Add(header);

        // Telemetry Counters
        var statsRow = new HorizontalStackLayout
        {
            Spacing = 16,
            Children =
            {
                CreateMetricPill("Render Frame Rate", (_fpsLabel = new Label { Text = "60 FPS", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#00C096") })),
                CreateMetricPill("Ring Buffer Window", new Label { Text = "500 pts", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#3B82F6") }),
                CreateMetricPill("Total Ingested", (_pointsLabel = new Label { Text = "0 pts", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#8B5CF6") }))
            }
        };
        layout.Children.Add(statsRow);

        // Chart View
        _lineSeries = new LineSeriesModel
        {
            Title = "Sensor Voltage (mV)",
            Color = ChartColor.FromHex("#00C096"),
            StrokeWidth = 2.5f,
            ShowMarkers = false
        };

        _chartView = new ChartView
        {
            HeightRequest = 420,
            Title = "Real-Time Voltage Signal",
            Theme = ChartTheme.CleanLight
        };
        _chartView.Model.Series.Add(_lineSeries);

        var card = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(16),
            Content = _chartView
        };
        layout.Children.Add(card);

        Content = new ScrollView { Content = layout };

        // Pipeline callback
        _pipeline.DataFlushed += points =>
        {
            _lineSeries.Points = points.ToList();
            _chartView.Invalidate();
        };

        StartStreaming();
    }

    private static Border CreateMetricPill(string label, Label valLabel)
    {
        return new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(16, 10),
            Content = new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    new Label { Text = label, FontSize = 12, TextColor = Color.FromArgb("#64748B") },
                    valLabel
                }
            }
        };
    }

    private void StartStreaming()
    {
        _isRunning = true;
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(16); // ~60 fps
        _timer.Tick += (s, e) =>
        {
            if (!_isRunning) return;

            // Generate 3 sample data points per tick
            for (var i = 0; i < 3; i++)
            {
                _time += 0.05;
                var signal = Math.Sin(_time) * 40.0 + Math.Cos(_time * 2.5) * 15.0 + (Random.Shared.NextDouble() - 0.5) * 8.0;
                _pipeline.Push(_time, signal);
                _totalPointsProduced++;
            }

            _pipeline.Flush();
            _pointsLabel.Text = $"{_totalPointsProduced:N0} pts";
        };
        _timer.Start();
    }

    private void OnToggleClicked(object? sender, EventArgs e)
    {
        _isRunning = !_isRunning;
        if (_isRunning)
        {
            _toggleButton.Text = "Pause Stream";
            _toggleButton.BackgroundColor = Color.FromArgb("#EF4444");
        }
        else
        {
            _toggleButton.Text = "Resume Stream";
            _toggleButton.BackgroundColor = Color.FromArgb("#10B981");
        }
    }
}
