namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using ChartNivra.Core.Themes;
using Microsoft.Maui.Controls;

public class RealTimePage : ContentPage
{
    private static readonly Color PageBg     = Color.FromArgb("#F1F5F9");
    private static readonly Color CardBg     = Colors.White;
    private static readonly Color CardBorder = Color.FromArgb("#E2E8F0");
    private static readonly Color TextDark   = Color.FromArgb("#0F172A");
    private static readonly Color TextMuted  = Color.FromArgb("#64748B");

    private readonly ChartNivra.Maui.Controls.ChartView _chart;
    private readonly LineSeriesModel _series;
    private IDispatcherTimer? _timer;
    private int _tick = 0;
    private double _lastY = 50;
    private readonly System.Random _rng = new(1);
    private readonly Label _statusLabel;
    private readonly Frame _chartCard;
    private readonly Frame _controlCard;
    private readonly Button _startButton;
    private bool _running = false;

    private const string Code = """
// Seed the series
var series = new LineSeriesModel
{
    Title       = "Live Feed",
    Color       = ChartColor.FromHex("#00C096"),
    StrokeWidth = 2f,
    ShowMarkers = false,
};
chartView.Model.Series.Add(series);

// Update on a timer
var timer = Dispatcher.CreateTimer();
timer.Interval = TimeSpan.FromMilliseconds(300);
timer.Tick += (_, _) =>
{
    // Slide window — keep last 60 points
    if (series.Points.Count >= 60)
        series.Points.RemoveAt(0);

    // Add new data point
    series.Points.Add(new ChartPoint(tick++, NextValue()));
    chartView.Invalidate();
};
timer.Start();
""";

    public RealTimePage()
    {
        BackgroundColor = PageBg;
        Shell.SetNavBarIsVisible(this, true);
        Title = "Real-Time";

        _series = new LineSeriesModel
        {
            Title       = "Live Feed",
            Color       = ChartColor.FromHex("#00C096"),
            StrokeWidth = 2f,
            ShowMarkers = false,
        };

        // Seed with initial data
        for (int i = 0; i < 40; i++)
        {
            _lastY = System.Math.Clamp(_lastY + (_rng.NextDouble() * 10 - 5), 10, 90);
            _series.Points.Add(new ChartPoint(_tick++, _lastY));
        }

        _chart = new ChartNivra.Maui.Controls.ChartView
        {
            HeightRequest   = 320,
            BackgroundColor = Colors.Transparent,
            Title           = "Live Data Stream",
            Theme           = ChartTheme.Dark,
        };
        _chart.Model.Series.Add(_series);

        _statusLabel = new Label
        {
            Text              = "▶  Running",
            FontSize          = 12,
            TextColor         = Color.FromArgb("#10B981"),
            HorizontalOptions = LayoutOptions.End,
        };

        _startButton = new Button
        {
            Text            = "⏸  Pause",
            BackgroundColor = Color.FromArgb("#6366F1"),
            TextColor       = Colors.White,
            CornerRadius    = 10,
            Padding         = new Thickness(20, 10),
            FontSize        = 14,
        };
        _startButton.Clicked += (_, _) =>
        {
            _running = !_running;
            if (_running) { _timer?.Start(); _startButton.Text = "⏸  Pause";  _statusLabel.Text = "▶  Running";  _statusLabel.TextColor = Color.FromArgb("#10B981"); }
            else          { _timer?.Stop();  _startButton.Text = "▶  Resume"; _statusLabel.Text = "⏸  Paused";   _statusLabel.TextColor = Color.FromArgb("#F59E0B"); }
        };

        var snippet = new Controls.CodeSnippetView { Code = Code, HeightRequest = 240 };

        _chartCard = new Frame
        {
            Margin          = new Thickness(16, 12),
            Padding         = new Thickness(4),
            CornerRadius    = 14,
            HasShadow       = false,
            BorderColor     = CardBorder,
            BackgroundColor = CardBg,
            Content         = _chart,
        };

        _controlCard = new Frame
        {
            Margin          = new Thickness(16, 0, 16, 10),
            Padding         = new Thickness(16, 12),
            CornerRadius    = 14,
            HasShadow       = false,
            BorderColor     = CardBorder,
            BackgroundColor = CardBg,
            Content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                },
                Children = { _statusLabel, _startButton }
            }
        };
        Grid.SetColumn(_startButton, 1);

        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                Spacing  = 0,
                Children =
                {
                    new VerticalStackLayout
                    {
                        Spacing = 4, Padding = new Thickness(20, 20, 20, 0),
                        BackgroundColor = CardBg,
                        Children =
                        {
                            new Label { Text = "Real-Time Chart", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = TextDark },
                            new Label { Text = "Live streaming data with sliding window", FontSize = 13, TextColor = TextMuted },
                            new BoxView { HeightRequest = 1, BackgroundColor = CardBorder, Margin = new Thickness(0,12,0,0) },
                        }
                    },
                    _chartCard,
                    _controlCard,
                    new Frame
                    {
                        Margin = new Thickness(16, 0, 16, 16), Padding = new Thickness(0),
                        CornerRadius = 14, HasShadow = false, BorderColor = CardBorder, BackgroundColor = CardBg,
                        Content = new VerticalStackLayout
                        {
                            Children =
                            {
                                new Grid
                                {
                                    Padding = new Thickness(16, 10),
                                    BackgroundColor = Color.FromArgb("#161B22"),
                                    Children =
                                    {
                                        new Label { Text = "C#", FontSize = 12, TextColor = Color.FromArgb("#8B949E") }
                                    }
                                },
                                new BoxView { HeightRequest = 1, BackgroundColor = Color.FromArgb("#30363D") },
                                snippet,
                            }
                        }
                    }
                }
            }
        };

        StartTimer();
        _running = true;
        SizeChanged += (_, _) => UpdateLayoutForWidth(Width);
    }

    private void StartTimer()
    {
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = System.TimeSpan.FromMilliseconds(300);
        _timer.Tick += (_, _) =>
        {
            if (_series.Points.Count >= 60)
                _series.Points.RemoveAt(0);

            _lastY = System.Math.Clamp(_lastY + (_rng.NextDouble() * 10 - 5), 10, 90);
            _series.Points.Add(new ChartPoint(_tick++, _lastY));
            _chart.Invalidate();
        };
        _timer.Start();
    }

    private void UpdateLayoutForWidth(double width)
    {
        if (width <= 0) return;
        var compact = Controls.ResponsiveLayout.IsCompact(width);
        var margin = compact ? 12 : 16;
        _chart.HeightRequest = compact ? 240 : 320;
        _chartCard.Margin = new Thickness(margin, compact ? 10 : 12);
        _controlCard.Margin = new Thickness(margin, 0, margin, 10);
        if (compact)
        {
            _controlCard.Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children = { _statusLabel, _startButton }
            };
            _startButton.HorizontalOptions = LayoutOptions.Fill;
        }
        else
        {
            var grid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Auto },
                }
            };
            grid.Add(_statusLabel, 0, 0);
            grid.Add(_startButton, 1, 0);
            _controlCard.Content = grid;
            _startButton.HorizontalOptions = LayoutOptions.End;
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _timer?.Stop();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_running) _timer?.Start();
    }
}
