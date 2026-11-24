namespace ChartNivra.Core.Decimation;

using ChartNivra.Core.Primitives;

/// <summary>
/// Largest-Triangle-Three-Buckets (LTTB) downsampling algorithm for high-performance rendering.
/// Downsamples large datasets (10,000+ points) to target pixel thresholds while preserving peaks and visual shape.
/// </summary>
public static class LttbDecimation
{
    public static IReadOnlyList<ChartPoint> Downsample(IReadOnlyList<ChartPoint> data, int threshold)
    {
        if (data == null || data.Count <= threshold || threshold <= 2)
            return data ?? Array.Empty<ChartPoint>();

        var sampled = new List<ChartPoint>(threshold);
        var bucketSize = (double)(data.Count - 2) / (threshold - 2);

        var a = 0;
        sampled.Add(data[a]); // Always add the first point

        for (var i = 0; i < threshold - 2; i++)
        {
            var avgX = 0.0;
            var avgY = 0.0;
            var avgRangeStart = (int)Math.Floor((i + 1) * bucketSize) + 1;
            var avgRangeEnd = (int)Math.Floor((i + 2) * bucketSize) + 1;
            if (avgRangeEnd > data.Count) avgRangeEnd = data.Count;

            var avgRangeLength = avgRangeEnd - avgRangeStart;
            for (var j = avgRangeStart; j < avgRangeEnd; j++)
            {
                avgX += data[j].X;
                avgY += data[j].Y;
            }
            if (avgRangeLength > 0)
            {
                avgX /= avgRangeLength;
                avgY /= avgRangeLength;
            }

            var rangeOffs = (int)Math.Floor(i * bucketSize) + 1;
            var rangeTo = (int)Math.Floor((i + 1) * bucketSize) + 1;
            if (rangeTo > data.Count) rangeTo = data.Count;

            var pointAX = data[a].X;
            var pointAY = data[a].Y;

            var maxArea = -1.0;
            var nextA = rangeOffs;

            for (var j = rangeOffs; j < rangeTo; j++)
            {
                var area = Math.Abs(
                    (pointAX - avgX) * (data[j].Y - pointAY) -
                    (pointAX - data[j].X) * (avgY - pointAY)
                ) * 0.5;

                if (area > maxArea)
                {
                    maxArea = area;
                    nextA = j;
                }
            }

            sampled.Add(data[nextA]);
            a = nextA;
        }

        sampled.Add(data[^1]); // Always add the last point
        return sampled;
    }
}
