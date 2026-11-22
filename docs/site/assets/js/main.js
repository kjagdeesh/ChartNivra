/* ═══════════════════════════════════════════════════════════
   ChartNivra Documentation — Interactive JS
   Clean, minimal, professional
═══════════════════════════════════════════════════════════ */

'use strict';

/* ── NAV scroll shadow ──────────────────────────────────── */
window.addEventListener('scroll', () => {
  document.getElementById('nav').classList.toggle('scrolled', window.scrollY > 10);
}, { passive: true });

/* ── Copy install command ───────────────────────────────── */
function copyInstall(btn) {
  const cmd = document.querySelector('.install-cmd').textContent;
  navigator.clipboard.writeText(cmd).then(() => {
    btn.innerHTML = `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="#22C55E" stroke-width="2.5"><polyline points="20 6 9 17 4 12"/></svg>`;
    setTimeout(() => {
      btn.innerHTML = `<svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="9" y="9" width="13" height="13" rx="2" ry="2"/><path d="M5 15H4a2 2 0 01-2-2V4a2 2 0 012-2h9a2 2 0 012 2v1"/></svg>`;
    }, 2000);
  });
}

/* ── Copy code snippet ──────────────────────────────────── */
function copySnippet(btn) {
  const pre = btn.closest('.code-block').querySelector('pre code');
  navigator.clipboard.writeText(pre.textContent).then(() => {
    const orig = btn.textContent;
    btn.textContent = 'Copied!';
    btn.style.color = '#22C55E';
    setTimeout(() => { btn.textContent = orig; btn.style.color = ''; }, 2000);
  });
}

/* ── Chart category tabs ────────────────────────────────── */
document.querySelectorAll('.tab-btn').forEach(btn => {
  btn.addEventListener('click', () => {
    document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
    btn.classList.add('active');

    const cat = btn.dataset.category;
    document.querySelectorAll('.chart-card').forEach(card => {
      if (cat === 'all' || card.dataset.category === cat) {
        card.classList.remove('hidden');
      } else {
        card.classList.add('hidden');
      }
    });
  });
});

/* ── Palette switcher for phone showcase ────────────────── */
const paletteColors = {
  blue:    { primary: '#0EA5E9', light: '#38BDF8', lighter: '#7DD3FC' },
  violet:  { primary: '#8B5CF6', light: '#A78BFA', lighter: '#C4B5FD' },
  emerald: { primary: '#059669', light: '#34D399', lighter: '#6EE7B7' },
  rose:    { primary: '#E11D48', light: '#FB7185', lighter: '#FDA4AF' },
  amber:   { primary: '#D97706', light: '#FBBF24', lighter: '#FDE68A' },
};

function switchPalette(palette, btn) {
  document.querySelectorAll('.palette-btn').forEach(b => b.classList.remove('active'));
  btn.classList.add('active');

  const c = paletteColors[palette];
  if (!c) return;

  // Update all chart SVG strokes in the phone mockups
  document.querySelectorAll('.phone-mockup .pm-screen svg [stroke]').forEach(el => {
    const s = el.getAttribute('stroke');
    if (s && s.startsWith('#') && s !== '#F1F5F9' && s !== '#E2E8F0' && s !== '#CBD5E1' && s !== '#94A3B8') {
      if (['#0EA5E9','#38BDF8','#7DD3FC','#8B5CF6','#A78BFA','#C4B5FD','#059669','#34D399','#E11D48','#FB7185','#D97706','#FBBF24'].includes(s)) {
        el.setAttribute('stroke', c.primary);
      }
    }
  });

  // Update fills
  document.querySelectorAll('.phone-mockup .pm-screen svg [fill]').forEach(el => {
    const f = el.getAttribute('fill');
    if (f && f.startsWith('#')) {
      if (['#0EA5E9','#38BDF8'].includes(f)) el.setAttribute('fill', c.primary);
      else if (['#7DD3FC','#BAE6FD'].includes(f)) el.setAttribute('fill', c.lighter);
    }
  });

  // Update circle chart in donut phone
  document.querySelectorAll('.phone-mockup circle[stroke]').forEach(el => {
    const s = el.getAttribute('stroke');
    if (s && ['#0EA5E9','#38BDF8'].includes(s)) el.setAttribute('stroke', c.primary);
  });

  // Update chart preview cards too
  document.querySelectorAll('.chart-preview svg path[stroke="#0EA5E9"]').forEach(el => {
    el.setAttribute('stroke', c.primary);
  });
  document.querySelectorAll('.chart-preview svg rect[fill="#0EA5E9"]').forEach(el => {
    el.setAttribute('fill', c.primary);
  });

  // Update CSS custom property for accent color
  document.documentElement.style.setProperty('--primary', c.primary);
}

