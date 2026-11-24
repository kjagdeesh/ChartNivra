# ChartNivra

ChartNivra is a cross-platform .NET charting library for .NET MAUI, Android, iOS, Mac Catalyst, and Windows. It combines 23 interactive gallery chart types, plus supporting variations and rendering features, in one NuGet package.

## Chart families

- Cartesian: line, multi-series, bar/column, area, bubble, step, range, waterfall, histogram, and bullet.
- Circular and hierarchical: pie, donut, sunburst, and treemap.
- Statistical: radar, heatmap, and box plot.
- Financial: candlestick/OHLC and technical indicators.
- Gauges and conversion: radial gauge and funnel.
- 3D: column, surface/mesh, and scatter/point cloud.

## MAUI setup

```xml
<PackageReference Include="ChartNivra" Version="1.0.0" />
```

```csharp
using ChartNivra.Core.Series;
using ChartNivra.Core.Primitives;

chartView.Model.Series.Add(new LineSeriesModel
{
    Title = "Revenue",
    Color = ChartColor.FromHex("#38BDF8"),
    Points = [new(0, 28), new(1, 43), new(2, 36), new(3, 62)]
});
```

See the separate `samples/ChartNivra.Gallery` project for interactive examples and full C#/XAML implementation references.
