namespace ChartNivra.Maui.Controls;

using ChartNivra.Core.Charts;
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Themes;
using ChartNivra.Rendering.Abstractions;
using ChartNivra.Rendering.Skia;
using Microsoft.Maui.Controls;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

/// <summary>
/// Cross-platform .NET MAUI view hosting ChartNivra rendering.
/// </summary>
public class ChartView : SKCanvasView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ChartView), string.Empty, propertyChanged: OnChartPropertyChanged);

    public static readonly BindableProperty ThemeProperty =
        BindableProperty.Create(nameof(Theme), typeof(ChartTheme), typeof(ChartView), ChartTheme.Light, propertyChanged: OnChartPropertyChanged);

    private readonly CartesianChartModel _model = new();
    private readonly CartesianChartRenderer _defaultRenderer;
    private IChartRenderer? _renderer;

    public ChartView()
    {
        _defaultRenderer = new CartesianChartRenderer(_model);
        PaintSurface += OnPaintSurface;
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public ChartTheme Theme
    {
        get => (ChartTheme)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public CartesianChartModel Model => _model;

    public IChartRenderer? Renderer
    {
        get => _renderer;
        set
        {
            _renderer = value;
            ApplyRendererTheme();
            InvalidateSurface();
        }
    }

    public void Invalidate() => InvalidateSurface();

    private static void OnChartPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ChartView chart)
        {
            chart._model.Title = chart.Title;
            chart._model.Theme = chart.Theme;
            chart.ApplyRendererTheme();
            chart.InvalidateSurface();
        }
    }

    private void ApplyRendererTheme()
    {
        switch (_renderer)
        {
            case BubbleChartRenderer renderer: renderer.Theme = Theme; break;
            case FunnelChartRenderer renderer: renderer.Theme = Theme; break;
            case RangeAreaRenderer renderer: renderer.Theme = Theme; break;
            case WaterfallChartRenderer renderer: renderer.Theme = Theme; break;
            case SunburstRenderer renderer: renderer.Theme = Theme; break;
            case TreemapRenderer renderer: renderer.Theme = Theme; break;
            case StepLineRenderer renderer: renderer.Theme = Theme; break;
            case BulletChartRenderer renderer: renderer.Theme = Theme; break;
        }
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        var skCanvas = e.Surface.Canvas;
        var info = e.Info;

        // Draw in device-independent units so text and strokes stay crisp and correctly sized on high-DPI screens.
        var scale = Width > 0 ? info.Width / (float)Width : 1f;
        skCanvas.Clear();
        skCanvas.Scale(scale);

        using var canvas = new SkiaChartCanvas(skCanvas);
        var bounds = new ChartRect(0, 0, info.Width / scale, info.Height / scale);
        var activeRenderer = _renderer ?? _defaultRenderer;
        activeRenderer.Render(canvas, bounds);
    }
}
