# ChartNivra library

This project contains the charting library used by the Gallery. It combines models, renderers, SkiaSharp drawing, and the .NET MAUI `ChartView` in one project.

## Install and use

After the stable package is published, add it to your MAUI project:

```sh
dotnet add package ChartNivra --version 1.0.0
```

In `MauiProgram.cs`, register the SkiaSharp MAUI controls:

```csharp
using SkiaSharp.Views.Maui.Controls.Hosting;

builder.UseMauiApp<App>().UseSkiaSharp();
```

Add a `ChartView` to a page, replacing `MyApp.SamplePage` with your own page class:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:charts="clr-namespace:ChartNivra.Maui.Controls;assembly=ChartNivra"
             x:Class="MyApp.SamplePage">
    <charts:ChartView x:Name="chartView" HeightRequest="320" />
</ContentPage>
```

After `InitializeComponent()` in the page's code-behind, add a series:

```csharp
using ChartNivra.Core.Primitives;
using ChartNivra.Core.Series;

chartView.Title = "Revenue";
chartView.Model.Series.Add(new LineSeriesModel
{
    Title = "Revenue",
    Color = ChartColor.FromHex("#00C096"),
    ShowMarkers = true,
    Points = [new(0, 28), new(1, 43), new(2, 36), new(3, 62)]
});
```

## Structure

- `Core`: chart models, axes, primitives, series, themes, data structures, and transforms.
- `Charts`: specialized chart families, including financial, statistical, circular, gauges, and 3D models.
- `Rendering`: rendering abstractions and Skia-based renderers.
- `Maui`: the MAUI chart control and platform integration.

The project targets plain `net10.0` and .NET 10 Android, iOS, and Mac Catalyst. The Windows target is included when building on Windows. The plain .NET target excludes the MAUI controls.

## Build

From the repository root:

```sh
dotnet build src/ChartNivra/ChartNivra.csproj -f net10.0
```

Use the appropriate platform target, workload, and host to build a MAUI target. The [project README](https://github.com/kjagdeesh/ChartNivra) has a `ChartView` example, and the [Gallery](https://github.com/kjagdeesh/ChartNivra/tree/development/samples/ChartNivra.Gallery) contains additional examples.

## Package status

The package ID is `ChartNivra`. Automatic package generation on build is disabled. The [package test project](https://github.com/kjagdeesh/ChartNivra/tree/development/tests/ChartNivra.PackageSmoke) consumes a local preview package; build or publish a package only when explicitly needed. A fresh checkout needs the local preview feed prepared before restoring the complete solution.

The package's metadata declares an MIT license expression. The current Android dependency has produced a 16 KB native page-size warning, which needs investigation before production distribution.
