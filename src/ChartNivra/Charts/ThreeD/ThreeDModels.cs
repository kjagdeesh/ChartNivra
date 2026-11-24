namespace ChartNivra.ThreeD;

using ChartNivra.Core.Primitives;
using ChartNivra.Core.ThreeD;

/// <summary>
/// Data point for 3D Bar/Column series.
/// </summary>
public record Column3DDataPoint(double X, double Y, double Z, double Width = 0.8, double Depth = 0.8, ChartColor? Color = null, string? Label = null);

/// <summary>
/// 3D Column / Bar Chart model.
/// </summary>
public class ColumnChart3DModel
{
    public string Title { get; set; } = "3D Column Chart";
    public Camera3D Camera { get; set; } = new();
    public List<Column3DDataPoint> Points { get; set; } = new();
    public ChartColor DefaultColor { get; set; } = ChartColor.FromHex("#00C096");
    public bool ShowWireframe { get; set; } = true;
    public bool EnableShading { get; set; } = true;
    public string XAxisTitle { get; set; } = "Category";
    public string YAxisTitle { get; set; } = "Value";
    public string ZAxisTitle { get; set; } = "Series";

    public void AddPoint(double x, double height, double z, ChartColor? color = null, string? label = null)
    {
        Points.Add(new Column3DDataPoint(x, height, z, Color: color, Label: label));
    }
}

/// <summary>
/// 3D Surface grid representation with height function z = f(x, y).
/// </summary>
public class SurfaceChart3DModel
{
    public string Title { get; set; } = "3D Surface Chart";
    public Camera3D Camera { get; set; } = new();
    public int ResolutionX { get; set; } = 20;
    public int ResolutionY { get; set; } = 20;
    public double MinX { get; set; } = -5.0;
    public double MaxX { get; set; } = 5.0;
    public double MinY { get; set; } = -5.0;
    public double MaxY { get; set; } = 5.0;
    public Func<double, double, double>? HeightFunction { get; set; }
    public ChartColor LowColor { get; set; } = ChartColor.FromHex("#3B82F6");
    public ChartColor HighColor { get; set; } = ChartColor.FromHex("#EF4444");
    public string XAxisTitle { get; set; } = "X axis";
    public string YAxisTitle { get; set; } = "Z axis";
    public string ValueTitle { get; set; } = "Height";

    public double[,] GenerateGrid()
    {
        var grid = new double[ResolutionX, ResolutionY];
        var stepX = (MaxX - MinX) / (ResolutionX - 1);
        var stepY = (MaxY - MinY) / (ResolutionY - 1);

        for (var i = 0; i < ResolutionX; i++)
        {
            var x = MinX + i * stepX;
            for (var j = 0; j < ResolutionY; j++)
            {
                var y = MinY + j * stepY;
                grid[i, j] = HeightFunction?.Invoke(x, y) ?? Math.Sin(Math.Sqrt(x * x + y * y));
            }
        }

        return grid;
    }
}

/// <summary>
/// 3D Scatter point with coordinates and optional size/color.
/// </summary>
public record Scatter3DDataPoint(double X, double Y, double Z, double Size = 5.0, ChartColor? Color = null, string? Label = null);

/// <summary>
/// 3D Scatter plot model.
/// </summary>
public class ScatterChart3DModel
{
    public string Title { get; set; } = "3D Scatter Plot";
    public Camera3D Camera { get; set; } = new();
    public List<Scatter3DDataPoint> Points { get; set; } = new();
    public ChartColor DefaultColor { get; set; } = ChartColor.FromHex("#8B5CF6");
    public string XAxisTitle { get; set; } = "X";
    public string YAxisTitle { get; set; } = "Y";
    public string ZAxisTitle { get; set; } = "Z";

    public void AddPoint(double x, double y, double z, double size = 5.0, ChartColor? color = null, string? label = null)
    {
        Points.Add(new Scatter3DDataPoint(x, y, z, size, color, label));
    }
}
