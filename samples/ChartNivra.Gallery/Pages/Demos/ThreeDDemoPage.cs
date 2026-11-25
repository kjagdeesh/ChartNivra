namespace ChartNivra.ThreeDDemo;

using System.Numerics;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Core.ThreeD;
using ChartNivra.Maui.Controls;
using ChartNivra.Gallery.Controls;
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
    private readonly Grid _workspace;
    private readonly Border _chartCard;
    private readonly Border _controlsCard;
    private readonly VerticalStackLayout _layout;
    private bool? _compact;

    public MainPage()
    {
        Title = "3D Visualization Studio";
        BackgroundColor = Color.FromArgb("#F8FAFC");
        Shell.SetNavBarIsVisible(this, false);

        _layout = new VerticalStackLayout
        {
            Padding = new Thickness(24, 20, 24, 32), Spacing = 20, MaximumWidthRequest = 1440, HorizontalOptions = LayoutOptions.Center
        };
        var topBar = new Grid { ColumnDefinitions = new ColumnDefinitionCollection { new() { Width = GridLength.Star }, new() { Width = GridLength.Auto } } };
        topBar.Add(new BrandView(), 0, 0);
        var back = GalleryTheme.BackButton();
        topBar.Add(back, 1, 0);
        _layout.Add(topBar);
        _layout.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.FromArgb("#E2E8F0") });
        var header = new VerticalStackLayout
        {
            Spacing = 6,
            Children =
            {
                new Label { Text = "CHARTNIVRA  /  3D STUDIO", FontSize = 10, CharacterSpacing = 1.2, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#4338CA") },
                new Label { Text = "3D Visualization Studio", FontSize = 30, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") },
                new Label { Text = "Explore the data from every angle.", FontSize = 14, TextColor = Color.FromArgb("#64748B") }
            }
        };
        _layout.Add(header);

        _renderer.Theme = ChartTheme.CleanLight;
        _chartView = new ChartView { HeightRequest = 420, Theme = ChartTheme.CleanLight, Renderer = _renderer };

        // Interactive Camera Controls
        var azimuth = CreateSliderRow("Orbit", 0, 360, 45, "°", val => { _azimuthDegrees = val; UpdateCamera(); }, out var azimuthSlider);
        var elevation = CreateSliderRow("Elevation", 10, 80, 30, "°", val => { _elevationDegrees = val; UpdateCamera(); }, out var elevationSlider);
        var distance = CreateSliderRow("Distance", 10, 50, 25, "", val => { _distance = val; UpdateCamera(); }, out var distanceSlider);
        var reset = new Button { Text = "Reset view", FontSize = 13, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#4338CA"), BackgroundColor = Color.FromArgb("#EEF2FF"), CornerRadius = 9, HeightRequest = 42 };
        reset.Clicked += (_, _) => { azimuthSlider.Value = 45; elevationSlider.Value = 30; distanceSlider.Value = 25; };
        _controlsCard = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(24),
            Content = new VerticalStackLayout
            {
                Spacing = 20,
                Children =
                {
                    new VerticalStackLayout { Spacing = 4, Children = { new Label { Text = "CAMERA", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = Color.FromArgb("#4338CA") }, new Label { Text = "Adjust your view", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") }, new Label { Text = "Move the sliders to explore the chart.", FontSize = 13, TextColor = Color.FromArgb("#64748B") } } },
                    azimuth, elevation, distance, reset
                }
            }
        };
        _chartCard = new Border
        {
            Stroke = Color.FromArgb("#E2E8F0"),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            BackgroundColor = Colors.White,
            Padding = new Thickness(24),
            Content = new VerticalStackLayout { Spacing = 16, Children = { new Label { Text = "LIVE PREVIEW", FontSize = 10, FontAttributes = FontAttributes.Bold, CharacterSpacing = 1, TextColor = Color.FromArgb("#4338CA") }, new Label { Text = "3D column matrix", FontSize = 20, FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#0F172A") }, _chartView, new Label { Text = "25 columns · height represents the generated value", FontSize = 12, TextColor = Color.FromArgb("#64748B") } } }
        };
        _workspace = new Grid { ColumnSpacing = 20, RowSpacing = 16 };
        _workspace.Add(_chartCard);
        _workspace.Add(_controlsCard);
        _layout.Add(_workspace);
        Content = new ScrollView { Content = _layout };
        this.SetAppThemeColor(BackgroundColorProperty, Color.FromArgb("#F8FAFC"), Color.FromArgb("#0B0B0C"));
        GalleryTheme.Apply(_layout);
        SizeChanged += (_, _) => UpdateLayout(Width);

        Populate3DColumns();
        UpdateCamera();
    }

    private void UpdateLayout(double width)
    {
        if (width <= 0) return;
        var compact = width < 800;
        _layout.Padding = compact ? new Thickness(16, 16, 16, 24) : new Thickness(24, 20, 24, 32);
        _layout.WidthRequest = width > 1440 ? 1400 : -1;
        if (_compact == compact) return;
        _compact = compact;
        _chartView.HeightRequest = compact ? 280 : 440;
        _workspace.ColumnDefinitions.Clear(); _workspace.RowDefinitions.Clear();
        if (compact)
        {
            _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); _workspace.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetRow(_chartCard, 0); Grid.SetColumn(_chartCard, 0); Grid.SetRow(_controlsCard, 1); Grid.SetColumn(_controlsCard, 0);
        }
        else
        {
            _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) }); _workspace.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            Grid.SetRow(_chartCard, 0); Grid.SetColumn(_chartCard, 0); Grid.SetRow(_controlsCard, 0); Grid.SetColumn(_controlsCard, 1);
        }
    }

    private static View CreateSliderRow(string title, double min, double max, double initial, string unit, Action<double> onChanged, out Slider slider)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Auto }
            }
        };

        var label = new Label { Text = title, TextColor = Color.FromArgb("#64748B"), FontSize = 13, VerticalOptions = LayoutOptions.Center };
        var valueLabel = new Label { Text = $"{initial:F0}{unit}", TextColor = Color.FromArgb("#4338CA"), FontAttributes = FontAttributes.Bold, FontSize = 13, VerticalOptions = LayoutOptions.Center };

        slider = new Slider
        {
            Minimum = min,
            Maximum = max,
            Value = initial,
            MinimumTrackColor = Color.FromArgb("#6366F1"),
            ThumbColor = Color.FromArgb("#6366F1")
        };
        slider.ValueChanged += (s, e) =>
        {
            valueLabel.Text = $"{e.NewValue:F0}{unit}";
            onChanged(e.NewValue);
        };

        grid.Add(label, 0, 0);
        grid.Add(valueLabel, 1, 0);

        return new VerticalStackLayout { Spacing = 5, Children = { grid, slider } };
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