/* ── Chart data: code snippets ──────────────────────────── */
const chartData = {
  line: {
    title: 'Line Chart',
    desc: 'Smooth spline with area fill, multi-series, real-time capable',
    tags: ['Spline', 'Area', 'Multi-Series', 'Bezier'],
    xaml: `<!-- Line Chart with area fill and multi-series -->
<cn:LineChartView
    Title="Monthly Revenue"
    Series="{Binding LineSeries}"
    Theme="{Binding AppTheme}"
    Palette="{Binding ColorPalette}"
    ShowGrid="True"
    ShowLegend="True"
    Smooth="True"
    ShowArea="True"
    AnimationDuration="800"
    XAxisLabel="Month"
    YAxisLabel="Revenue ($)"
    HeightRequest="300" />`,
    csharp: `// Create a smooth spline line chart with area fill
var chart = new LineChartView
{
    Title = "Monthly Revenue",
    ShowGrid = true,
    ShowLegend = true,
    Smooth = true,
    ShowArea = true,
    AnimationDuration = 800,
    Theme = ChartTheme.CleanLight,
    Palette = ChartPalette.Ocean,
};

var series = new LineSeriesModel { Label = "Revenue", Smooth = true, ShowArea = true };
series.Points.AddRange(new[]
{
    new ChartPoint { X = 0, Y = 12400, Label = "Jan" },
    new ChartPoint { X = 1, Y = 15200, Label = "Feb" },
    new ChartPoint { X = 2, Y = 11800, Label = "Mar" },
    new ChartPoint { X = 3, Y = 18600, Label = "Apr" },
    new ChartPoint { X = 4, Y = 21000, Label = "May" },
    new ChartPoint { X = 5, Y = 19400, Label = "Jun" },
});
chart.Series.Add(series);`,
    mvvm: `// ViewModel — full MVVM binding pattern
public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<LineSeriesModel> lineSeries = new();

    [ObservableProperty]
    private ChartTheme appTheme = ChartTheme.CleanLight;

    [ObservableProperty]
    private ChartPalette colorPalette = ChartPalette.Ocean;

    public DashboardViewModel()
    {
        var s1 = new LineSeriesModel { Label = "Revenue", Smooth = true, ShowArea = true };
        var s2 = new LineSeriesModel { Label = "Target",  Smooth = true, ShowArea = false };
        for (int i = 0; i < 7; i++)
        {
            s1.Points.Add(new ChartPoint { X = i, Y = Random.Shared.NextDouble() * 20000 });
            s2.Points.Add(new ChartPoint { X = i, Y = 15000 });
        }
        LineSeries.Add(s1);
        LineSeries.Add(s2);
    }
}`,
    props: [
      { name: 'Smooth', type: 'bool', desc: 'Use cubic bezier spline instead of straight segments' },
      { name: 'ShowArea', type: 'bool', desc: 'Enable gradient area fill under the line' },
      { name: 'StrokeWidth', type: 'double', desc: 'Line stroke thickness in pixels (default: 2.5)' },
      { name: 'AnimationDuration', type: 'int (ms)', desc: 'Draw animation duration in milliseconds' },
      { name: 'ShowPoints', type: 'bool', desc: 'Render data point circles on the line' },
      { name: 'PointRadius', type: 'double', desc: 'Radius of data point circles' },
    ],
  },

  bar: {
    title: 'Bar / Column Chart',
    desc: 'Vertical & horizontal bars, grouped, stacked, 100% stacked',
    tags: ['Grouped', 'Stacked', '100%', 'Horizontal'],
    xaml: `<!-- Grouped bar chart with rounded corners -->
<cn:BarChartView
    Title="Quarterly Sales"
    Series="{Binding BarSeries}"
    Orientation="Vertical"
    GroupMode="Grouped"
    ShowValues="True"
    CornerRadius="4"
    BarSpacing="0.3"
    ShowGrid="True"
    ShowLegend="True"
    AnimationDuration="600"
    HeightRequest="300" />`,
    csharp: `// Grouped bar chart
var chart = new BarChartView
{
    Title = "Quarterly Sales",
    Orientation = BarOrientation.Vertical,
    GroupMode = BarGroupMode.Grouped,
    ShowValues = true,
    CornerRadius = 4,
    BarSpacing = 0.3,
};

var q1 = new BarSeriesModel { Label = "Q1 2025" };
var q2 = new BarSeriesModel { Label = "Q2 2025" };
string[] months = { "Jan", "Feb", "Mar" };
for (int i = 0; i < 3; i++)
{
    q1.Points.Add(new ChartPoint { X = i, Y = 45000 + i * 3000, Label = months[i] });
    q2.Points.Add(new ChartPoint { X = i, Y = 52000 + i * 2500, Label = months[i] });
}
chart.Series.Add(q1);
chart.Series.Add(q2);`,
    mvvm: `public partial class SalesViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<BarSeriesModel> barSeries = new();

    [ObservableProperty]
    private BarGroupMode groupMode = BarGroupMode.Grouped;

    // Bind GroupMode to a Picker for live Grouped/Stacked/100% switching
    [RelayCommand]
    private void ToggleGroupMode()
    {
        GroupMode = GroupMode == BarGroupMode.Grouped
            ? BarGroupMode.Stacked
            : BarGroupMode.Grouped;
    }
}`,
    props: [
      { name: 'Orientation', type: 'BarOrientation', desc: 'Vertical (column) or Horizontal bars' },
      { name: 'GroupMode', type: 'BarGroupMode', desc: 'Grouped, Stacked, or Stacked100 rendering' },
      { name: 'CornerRadius', type: 'double', desc: 'Rounded corner radius on bar tops' },
      { name: 'BarSpacing', type: 'double (0–1)', desc: 'Gap between bars as fraction of bar width' },
      { name: 'ShowValues', type: 'bool', desc: 'Render value labels above each bar' },
    ],
  },

  area: {
    title: 'Area Chart',
    desc: 'Stacked and overlapping area charts with gradient fills',
    tags: ['Stacked', 'Gradient', 'Multi-Layer', 'Overlap'],
    xaml: `<!-- Stacked area chart -->
<cn:AreaChartView
    Title="Traffic Sources"
    Series="{Binding AreaSeries}"
    Stacked="True"
    ShowArea="True"
    FillOpacity="0.75"
    ShowGrid="True"
    ShowLegend="True"
    AnimationDuration="700"
    HeightRequest="300" />`,
    csharp: `// Stacked area chart
var chart = new AreaChartView
{
    Title = "Traffic Sources",
    Stacked = true,
    ShowArea = true,
    FillOpacity = 0.75,
    ShowGrid = true,
    ShowLegend = true,
};

chart.Series.Add(new AreaSeriesModel
{
    Label = "Organic",
    Points = { new(0,4200), new(1,5100), new(2,4800), new(3,6200), new(4,7100) }
});
chart.Series.Add(new AreaSeriesModel
{
    Label = "Paid",
    Points = { new(0,2100), new(1,2400), new(2,2200), new(3,3100), new(4,2800) }
});
chart.Series.Add(new AreaSeriesModel
{
    Label = "Direct",
    Points = { new(0,1200), new(1,1400), new(2,1100), new(3,1500), new(4,1300) }
});`,
    mvvm: `public partial class TrafficViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<AreaSeriesModel> areaSeries = new();
    [ObservableProperty] private bool stacked = true;
    [ObservableProperty] private double fillOpacity = 0.75;
}`,
    props: [
      { name: 'Stacked', type: 'bool', desc: 'Stack series vertically' },
      { name: 'FillOpacity', type: 'double', desc: 'Area fill opacity (0 = transparent, 1 = solid)' },
      { name: 'GradientFill', type: 'bool', desc: 'Use vertical gradient instead of solid fill' },
      { name: 'Smooth', type: 'bool', desc: 'Use bezier spline for smooth curves' },
    ],
  },

  scatter: {
    title: 'Scatter / Bubble Chart',
    desc: 'XY scatter with variable-size bubbles and color encoding',
    tags: ['XY Plot', 'Bubble', 'Clustering', 'Size Encoding'],
    xaml: `<!-- Bubble chart with size and color encoding -->
<cn:ScatterChartView
    Title="Market Share Analysis"
    Series="{Binding ScatterSeries}"
    Mode="Bubble"
    ShowTooltip="True"
    ShowGrid="True"
    AnimationDuration="500"
    HeightRequest="320" />`,
    csharp: `// Bubble chart
var chart = new ScatterChartView
{
    Title = "Market Share",
    Mode = ScatterMode.Bubble,
    ShowTooltip = true,
};
var series = new ScatterSeriesModel { Label = "Products" };
series.Points.AddRange(new[]
{
    new BubblePoint { X = 25, Y = 85, Size = 40, Label = "Product A" },
    new BubblePoint { X = 60, Y = 55, Size = 65, Label = "Product B" },
    new BubblePoint { X = 80, Y = 70, Size = 30, Label = "Product C" },
    new BubblePoint { X = 40, Y = 30, Size = 50, Label = "Product D" },
});
chart.Series.Add(series);`,
    mvvm: `public partial class MarketViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<ScatterSeriesModel> scatterSeries = new();
    [ObservableProperty] private ScatterMode mode = ScatterMode.Bubble;
}`,
    props: [
      { name: 'Mode', type: 'ScatterMode', desc: 'Point (uniform size) or Bubble (variable size)' },
      { name: 'PointRadius', type: 'double', desc: 'Default radius for Point mode' },
      { name: 'MaxBubbleSize', type: 'double', desc: 'Max radius in Bubble mode' },
      { name: 'ShowTooltip', type: 'bool', desc: 'Show data tooltip on tap/hover' },
    ],
  },

  candlestick: {
    title: 'Candlestick / OHLC',
    desc: 'Financial OHLC, candlestick with volume bars and technical overlays',
    tags: ['OHLC', 'Volume', 'SMA', 'EMA', 'Financial'],
    xaml: `<!-- Candlestick chart with volume and SMA overlay -->
<cn:CandlestickChartView
    Title="AAPL - 1D"
    Series="{Binding CandleSeries}"
    ShowVolume="True"
    VolumePaneRatio="0.25"
    Indicators="{Binding Indicators}"
    ShowCrosshair="True"
    HeightRequest="420" />`,
    csharp: `// Candlestick chart with technical indicators
var chart = new CandlestickChartView
{
    Title = "AAPL - 1D",
    ShowVolume = true,
    VolumePaneRatio = 0.25,
    ShowCrosshair = true,
};

var series = new CandlestickSeriesModel();
series.Points.Add(new FinancialPoint
{
    Timestamp = DateTime.Today.AddDays(-5),
    Open = 182.5,  High = 186.2,
    Low  = 181.0,  Close = 185.8,
    Volume = 48_200_000
});
// ... add more points

// Add SMA(20) indicator overlay
chart.Indicators.Add(new SmaIndicator { Period = 20, Color = Colors.Orange });
chart.Indicators.Add(new EmaIndicator { Period = 50, Color = Colors.Purple });
chart.Indicators.Add(new BollingerBandIndicator { Period = 20, StdDev = 2.0 });`,
    mvvm: `public partial class TradingViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<CandlestickSeriesModel> candleSeries = new();
    [ObservableProperty] private ObservableCollection<IChartIndicator> indicators = new();

    public TradingViewModel()
    {
        Indicators.Add(new SmaIndicator { Period = 20 });
        Indicators.Add(new BollingerBandIndicator { Period = 20 });
        Indicators.Add(new RsiIndicator { Period = 14 }); // bottom pane
        Indicators.Add(new MacdIndicator { Fast = 12, Slow = 26, Signal = 9 });
    }
}`,
    props: [
      { name: 'ShowVolume', type: 'bool', desc: 'Show volume histogram in separate pane' },
      { name: 'VolumePaneRatio', type: 'double', desc: 'Fraction of height for volume pane (0.0–0.5)' },
      { name: 'Indicators', type: 'IChartIndicator[]', desc: 'Technical overlays: SMA, EMA, Bollinger, RSI, MACD' },
      { name: 'ShowCrosshair', type: 'bool', desc: 'Interactive price/date crosshair' },
      { name: 'BullColor', type: 'Color', desc: 'Rising candle body color (default green)' },
      { name: 'BearColor', type: 'Color', desc: 'Falling candle body color (default red)' },
    ],
  },

  gauge: {
    title: 'Radial Gauge',
    desc: 'Semi-circle and full-circle gauges with zones, needle, and bands',
    tags: ['Needle', 'Zones', 'Speed', 'Animated'],
    xaml: `<!-- Radial gauge with colored zones and needle -->
<cn:RadialGaugeView
    Title="CPU Usage"
    Value="{Binding CpuUsage}"
    Minimum="0"
    Maximum="100"
    SweepAngle="240"
    ShowNeedle="True"
    ShowValueLabel="True"
    ValueFormat="{}{0:F0}%"
    HeightRequest="250">
  <cn:RadialGaugeView.Ranges>
    <cn:GaugeRange From="0"  To="50"  Color="#22C55E" />
    <cn:GaugeRange From="50" To="80"  Color="#F97316" />
    <cn:GaugeRange From="80" To="100" Color="#EF4444" />
  </cn:RadialGaugeView.Ranges>
</cn:RadialGaugeView>`,
    csharp: `// Radial gauge with live binding
var gauge = new RadialGaugeView
{
    Title = "CPU Usage",
    Minimum = 0,
    Maximum = 100,
    SweepAngle = 240,
    ShowNeedle = true,
    ShowValueLabel = true,
    ValueFormat = "{0:F0}%",
};
gauge.Ranges.Add(new GaugeRange { From = 0,  To = 50,  Color = Colors.Green });
gauge.Ranges.Add(new GaugeRange { From = 50, To = 80,  Color = Colors.Orange });
gauge.Ranges.Add(new GaugeRange { From = 80, To = 100, Color = Colors.Red });
gauge.SetBinding(RadialGaugeView.ValueProperty, "CpuUsage");`,
    mvvm: `public partial class SystemViewModel : ObservableObject
{
    [ObservableProperty] private double cpuUsage;

    public SystemViewModel()
    {
        // Simulate live data update
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            CpuUsage = Random.Shared.NextDouble() * 100;
            return true;
        });
    }
}`,
    props: [
      { name: 'Value', type: 'double', desc: 'Current gauge value (bindable)' },
      { name: 'Minimum / Maximum', type: 'double', desc: 'Scale range for the gauge' },
      { name: 'SweepAngle', type: 'double (°)', desc: 'Arc sweep angle (180 = semi-circle, 360 = full)' },
      { name: 'ShowNeedle', type: 'bool', desc: 'Display animated needle pointer' },
      { name: 'Ranges', type: 'GaugeRange[]', desc: 'Colored arc zones with from/to values' },
      { name: 'AnimationDuration', type: 'int (ms)', desc: 'Needle sweep animation duration' },
    ],
  },

  histogram: {
    title: 'Histogram',
    desc: 'Frequency histogram with normal distribution overlay and bin config',
    tags: ['Frequency', 'Distribution', 'Bins', 'Normal Curve'],
    xaml: `<!-- Histogram with normal distribution curve -->
<cn:HistogramView
    Title="Response Time Distribution"
    Data="{Binding ResponseTimes}"
    BinCount="20"
    ShowNormalCurve="True"
    ShowMeanLine="True"
    ShowGrid="True"
    HeightRequest="280" />`,
    csharp: `// Histogram from raw data
var histogram = new HistogramView
{
    Title = "Response Time Distribution (ms)",
    BinCount = 20,
    ShowNormalCurve = true,
    ShowMeanLine = true,
};

// Bind or set raw data values
histogram.Data = Enumerable.Range(0, 1000)
    .Select(_ => Random.Shared.NextDouble() * 300 + 50)
    .ToList();`,
    mvvm: `public partial class MetricsViewModel : ObservableObject
{
    [ObservableProperty] private IList<double> responseTimes = new List<double>();
    [ObservableProperty] private int binCount = 20;
    [ObservableProperty] private bool showNormalCurve = true;
}`,
    props: [
      { name: 'Data', type: 'IList<double>', desc: 'Raw numeric values to histogram' },
      { name: 'BinCount', type: 'int', desc: 'Number of bins (auto if 0)' },
      { name: 'BinWidth', type: 'double?', desc: 'Fixed bin width in data units (overrides BinCount)' },
      { name: 'ShowNormalCurve', type: 'bool', desc: 'Overlay a fitted normal distribution curve' },
      { name: 'ShowMeanLine', type: 'bool', desc: 'Draw a vertical mean reference line' },
    ],
  },

  boxplot: {
    title: 'Box Plot',
    desc: 'Quartile box plot with whiskers, outliers, and violin variants',
    tags: ['Quartile', 'Outliers', 'Whiskers', 'Violin'],
    xaml: `<!-- Box plot comparison -->
<cn:BoxPlotView
    Title="Score Distribution by Group"
    Series="{Binding BoxSeries}"
    ShowOutliers="True"
    ShowMean="True"
    Mode="BoxWhisker"
    HeightRequest="280" />`,
    csharp: `// Box plot series
var chart = new BoxPlotView
{
    Title = "Score Distribution",
    ShowOutliers = true,
    ShowMean = true,
    Mode = BoxPlotMode.BoxWhisker,
};
chart.Series.Add(new BoxPlotSeriesModel
{
    Label = "Group A",
    // Provide raw data — library computes Q1/Q3/median/whiskers
    RawData = new[] { 72.0, 75, 78, 80, 82, 85, 88, 90, 91, 93, 95, 98 }
});`,
    mvvm: `public partial class StatsViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<BoxPlotSeriesModel> boxSeries = new();
    [ObservableProperty] private BoxPlotMode mode = BoxPlotMode.BoxWhisker;
}`,
    props: [
      { name: 'Mode', type: 'BoxPlotMode', desc: 'BoxWhisker or Violin rendering style' },
      { name: 'ShowOutliers', type: 'bool', desc: 'Render outlier points beyond whiskers' },
      { name: 'ShowMean', type: 'bool', desc: 'Render a diamond marker at the mean' },
      { name: 'WhiskerExtent', type: 'double', desc: 'Whisker length as IQR multiple (default 1.5)' },
    ],
  },

  radar: {
    title: 'Radar / Spider Chart',
    desc: 'Multi-axis radar with configurable axes, fill opacity, and labels',
    tags: ['Spider', 'Polar', 'Multi-Axis', 'Overlay'],
    xaml: `<!-- Radar chart with two overlapping series -->
<cn:RadarChartView
    Title="Skill Comparison"
    Series="{Binding RadarSeries}"
    Axes="{Binding RadarAxes}"
    FillOpacity="0.25"
    ShowLabels="True"
    ShowGridLines="True"
    HeightRequest="300" />`,
    csharp: `// Radar/spider chart
var chart = new RadarChartView
{
    Title = "Skill Comparison",
    FillOpacity = 0.25,
    ShowLabels = true,
};
chart.Axes.AddRange(new[] { "Speed","Power","Accuracy","Endurance","Agility" });
chart.Series.Add(new RadarSeriesModel
{
    Label = "Athlete A",
    Values = new[] { 85.0, 70, 90, 75, 80 }
});
chart.Series.Add(new RadarSeriesModel
{
    Label = "Athlete B",
    Values = new[] { 70.0, 90, 75, 85, 65 }
});`,
    mvvm: `public partial class SkillViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<RadarSeriesModel> radarSeries = new();
    [ObservableProperty] private ObservableCollection<string> radarAxes = new();
}`,
    props: [
      { name: 'Axes', type: 'IList<string>', desc: 'Axis labels evenly spaced around center' },
      { name: 'FillOpacity', type: 'double', desc: 'Polygon fill opacity (0 = none, 1 = solid)' },
      { name: 'GridLevels', type: 'int', desc: 'Number of concentric web grid rings' },
      { name: 'ShowLabels', type: 'bool', desc: 'Display axis labels at outer ring' },
    ],
  },

  '3dbar': {
    title: '3D Column / Bar Chart',
    desc: 'True perspective 3D bars with Camera3D, orbit, pan, and zoom',
    tags: ['3D', 'Perspective', 'Orbit', 'Camera', 'Lighting'],
    xaml: `<!-- True 3D column chart with orbit gestures -->
<cn:ColumnChart3DView
    Title="Regional Sales 3D"
    Series="{Binding ColumnSeries3D}"
    Camera="{Binding Camera}"
    EnableOrbit="True"
    EnablePinchZoom="True"
    ShowShadow="True"
    LightAngle="45"
    HeightRequest="380" />`,
    csharp: `// True 3D column chart
var chart = new ColumnChart3DView
{
    Title = "Regional Sales 3D",
    EnableOrbit = true,
    EnablePinchZoom = true,
    ShowShadow = true,
    LightAngle = 45,
    Camera = new Camera3D
    {
        Yaw   = -30,
        Pitch =  25,
        Fov   =  60,
        Distance = 10,
    },
};

chart.Series.Add(new BarSeries3DModel
{
    Label = "North",
    Points = {
        new ChartPoint3D { X=0, Y=42000, Z=0 },
        new ChartPoint3D { X=1, Y=58000, Z=0 },
        new ChartPoint3D { X=2, Y=51000, Z=0 },
    }
});`,
    mvvm: `public partial class Sales3DViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<BarSeries3DModel> columnSeries3D = new();
    [ObservableProperty] private Camera3D camera = new() { Yaw = -30, Pitch = 25, Fov = 60 };
    [ObservableProperty] private ChartPalette colorPalette = ChartPalette.Vibrant;
}`,
    props: [
      { name: 'Camera', type: 'Camera3D', desc: 'Yaw, Pitch, Fov, Distance controls for 3D view' },
      { name: 'EnableOrbit', type: 'bool', desc: 'Pan gesture rotates the camera around the scene' },
      { name: 'EnablePinchZoom', type: 'bool', desc: 'Pinch gesture zooms in/out' },
      { name: 'LightAngle', type: 'double (°)', desc: 'Lambertian light direction for shading' },
      { name: 'ShowShadow', type: 'bool', desc: 'Cast shadows on the floor plane' },
    ],
  },

  '3dsurface': {
    title: '3D Surface / Mesh',
    desc: "Parametric 3D surface with Painter's algorithm depth sort and lighting",
    tags: ['Mesh', 'Wireframe', 'Lighting', 'Parametric', 'Smooth'],
    xaml: `<!-- 3D Surface chart -->
<cn:SurfaceChart3DView
    Title="Terrain Map"
    Data="{Binding SurfaceData}"
    Camera="{Binding Camera}"
    ColorScale="HeatMap"
    ShowWireframe="False"
    EnableOrbit="True"
    HeightRequest="400" />`,
    csharp: `// 3D Surface / mesh chart
var chart = new SurfaceChart3DView
{
    Title = "Terrain Map",
    ColorScale = ColorScale.HeatMap,
    ShowWireframe = false,
    EnableOrbit = true,
    Camera = new Camera3D { Yaw = -35, Pitch = 30, Fov = 55 },
};

// Provide a 2D grid of Z values
int n = 30;
var zGrid = new double[n, n];
for (int x = 0; x < n; x++)
for (int z = 0; z < n; z++)
{
    double u = (x - n/2.0) / 5.0;
    double v = (z - n/2.0) / 5.0;
    zGrid[x, z] = Math.Sin(Math.Sqrt(u*u + v*v));
}
chart.Data = zGrid;`,
    mvvm: `public partial class SurfaceViewModel : ObservableObject
{
    [ObservableProperty] private double[,] surfaceData;
    [ObservableProperty] private Camera3D camera = new() { Yaw = -35, Pitch = 30 };
    [ObservableProperty] private ColorScale colorScale = ColorScale.HeatMap;
    [ObservableProperty] private bool showWireframe = false;
}`,
    props: [
      { name: 'Data', type: 'double[,]', desc: 'Z-height grid (NxM 2D array)' },
      { name: 'ColorScale', type: 'ColorScale', desc: 'Color mapping: HeatMap, Rainbow, Monochrome' },
      { name: 'ShowWireframe', type: 'bool', desc: 'Render edges in wireframe mode' },
      { name: 'SmoothNormals', type: 'bool', desc: 'Per-vertex Phong shading for smooth look' },
    ],
  },

  '3dscatter': {
    title: '3D Scatter / Point Cloud',
    desc: '3D point cloud with XYZ projections, orbit gestures, size encoding',
    tags: ['Point Cloud', 'XYZ', 'Orbit', 'Projection'],
    xaml: `<!-- 3D Scatter / point cloud -->
<cn:ScatterChart3DView
    Title="Cluster Analysis"
    Series="{Binding Scatter3DSeries}"
    Camera="{Binding Camera}"
    ShowProjections="True"
    EnableOrbit="True"
    PointShape="Sphere"
    HeightRequest="380" />`,
    csharp: `// 3D scatter / point cloud
var chart = new ScatterChart3DView
{
    Title = "Cluster Analysis",
    ShowProjections = true,
    EnableOrbit = true,
    PointShape = PointShape3D.Sphere,
    Camera = new Camera3D { Yaw = -30, Pitch = 20, Fov = 60 },
};
chart.Series.Add(new ScatterSeries3DModel
{
    Label = "Cluster 1",
    Points = GenerateCluster(centroid: (2, 3, 4), spread: 1.5, count: 80)
});`,
    mvvm: `public partial class ClusterViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<ScatterSeries3DModel> scatter3DSeries = new();
    [ObservableProperty] private Camera3D camera = new() { Yaw = -30, Pitch = 20 };
    [ObservableProperty] private bool showProjections = true;
}`,
    props: [
      { name: 'ShowProjections', type: 'bool', desc: 'Draw XY/XZ/YZ shadow projections on axes planes' },
      { name: 'PointShape', type: 'PointShape3D', desc: 'Sphere, Cube, or Pyramid point shapes' },
      { name: 'PointSize', type: 'double', desc: 'Size of each 3D point glyph' },
      { name: 'EnableOrbit', type: 'bool', desc: 'Pan gesture rotates the 3D view' },
    ],
  },

  pie: {
    title: 'Pie Chart',
    desc: 'Classic pie with explode-on-tap, animated entry, custom labels',
    tags: ['Explode', 'Labels', 'Animated', 'Legend'],
    xaml: `<!-- Pie chart with explode on tap -->
<cn:PieChartView
    Title="Market Share"
    Series="{Binding PieSeries}"
    ShowLabels="True"
    ShowLegend="True"
    ExplodeOnTap="True"
    ExplodeDistance="12"
    AnimationDuration="800"
    HeightRequest="300" />`,
    csharp: `// Pie chart
var chart = new PieChartView
{
    Title = "Market Share",
    ShowLabels = true,
    ShowLegend = true,
    ExplodeOnTap = true,
    ExplodeDistance = 12,
    AnimationDuration = 800,
};
chart.Series.Add(new PieSeriesModel
{
    Slices = {
        new PieSlice { Label = "iOS",     Value = 55 },
        new PieSlice { Label = "Android", Value = 32 },
        new PieSlice { Label = "Other",   Value = 13 },
    }
});`,
    mvvm: `public partial class MarketViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<PieSeriesModel> pieSeries = new();
    [ObservableProperty] private bool explodeOnTap = true;
}`,
    props: [
      { name: 'ExplodeOnTap', type: 'bool', desc: 'Tap a slice to explode it outward' },
      { name: 'ExplodeDistance', type: 'double', desc: 'Pixels to offset exploded slice' },
      { name: 'StartAngle', type: 'double (°)', desc: 'Starting angle for first slice (default -90)' },
      { name: 'ShowLabels', type: 'bool', desc: 'Render value/percentage labels on slices' },
    ],
  },

  donut: {
    title: 'Donut / Doughnut',
    desc: 'Ring chart with center label, multi-ring and progress variants',
    tags: ['Ring', 'Progress', 'Multi-Ring', 'Center Label'],
    xaml: `<!-- Donut chart with center label -->
<cn:DonutChartView
    Title="Storage Usage"
    Series="{Binding DonutSeries}"
    CenterLabel="{Binding UsedPercent, StringFormat='{0:F0}%'}"
    CenterSubLabel="Used"
    HoleRadius="0.6"
    ShowLegend="True"
    AnimationDuration="700"
    HeightRequest="280" />`,
    csharp: `// Donut / doughnut chart
var chart = new DonutChartView
{
    Title = "Storage Usage",
    CenterLabel = "72%",
    CenterSubLabel = "Used",
    HoleRadius = 0.6,
    ShowLegend = true,
};
chart.Series.Add(new DonutSeriesModel
{
    Slices = {
        new PieSlice { Label = "Photos",   Value = 42 },
        new PieSlice { Label = "Apps",     Value = 18 },
        new PieSlice { Label = "System",   Value = 12 },
        new PieSlice { Label = "Free",     Value = 28 },
    }
});`,
    mvvm: `public partial class StorageViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<DonutSeriesModel> donutSeries = new();
    [ObservableProperty] private double usedPercent = 72;
    [ObservableProperty] private double holeRadius = 0.6;
}`,
    props: [
      { name: 'HoleRadius', type: 'double (0–1)', desc: 'Inner hole size as fraction of outer radius' },
      { name: 'CenterLabel', type: 'string', desc: 'Large text displayed in the center hole' },
      { name: 'CenterSubLabel', type: 'string', desc: 'Small subtext below the center label' },
      { name: 'RingSpacing', type: 'double', desc: 'Gap between rings in multi-ring mode (pixels)' },
    ],
  },

  sunburst: {
    title: 'Sunburst Chart',
    desc: 'Hierarchical sunburst with drill-down and breadcrumb navigation',
    tags: ['Hierarchical', 'Drill-Down', 'Breadcrumb', 'Tree'],
    xaml: `<!-- Sunburst chart with drill-down -->
<cn:SunburstView
    Title="File System"
    Root="{Binding FileSystemRoot}"
    EnableDrillDown="True"
    ShowLabels="True"
    AnimationDuration="400"
    HeightRequest="360" />`,
    csharp: `// Sunburst hierarchical chart
var chart = new SunburstView
{
    Title = "File System",
    EnableDrillDown = true,
    ShowLabels = true,
};
chart.Root = new SunburstNode
{
    Label = "Root",
    Children = {
        new SunburstNode { Label = "Documents", Value = 4200,
            Children = {
                new SunburstNode { Label = "Work", Value = 2500 },
                new SunburstNode { Label = "Personal", Value = 1700 },
            }
        },
        new SunburstNode { Label = "Media", Value = 8600 },
        new SunburstNode { Label = "Apps", Value = 2100 },
    }
};`,
    mvvm: `public partial class FilesViewModel : ObservableObject
{
    [ObservableProperty] private SunburstNode fileSystemRoot;
    [ObservableProperty] private bool enableDrillDown = true;
}`,
    props: [
      { name: 'Root', type: 'SunburstNode', desc: 'Root node of the hierarchy tree' },
      { name: 'EnableDrillDown', type: 'bool', desc: 'Tap a segment to zoom into that sub-tree' },
      { name: 'MaxDepth', type: 'int', desc: 'Maximum ring depth to render' },
      { name: 'ShowLabels', type: 'bool', desc: 'Render labels on segments when space allows' },
    ],
  },

  heatmap: {
    title: 'Heatmap',
    desc: 'Calendar heatmap and matrix heatmap with custom color scales',
    tags: ['Calendar', 'Matrix', 'Color Scale', 'Activity'],
    xaml: `<!-- Calendar heatmap (GitHub-style) -->
<cn:HeatmapView
    Title="Commit Activity"
    Data="{Binding CommitData}"
    Mode="Calendar"
    ColorScale="{Binding HeatColorScale}"
    ShowTooltip="True"
    CellSize="12"
    CellSpacing="2"
    HeightRequest="140" />`,
    csharp: `// Calendar heatmap
var heatmap = new HeatmapView
{
    Title = "Commit Activity",
    Mode = HeatmapMode.Calendar,
    ColorScale = ColorScale.GreenFade,
    ShowTooltip = true,
    CellSize = 12,
    CellSpacing = 2,
};
// Key = date, Value = intensity
heatmap.Data = Enumerable.Range(0, 365)
    .ToDictionary(
        i => DateTime.Today.AddDays(-i),
        i => (double)Random.Shared.Next(0, 20));`,
    mvvm: `public partial class ActivityViewModel : ObservableObject
{
    [ObservableProperty] private IDictionary<DateTime, double> commitData;
    [ObservableProperty] private ColorScale heatColorScale = ColorScale.GreenFade;
}`,
    props: [
      { name: 'Mode', type: 'HeatmapMode', desc: 'Calendar (GitHub-style) or Matrix grid' },
      { name: 'ColorScale', type: 'ColorScale', desc: 'Color mapping: GreenFade, BlueFade, HeatMap, Custom' },
      { name: 'CellSize', type: 'double', desc: 'Size of each cell in pixels' },
      { name: 'CellSpacing', type: 'double', desc: 'Gap between cells in pixels' },
    ],
  },

  treemap: {
    title: 'Treemap',
    desc: 'Hierarchical treemap with proportional area and drill-down',
    tags: ['Hierarchical', 'Proportional', 'Squarify', 'Drill-Down'],
    xaml: `<!-- Treemap with squarify layout -->
<cn:TreemapView
    Title="Portfolio"
    Root="{Binding PortfolioRoot}"
    Layout="Squarify"
    ShowLabels="True"
    EnableDrillDown="True"
    HeightRequest="360" />`,
    csharp: `// Treemap chart
var treemap = new TreemapView
{
    Title = "Portfolio Allocation",
    Layout = TreemapLayout.Squarify,
    ShowLabels = true,
    EnableDrillDown = true,
};
treemap.Root = new TreemapNode
{
    Label = "Portfolio",
    Children = {
        new TreemapNode { Label = "Equities", Value = 55000 },
        new TreemapNode { Label = "Bonds",    Value = 25000 },
        new TreemapNode { Label = "Cash",     Value = 10000 },
        new TreemapNode { Label = "Crypto",   Value = 10000 },
    }
};`,
    mvvm: `public partial class PortfolioViewModel : ObservableObject
{
    [ObservableProperty] private TreemapNode portfolioRoot;
    [ObservableProperty] private TreemapLayout layout = TreemapLayout.Squarify;
}`,
    props: [
      { name: 'Layout', type: 'TreemapLayout', desc: 'Squarify (default), Slice, Dice, SliceDice' },
      { name: 'ShowLabels', type: 'bool', desc: 'Render labels on cells when space allows' },
      { name: 'EnableDrillDown', type: 'bool', desc: 'Tap a cell to zoom into child nodes' },
    ],
  },

  waterfall: {
    title: 'Waterfall Chart',
    desc: 'Running total waterfall with positive/negative color encoding',
    tags: ['Bridge', 'P&L', 'Running Total', 'Variance'],
    xaml: `<!-- Waterfall chart for P&L -->
<cn:WaterfallChartView
    Title="Profit & Loss Breakdown"
    Series="{Binding WaterfallSeries}"
    PositiveColor="#22C55E"
    NegativeColor="#EF4444"
    TotalColor="#0EA5E9"
    ShowConnectorLines="True"
    ShowValues="True"
    HeightRequest="300" />`,
    csharp: `// Waterfall / bridge chart
var chart = new WaterfallChartView
{
    Title = "P&L Breakdown",
    PositiveColor = Colors.Green,
    NegativeColor = Colors.Red,
    TotalColor    = Colors.Blue,
    ShowConnectorLines = true,
    ShowValues = true,
};
chart.Series.Add(new WaterfallSeriesModel
{
    Points = {
        new WaterfallPoint { Label = "Revenue",   Value = 100000, Kind = WaterfallKind.Absolute },
        new WaterfallPoint { Label = "COGS",      Value = -35000 },
        new WaterfallPoint { Label = "Gross",     Value = 0,     Kind = WaterfallKind.RunningTotal },
        new WaterfallPoint { Label = "OpEx",      Value = -22000 },
        new WaterfallPoint { Label = "Net Profit",Value = 0,     Kind = WaterfallKind.Total },
    }
});`,
    mvvm: `public partial class PnLViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<WaterfallSeriesModel> waterfallSeries = new();
}`,
    props: [
      { name: 'PositiveColor', type: 'Color', desc: 'Color for positive increment bars' },
      { name: 'NegativeColor', type: 'Color', desc: 'Color for negative decrement bars' },
      { name: 'TotalColor', type: 'Color', desc: 'Color for running total / subtotal bars' },
      { name: 'ShowConnectorLines', type: 'bool', desc: 'Draw dashed lines connecting bar tops' },
    ],
  },

  range: {
    title: 'Range / Band Chart',
    desc: 'High-Low range bands, confidence intervals, error bars',
    tags: ['Hi-Lo', 'Band', 'Confidence', 'Error Bar'],
    xaml: `<!-- Range area chart for temperature highs/lows -->
<cn:RangeChartView
    Title="Temperature Range"
    Series="{Binding RangeSeries}"
    ShowMidLine="True"
    FillOpacity="0.3"
    ShowGrid="True"
    HeightRequest="280" />`,
    csharp: `// Range / band chart
var chart = new RangeChartView
{
    Title = "Temperature Range (°C)",
    ShowMidLine = true,
    FillOpacity = 0.3,
};
chart.Series.Add(new RangeSeriesModel
{
    Label = "Temperature",
    Points = {
        new RangePoint { X = 0, Low = 8,  High = 18, Label = "Mon" },
        new RangePoint { X = 1, Low = 10, High = 22, Label = "Tue" },
        new RangePoint { X = 2, Low = 12, High = 25, Label = "Wed" },
        new RangePoint { X = 3, Low = 9,  High = 20, Label = "Thu" },
        new RangePoint { X = 4, Low = 7,  High = 17, Label = "Fri" },
    }
});`,
    mvvm: `public partial class WeatherViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<RangeSeriesModel> rangeSeries = new();
    [ObservableProperty] private bool showMidLine = true;
}`,
    props: [
      { name: 'ShowMidLine', type: 'bool', desc: 'Draw mid-point line through the range band' },
      { name: 'FillOpacity', type: 'double', desc: 'Band fill opacity (0 = transparent)' },
      { name: 'GradientFill', type: 'bool', desc: 'Apply a gradient from low to high color' },
    ],
  },

  spline: {
    title: 'Spline / Step Line',
    desc: 'Cubic bezier spline and step-line with configurable tension',
    tags: ['Bezier', 'Step', 'Smooth', 'Tension'],
    xaml: `<!-- Spline chart with configurable tension -->
<cn:SplineChartView
    Title="Smooth Trend"
    Series="{Binding SplineSeries}"
    Tension="0.5"
    StrokeWidth="2.5"
    ShowPoints="True"
    ShowGrid="True"
    HeightRequest="260" />`,
    csharp: `// Spline chart
var chart = new SplineChartView
{
    Title = "Smooth Trend",
    Tension = 0.5,
    StrokeWidth = 2.5,
    ShowPoints = true,
};`,
    mvvm: `public partial class TrendViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<LineSeriesModel> splineSeries = new();
    [ObservableProperty] private double tension = 0.5;
}`,
    props: [
      { name: 'Tension', type: 'double (0–1)', desc: 'Bezier tension (0 = linear, 1 = max smooth)' },
      { name: 'StrokeWidth', type: 'double', desc: 'Line thickness in pixels' },
      { name: 'ShowPoints', type: 'bool', desc: 'Render dot markers at each data point' },
    ],
  },

  technical: {
    title: 'Technical Indicators',
    desc: 'Bollinger Bands, SMA/EMA/RSI/MACD/Stochastic overlays',
    tags: ['Bollinger', 'RSI', 'MACD', 'SMA', 'EMA'],
    xaml: `<!-- Technical indicators overlay panel -->
<cn:CandlestickChartView
    Title="TSLA Technical Analysis"
    Series="{Binding CandleSeries}">
  <cn:CandlestickChartView.Indicators>
    <cn:SmaIndicator Period="20" Color="#F97316" />
    <cn:EmaIndicator Period="50" Color="#8B5CF6" />
    <cn:BollingerBandIndicator Period="20" StdDev="2" />
    <cn:RsiIndicator Period="14" OverboughtLevel="70" OversoldLevel="30" />
    <cn:MacdIndicator Fast="12" Slow="26" Signal="9" />
  </cn:CandlestickChartView.Indicators>
</cn:CandlestickChartView>`,
    csharp: `// Add technical indicators programmatically
chart.Indicators.Add(new SmaIndicator        { Period = 20, Color = Colors.Orange });
chart.Indicators.Add(new EmaIndicator        { Period = 50, Color = Colors.Purple });
chart.Indicators.Add(new BollingerBandIndicator { Period = 20, StdDev = 2.0 });
chart.Indicators.Add(new RsiIndicator        { Period = 14, OverboughtLevel = 70, OversoldLevel = 30 });
chart.Indicators.Add(new MacdIndicator       { Fast = 12, Slow = 26, Signal = 9 });
chart.Indicators.Add(new StochasticIndicator { KPeriod = 14, DPeriod = 3 });`,
    mvvm: `public partial class TechAnalysisViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<IChartIndicator> indicators = new()
    {
        new SmaIndicator { Period = 20 },
        new BollingerBandIndicator { Period = 20 },
        new RsiIndicator { Period = 14 },
        new MacdIndicator { Fast = 12, Slow = 26, Signal = 9 }
    };
}`,
    props: [
      { name: 'SmaIndicator.Period', type: 'int', desc: 'Simple Moving Average period' },
      { name: 'EmaIndicator.Period', type: 'int', desc: 'Exponential Moving Average period' },
      { name: 'BollingerBandIndicator.StdDev', type: 'double', desc: 'Standard deviation multiplier (default 2.0)' },
      { name: 'RsiIndicator.Period', type: 'int', desc: 'RSI calculation period (default 14)' },
      { name: 'MacdIndicator.Fast/Slow/Signal', type: 'int', desc: 'MACD line periods (12/26/9 standard)' },
    ],
  },

  funnel: {
    title: 'Funnel / Pyramid',
    desc: 'Sales funnel and pyramid charts with conversion rate labels',
    tags: ['Funnel', 'Pyramid', 'Conversion', 'Sales'],
    xaml: `<!-- Sales funnel chart -->
<cn:FunnelChartView
    Title="Conversion Funnel"
    Series="{Binding FunnelSeries}"
    Mode="Funnel"
    ShowPercentage="True"
    ShowValues="True"
    HeightRequest="320" />`,
    csharp: `// Funnel chart
var chart = new FunnelChartView
{
    Title = "Conversion Funnel",
    Mode = FunnelMode.Funnel,
    ShowPercentage = true,
    ShowValues = true,
};
chart.Series.Add(new FunnelSeriesModel
{
    Segments = {
        new FunnelSegment { Label = "Visitors",  Value = 10000 },
        new FunnelSegment { Label = "Sign-Ups",  Value = 6500  },
        new FunnelSegment { Label = "Free Trial",Value = 3200  },
        new FunnelSegment { Label = "Paid",      Value = 980   },
    }
});`,
    mvvm: `public partial class ConversionViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<FunnelSeriesModel> funnelSeries = new();
    [ObservableProperty] private FunnelMode mode = FunnelMode.Funnel;
    [ObservableProperty] private bool showPercentage = true;
}`,
    props: [
      { name: 'Mode', type: 'FunnelMode', desc: 'Funnel (tapering) or Pyramid (ascending)' },
      { name: 'ShowPercentage', type: 'bool', desc: 'Display conversion % between segments' },
      { name: 'ShowValues', type: 'bool', desc: 'Display raw value labels on segments' },
      { name: 'NeckWidth', type: 'double', desc: 'Width of the bottom neck in funnel mode' },
    ],
  },

  lineargauge: {
    title: 'Linear / Bullet Gauge',
    desc: 'Horizontal and vertical bar gauges with target markers and zones',
    tags: ['Bullet', 'Progress', 'KPI', 'Target'],
    xaml: `<!-- Bullet / linear gauge -->
<cn:LinearGaugeView
    Title="Sales Target"
    Value="{Binding SalesValue}"
    Target="{Binding SalesTarget}"
    Minimum="0"
    Maximum="100000"
    Orientation="Horizontal"
    ShowTarget="True"
    ShowZones="True"
    HeightRequest="80">
  <cn:LinearGaugeView.Ranges>
    <cn:GaugeRange From="0"     To="40000" Color="#EF4444" />
    <cn:GaugeRange From="40000" To="70000" Color="#F97316" />
    <cn:GaugeRange From="70000" To="100000" Color="#22C55E" />
  </cn:LinearGaugeView.Ranges>
</cn:LinearGaugeView>`,
    csharp: `// Linear / bullet gauge
var gauge = new LinearGaugeView
{
    Title = "Sales Target",
    Minimum = 0,
    Maximum = 100000,
    Orientation = GaugeOrientation.Horizontal,
    ShowTarget = true,
    ShowZones = true,
};
gauge.Ranges.Add(new GaugeRange { From = 0,     To = 40000, Color = Colors.Red    });
gauge.Ranges.Add(new GaugeRange { From = 40000, To = 70000, Color = Colors.Orange });
gauge.Ranges.Add(new GaugeRange { From = 70000, To = 100000,Color = Colors.Green  });
gauge.SetBinding(LinearGaugeView.ValueProperty, "SalesValue");
gauge.SetBinding(LinearGaugeView.TargetProperty, "SalesTarget");`,
    mvvm: `public partial class KpiViewModel : ObservableObject
{
    [ObservableProperty] private double salesValue = 72000;
    [ObservableProperty] private double salesTarget = 85000;
    [ObservableProperty] private GaugeOrientation orientation = GaugeOrientation.Horizontal;
}`,
    props: [
      { name: 'Value', type: 'double', desc: 'Current value fill level (bindable)' },
      { name: 'Target', type: 'double', desc: 'Target marker position on the gauge' },
      { name: 'Orientation', type: 'GaugeOrientation', desc: 'Horizontal or Vertical layout' },
      { name: 'ShowTarget', type: 'bool', desc: 'Display a target marker line/triangle' },
    ],
  },
};

