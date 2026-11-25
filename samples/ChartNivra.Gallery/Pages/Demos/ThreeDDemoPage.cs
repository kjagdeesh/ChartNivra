namespace ChartNivra.ThreeDDemo;

using System.Numerics;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Core.ThreeD;
using ChartNivra.Maui.Controls;
using ChartNivra.Rendering.Abstractions;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

public class MainPage : ContentPage
{
    private readonly ThreeDChartRenderer _renderer = new();
    private readonly ChartView _chartView;
    private double _azimuthDegrees = 45.0;
    private double _elevationDegrees = 30.0;
    private double _distance = 25.0;

    public MainPage()
    {
        Title = "3D Visualization Studio";
        BackgroundColor = Color.FromArgb("#F8FAFC");

        var layout = new VerticalStackLayout
        {
            Padding = new Thickness(24),
            Spacing = 16
        };

        // Header
        var header = new VerticalStackLayout
        {
            Spacing = 4,
            Children =
            {
                new Label { Text = "3D Spatial Bar & Surface Engine", FontSize = 22, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                new Label { Text = "Full 3D perspective projection, Lambertian lighting, and Painter's depth sorting", FontSize = 13, TextColor = Color.FromArgb("#64748B") }
            }
        };
        layout.Children.Add(header);

        // Interactive Camera Controls
        var controlsCard = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(16),
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Children =
                {
                    new Label { Text = "Camera Orbit Controls", FontSize = 15, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                    CreateSliderRow("Orbit Azimuth (°)", 0, 360, 45, val => { _azimuthDegrees = val; UpdateCamera(); }),
                    CreateSliderRow("Camera Elevation (°)", 10, 80, 30, val => { _elevationDegrees = val; UpdateCamera(); }),
                    CreateSliderRow("Zoom Distance", 10, 50, 25, val => { _distance = val; UpdateCamera(); })
                }
            }
        };
        layout.Children.Add(controlsCard);

        // 3D Chart Canvas
        _renderer.Theme = ChartTheme.CleanLight;
        _chartView = new ChartView
        {
            HeightRequest = 420,
            Title = "3D Column Matrix",
            Theme = ChartTheme.CleanLight,
            Renderer = _renderer
        };

        var chartCard = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(16),
            Content = _chartView
        };
        layout.Children.Add(chartCard);

        Content = new ScrollView { Content = layout };

        Populate3DColumns();
        UpdateCamera();
    }

    private static Grid CreateSliderRow(string title, double min, double max, double initial, Action<double> onChanged)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(160) },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = new GridLength(60) }
            }
        };

        var label = new Label { Text = title, TextColor = Color.FromArgb("#64748B"), FontSize = 13, VerticalOptions = LayoutOptions.Center };
        var valueLabel = new Label { Text = $"{initial:F0}", TextColor = Color.FromArgb("#0F172A"), FontAttributes = FontAttributes.Bold, FontSize = 13, VerticalOptions = LayoutOptions.Center };

        var slider = new Slider
        {
            Minimum = min,
            Maximum = max,
            Value = initial,
            MinimumTrackColor = Color.FromArgb("#00C096"),
            ThumbColor = Color.FromArgb("#00C096")
        };
        slider.ValueChanged += (s, e) =>
        {
            valueLabel.Text = $"{e.NewValue:F0}";
            onChanged(e.NewValue);
        };

        grid.Add(label, 0, 0);
        grid.Add(slider, 1, 0);
        grid.Add(valueLabel, 2, 0);

        return grid;
    }

    private void Populate3DColumns()
    {
        var palette = new[]
        {
            ChartColor.FromHex("#00C096"),
            ChartColor.FromHex("#3B82F6"),
            ChartColor.FromHex("#8B5CF6"),
            ChartColor.FromHex("#F59E0B")
        };

        _renderer.Meshes.Clear();

        for (var x = -2; x <= 2; x++)
        {
            for (var z = -2; z <= 2; z++)
            {
                var height = (Math.Sin(x * 0.8) + Math.Cos(z * 0.8) + 2.0) * 2.5;
                var colorIdx = Math.Abs((x + 2) + (z + 2)) % palette.Length;
                var cuboid = Column3DMeshGenerator.CreateCuboid(
                    new Vector3(x * 2f - 0.6f, 0f, z * 2f - 0.6f),
                    new Vector3(x * 2f + 0.6f, (float)height, z * 2f + 0.6f),
                    palette[colorIdx]);
                _renderer.Meshes.AddRange(cuboid);
            }
        }
    }

    private void UpdateCamera()
    {
        var radAzimuth = _azimuthDegrees * Math.PI / 180.0;
        var radElevation = _elevationDegrees * Math.PI / 180.0;

        var x = (float)(_distance * Math.Cos(radElevation) * Math.Sin(radAzimuth));
        var y = (float)(_distance * Math.Sin(radElevation));
        var z = (float)(_distance * Math.Cos(radElevation) * Math.Cos(radAzimuth));

        _renderer.Camera.Position = new Vector3(x, y, z);
        _renderer.Camera.Target = new Vector3(0f, 2f, 0f);

        _chartView.Invalidate();
    }
}
