# ChartNivra

ChartNivra is a charting library and interactive sample gallery for .NET MAUI. It uses SkiaSharp to render charts for Android, iOS, Mac Catalyst, and Windows, and also exposes a plain .NET target for models and rendering code.

> **Development status:** ChartNivra is under active development. The local NuGet package is a preview for testing; production readiness and all platform combinations have not yet been verified.

## Screenshots

The screenshots below show the Gallery on desktop and mobile. They are snapshots of the application and may not reflect every subsequent change.

### Desktop

| Chart gallery | Radar sample |
| --- | --- |
| ![Desktop chart gallery](images/File%202.png) | ![Desktop radar sample and source code](images/File%201.png) |
| ![Chart gallery, circular and financial samples](images/File%203.png) | ![Chart gallery, gauges and 3D samples](images/File%204.png) |

![Real-time chart and source code on desktop](images/File%205.png)

### Mobile

<p>
  <img src="images/mobile%20%286%29.jpg" alt="Mobile gallery" width="185" />
  <img src="images/mobile%20%285%29.jpg" alt="Mobile line chart" width="185" />
  <img src="images/mobile%20%281%29.jpg" alt="Mobile candlestick chart" width="185" />
</p>
<p>
  <img src="images/mobile%20%284%29.jpg" alt="Mobile heatmap" width="185" />
  <img src="images/mobile%20%283%29.jpg" alt="Mobile funnel chart" width="185" />
  <img src="images/mobile%20%282%29.jpg" alt="Mobile 3D visualization studio" width="185" />
</p>

## Chart gallery

The Gallery lists 23 chart types and five additional demos:

- **Cartesian:** line, bar/column, area, scatter/bubble, spline/step, range/band, and waterfall.
- **Circular and hierarchical:** pie, donut, sunburst, and treemap.
- **Statistical and financial:** radar, heatmap, histogram, box plot, candlestick/OHLC, and technical indicators.
- **Gauges and conversion:** radial gauge, linear/bullet gauge, and funnel/pyramid.
- **3D:** column/bar, surface/mesh, and scatter/point cloud.
- **Demos:** streaming, 3D visualization studio, annotations, real-time chart, and multiple series.

The Gallery includes light/dark appearance options, chart sample pages, and C#/XAML views where available. Some studio pages are implemented entirely in C#. Chart support and interactions vary by renderer; see the samples and source before relying on a specific feature.

## Repository structure

| Location | Purpose |
| --- | --- |
| [`src/ChartNivra`](src/ChartNivra/) | Chart models, rendering, and MAUI control library. |
| [`samples/ChartNivra.Gallery`](samples/ChartNivra.Gallery/) | Interactive Gallery using a project reference to the library. |
| [`tests/ChartNivra.Tests`](tests/ChartNivra.Tests/) | Consolidated xUnit test project. |
| [`tests/ChartNivra.PackageSmoke`](tests/ChartNivra.PackageSmoke/) | Copy of Gallery that consumes a local preview NuGet package. |
| [`benchmarks/ChartNivra.Benchmarks`](benchmarks/ChartNivra.Benchmarks/) | BenchmarkDotNet benchmarks for selected core operations. |
| [`docs/site`](docs/site/) | Static documentation website. |
| [`images`](images/) | Screenshots used in this README. |

## Requirements

- .NET 10 SDK and the .NET MAUI workload for the target platform.
- Visual Studio with .NET MAUI support on Windows, or a compatible .NET MAUI development environment on macOS.
- Xcode and a compatible Apple workload on a Mac for iOS and Mac Catalyst. Deploying to an iPhone requires appropriate signing and provisioning.
- Android SDK and an emulator or device for Android development.

Check the selected SDK, workloads, and Xcode version with `dotnet --version`, `dotnet workload list`, and `xcodebuild -version` on macOS.

## Run Gallery

Open `ChartNivra.slnx` and select **ChartNivra.Gallery** as the startup project, or build a target from the repository root:

```sh
# Windows
dotnet build samples/ChartNivra.Gallery/ChartNivra.Gallery.csproj -f net10.0-windows10.0.19041.0

# Android
dotnet build samples/ChartNivra.Gallery/ChartNivra.Gallery.csproj -f net10.0-android

# Mac Catalyst, on a Mac
dotnet build samples/ChartNivra.Gallery/ChartNivra.Gallery.csproj -f net10.0-maccatalyst
dotnet build samples/ChartNivra.Gallery/ChartNivra.Gallery.csproj -f net10.0-maccatalyst -t:Run
```

Select a connected Android device, iOS device/simulator, or Windows target in the IDE to deploy and run. See the [Gallery README](samples/ChartNivra.Gallery/README.md) for details.

## Use a chart

Register SkiaSharp in the MAUI application:

```csharp
using SkiaSharp.Views.Maui.Controls.Hosting;

builder.UseMauiApp<App>().UseSkiaSharp();
```

Add a chart host in XAML:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:charts="clr-namespace:ChartNivra.Maui.Controls;assembly=ChartNivra"
             x:Class="MyApp.SamplePage">
    <charts:ChartView x:Name="chartView" HeightRequest="320" />
</ContentPage>
```

Configure the chart after `InitializeComponent()` in the page's code-behind:

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

The Gallery references the library project directly. The separate [package test app](tests/ChartNivra.PackageSmoke/README.md) references a locally generated preview package instead; no package is published by the ordinary build.

## Tests and benchmarks

```sh
dotnet test tests/ChartNivra.Tests/ChartNivra.Tests.csproj
dotnet run -c Release --project benchmarks/ChartNivra.Benchmarks/ChartNivra.Benchmarks.csproj -- --filter "*"
```

The solution includes the package test project, which requires `ChartNivra.0.1.0-preview.1.nupkg` in `artifacts/local-feed` or another configured source before a full solution restore. See its [README](tests/ChartNivra.PackageSmoke/README.md). Benchmark results are machine-specific and are not a substitute for rendering or device tests.

## Current limitations

- The last Android builds reported a 16 KB page-size warning for the SkiaSharp native dependency; this needs resolution and device verification.
- Windows, Android, and Mac Catalyst have been exercised during development, but a complete platform, device, and chart regression matrix remains to be run. iOS deployment requires separate verification.
- The previous automated test run in this environment could not connect to its test host. Run the suite in your development environment and review the results.
- Gallery snippets are examples. Some need further validation against the exact chart setup and data; the documentation site mirrors Gallery's displayed snippets.
- The website data is generated from Gallery source with `python tools/sync_gallery_site.py`; rerun it when Gallery cards or snippets change.

## License

The library project declares `MIT` as its package license expression. Check the repository for a license file before distributing the project or package.
