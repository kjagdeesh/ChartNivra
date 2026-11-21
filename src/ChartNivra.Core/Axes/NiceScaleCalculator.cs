namespace ChartNivra.Core.Axes;

using ChartNivra.Core.Primitives;

/// <summary>
/// Represents a numerical tick along an axis.
/// </summary>
public readonly record struct AxisTick(double Value, string Label, double Position);

/// <summary>
/// Represents a numeric range [Min, Max].
/// </summary>
public readonly record struct DataRange(double Min, double Max)
{
    public double Delta => Max - Min;
    public bool IsValid => !double.IsNaN(Min) && !double.IsNaN(Max) && Max >= Min;

    public DataRange Expand(double marginFactor)
    {
        if (!IsValid) return this;
        var delta = Delta;
        if (delta == 0) delta = 1.0;
        var pad = delta * marginFactor;
        return new DataRange(Min - pad, Max + pad);
    }
}

/// <summary>
/// High-performance "nice numbers" tick generator for linear scales.
/// </summary>
public static class NiceScaleCalculator
{
    public static (double niceMin, double niceMax, double step) Calculate(double min, double max, int maxTicks = 5)
    {
        if (min > max) (min, max) = (max, min);
        if (Math.Abs(max - min) < 1e-9)
        {
            min -= 1.0;
            max += 1.0;
        }

        var range = NiceNum(max - min, false);
        var step = NiceNum(range / (maxTicks - 1), true);
        var niceMin = Math.Floor(min / step) * step;
        var niceMax = Math.Ceiling(max / step) * step;

        return (niceMin, niceMax, step);
    }

    private static double NiceNum(double range, bool round)
    {
        var exponent = Math.Floor(Math.Log10(range));
        var fraction = range / Math.Pow(10, exponent);
        double niceFraction;

        if (round)
        {
            if (fraction < 1.5) niceFraction = 1;
            else if (fraction < 3) niceFraction = 2;
            else if (fraction < 7) niceFraction = 5;
            else niceFraction = 10;
        }
        else
        {
            if (fraction <= 1) niceFraction = 1;
            else if (fraction <= 2) niceFraction = 2;
            else if (fraction <= 5) niceFraction = 5;
            else niceFraction = 10;
        }

        return niceFraction * Math.Pow(10, exponent);
    }
}
