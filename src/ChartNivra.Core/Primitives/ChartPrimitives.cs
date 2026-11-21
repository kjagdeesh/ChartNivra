namespace ChartNivra.Core.Primitives;

/// <summary>
/// Represents a 2D point with double precision.
/// </summary>
public readonly record struct ChartPoint(double X, double Y)
{
    public static readonly ChartPoint Zero = new(0, 0);
    public static readonly ChartPoint Empty = new(double.NaN, double.NaN);

    public bool IsEmpty => double.IsNaN(X) || double.IsNaN(Y);
}

/// <summary>
/// Represents a 2D size with double precision.
/// </summary>
public readonly record struct ChartSize(double Width, double Height)
{
    public static readonly ChartSize Zero = new(0, 0);
    public static readonly ChartSize Empty = new(double.NaN, double.NaN);

    public bool IsEmpty => double.IsNaN(Width) || double.IsNaN(Height);
}

/// <summary>
/// Represents a 2D bounding rectangle with double precision.
/// </summary>
public readonly record struct ChartRect(double X, double Y, double Width, double Height)
{
    public static readonly ChartRect Zero = new(0, 0, 0, 0);

    public double Left => X;
    public double Top => Y;
    public double Right => X + Width;
    public double Bottom => Y + Height;

    public ChartPoint Center => new(X + Width / 2.0, Y + Height / 2.0);

    public bool Contains(double px, double py) =>
        px >= Left && px <= Right && py >= Top && py <= Bottom;

    public bool Contains(ChartPoint pt) => Contains(pt.X, pt.Y);
}

/// <summary>
/// Represents an RGBA color structure.
/// </summary>
public readonly record struct ChartColor(byte R, byte G, byte B, byte A = 255)
{
    public static readonly ChartColor Transparent = new(0, 0, 0, 0);
    public static readonly ChartColor Black = new(0, 0, 0, 255);
    public static readonly ChartColor White = new(255, 255, 255, 255);

    public static ChartColor FromArgb(byte a, byte r, byte g, byte b) => new(r, g, b, a);
    public static ChartColor FromRgb(byte r, byte g, byte b) => new(r, g, b, 255);

    public static ChartColor FromHex(string hex)
    {
        if (string.IsNullOrWhiteSpace(hex)) return Black;
        hex = hex.TrimStart('#');
        if (hex.Length == 6)
        {
            return new ChartColor(
                Convert.ToByte(hex[..2], 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16)
            );
        }
        if (hex.Length == 8)
        {
            return new ChartColor(
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16),
                Convert.ToByte(hex.Substring(6, 2), 16),
                Convert.ToByte(hex[..2], 16)
            );
        }
        return Black;
    }

    public ChartColor WithAlpha(byte a) => new(R, G, B, a);
}
