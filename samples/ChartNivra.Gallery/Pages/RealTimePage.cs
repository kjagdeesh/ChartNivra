namespace ChartNivra.Gallery.Pages;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;
using Microsoft.Maui.Controls;

/// <summary>Live chart sample using the shared responsive chart and source workspace.</summary>
public class RealTimePage : ChartPageBase
{
    private LineSeriesModel _series = null!;
    private Label _status = null!;
    private Button _toggle = null!;
    private IDispatcherTimer? _timer;
    private Random _random = null!;
    private int _tick;
    private double _lastValue;
    private bool _running;

    private const string Code = """
var random = new Random(1);
var tick = 0;
var value = 50d;
var series = new LineSeriesModel
{
    Title = "Live Feed",
    Color = ChartColor.FromHex("#00C096"),
    StrokeWidth = 2f,
    ShowMarkers = false
};
for (var i = 0; i < 40; i++)
{
    value = Math.Clamp(value + random.NextDouble() * 10 - 5, 10, 90);
    series.Points.Add(new ChartPoint(tick++, value));
}
chartView.Title = "Live Data Stream";
chartView.Model.Series.Add(series);

var timer = Dispatcher.CreateTimer();
timer.Interval = TimeSpan.FromMilliseconds(300);
timer.Tick += (_, _) =>
{
    if (series.Points.Count >= 60) series.Points.RemoveAt(0);
    value = Math.Clamp(value + random.NextDouble() * 10 - 5, 10, 90);
    series.Points.Add(new ChartPoint(tick++, value));
    chartView.Invalidate();
};
timer.Start();
// Pause or resume with timer.Stop() and timer.Start().
""";

    public RealTimePage() : base("Real-Time Chart", "Live streaming data with a sliding window", Code) { }

    protected override void ConfigureChart()
    {
        _random = new Random(1);
        _lastValue = 50;
        _series = new LineSeriesModel { Title = "Live Feed", Color = ChartColor.FromHex("#00C096"), StrokeWidth = 2f, ShowMarkers = false };
        for (var i = 0; i < 40; i++) AddPoint();
        ChartView.Title = "Live Data Stream";
        ChartView.Model.Series.Add(_series);

        _status = new Label { Text = "●  Streaming", FontSize = 13, TextColor = Color.FromArgb("#059669"), VerticalOptions = LayoutOptions.Center };
        _toggle = new Button { Text = "Pause stream", FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Colors.White, BackgroundColor = Color.FromArgb("#4338CA"), CornerRadius = 9, Padding = new Thickness(16, 0), HeightRequest = 42 };
        _toggle.Clicked += (_, _) =>
        {
            _running = !_running;
            if (_running) _timer?.Start(); else _timer?.Stop();
            _toggle.Text = _running ? "Pause stream" : "Resume stream";
            _status.Text = _running ? "●  Streaming" : "●  Paused";
            _status.TextColor = Color.FromArgb(_running ? "#059669" : "#B45309");
        };
        var controls = new Grid { ColumnSpacing = 12, ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Auto } } };
        controls.Add(_status, 0, 0);
        controls.Add(_toggle, 1, 0);
        AddPreviewControl(controls);
        _running = true;
    }

    private void AddPoint()
    {
        if (_series.Points.Count >= 60) _series.Points.RemoveAt(0);
        _lastValue = Math.Clamp(_lastValue + _random.NextDouble() * 10 - 5, 10, 90);
        _series.Points.Add(new ChartPoint(_tick++, _lastValue));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_timer is null)
        {
            _timer = Dispatcher.CreateTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(300);
            _timer.Tick += (_, _) => { AddPoint(); ChartView.Invalidate(); };
        }
        if (_running) _timer.Start();
    }

    protected override void OnDisappearing()
    {
        _timer?.Stop();
        base.OnDisappearing();
    }
}
