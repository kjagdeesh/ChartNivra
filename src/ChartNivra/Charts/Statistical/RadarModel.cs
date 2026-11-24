namespace ChartNivra.Statistical;

using ChartNivra.Core.Primitives;

/// <summary>
/// Series in a multi-axis Radar / Spider chart.
/// </summary>
public class RadarSeriesModel
{
    public string Name { get; set; }
    public ChartColor Color { get; set; }
    public List<double> Values { get; set; } = new();
    public double FillOpacity { get; set; } = 0.3;

    public RadarSeriesModel(string name, ChartColor color)
    {
        Name = name;
        Color = color;
    }
}

/// <summary>
/// Radar / Spider chart model with equiangular spokes.
/// </summary>
public class RadarChartModel
{
    public string Title { get; set; } = "Radar Chart";
    public ChartNivra.Core.Themes.ChartTheme Theme { get; set; } = ChartNivra.Core.Themes.ChartTheme.Light;
    public List<string> Axes { get; set; } = new();
    public List<RadarSeriesModel> Series { get; set; } = new();
    public double MaxScale { get; set; } = 100.0;
    public int GridRings { get; set; } = 5;
}
