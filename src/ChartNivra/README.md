# ChartNivra library

This project contains the charting library used by the Gallery. It combines models, renderers, SkiaSharp drawing, and the .NET MAUI `ChartView` in one project.

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

Use the appropriate platform target, workload, and host to build a MAUI target. The [root README](../../README.md) has a `ChartView` example, and the [Gallery](../../samples/ChartNivra.Gallery/) contains additional examples.

## Package status

The package ID is `ChartNivra`. Automatic package generation on build is disabled. The [package test project](../../tests/ChartNivra.PackageSmoke/README.md) consumes a local preview package; build or publish a package only when explicitly needed. A fresh checkout needs the local preview feed prepared before restoring the complete solution.

The package's metadata declares an MIT license expression. The current Android dependency has produced a 16 KB native page-size warning, which needs investigation before production distribution.