/* ── Modal open/close ───────────────────────────────────── */
let activeChartKey = null;
let activeTab = 'xaml';

function openChart(key) {
  const data = chartData[key];
  if (!data) return;

  activeChartKey = key;
  activeTab = 'xaml';

  document.getElementById('modalTitle').textContent = data.title;
  document.getElementById('modalDesc').textContent = data.desc;

  const tagsEl = document.getElementById('modalTags');
  tagsEl.innerHTML = data.tags.map(t => `<span>${t}</span>`).join('');

  // Reset tabs
  document.querySelectorAll('.mtab').forEach((t, i) => {
    t.classList.toggle('active', i === 0);
  });

  renderCode('xaml');
  renderProps(data.props);

  document.getElementById('chartModal').classList.add('open');
  document.body.style.overflow = 'hidden';
}

function renderCode(tab) {
  const data = chartData[activeChartKey];
  if (!data) return;
  const code = data[tab] || '// Not available for this chart type';
  document.getElementById('modalCodeInner').textContent = code;
}

function renderProps(props) {
  const el = document.getElementById('modalProps');
  el.innerHTML = (props || []).map(p => `
    <div class="prop-item">
      <div class="prop-name">${p.name}</div>
      <div class="prop-type">${p.type}</div>
      <div class="prop-desc">${p.desc}</div>
    </div>
  `).join('');
}

function switchCodeTab(tab, btn) {
  document.querySelectorAll('.mtab').forEach(t => t.classList.remove('active'));
  btn.classList.add('active');
  activeTab = tab;
  renderCode(tab);
}

function closeModal(e) {
  if (e.target === document.getElementById('chartModal')) closeModalBtn();
}
function closeModalBtn() {
  document.getElementById('chartModal').classList.remove('open');
  document.body.style.overflow = '';
}

document.addEventListener('keydown', e => {
  if (e.key === 'Escape') closeModalBtn();
});

function copyCode() {
  const code = document.getElementById('modalCodeInner').textContent;
  const btn = document.querySelector('.code-copy-btn');
  navigator.clipboard.writeText(code).then(() => {
    const orig = btn.innerHTML;
    btn.innerHTML = `<svg width="13" height="13" viewBox="0 0 24 24" fill="none" stroke="#22C55E" stroke-width="2"><polyline points="20 6 9 17 4 12"/></svg> Copied!`;
    btn.style.color = '#22C55E';
    setTimeout(() => { btn.innerHTML = orig; btn.style.color = ''; }, 2000);
  });
}

/* ── Smooth scroll offset for fixed nav ────────────────── */
document.querySelectorAll('a[href^="#"]').forEach(a => {
  a.addEventListener('click', e => {
    const id = a.getAttribute('href').slice(1);
    const target = document.getElementById(id);
    if (!target) return;
    e.preventDefault();
    const offset = 68 + 16;
    const top = target.getBoundingClientRect().top + window.scrollY - offset;
    window.scrollTo({ top, behavior: 'smooth' });
  });
});

/* ── Animate elements on scroll ────────────────────────── */
const obs = new IntersectionObserver(entries => {
  entries.forEach(entry => {
    if (entry.isIntersecting) {
      entry.target.style.opacity = '1';
      entry.target.style.transform = 'translateY(0)';
    }
  });
}, { threshold: 0.1 });

document.querySelectorAll('.chart-card, .feature-card, .pkg-card, .step').forEach(el => {
  el.style.opacity = '0';
  el.style.transform = 'translateY(20px)';
  el.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
  obs.observe(el);
});
