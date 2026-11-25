# ChartNivra.Maui

> **A modern, high-performance 2D and 3D charting framework for .NET MAUI.**

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET-9.0%20%7C%2010.0-purple.svg)](https://dotnet.microsoft.com/)

---

## 🌟 Overview

**ChartNivra.Maui** is a comprehensive, modular charting framework built for .NET MAUI targeting Android, iOS, macOS (Mac Catalyst), and Windows.

Unlike lightweight wrappers or legacy libraries, ChartNivra is engineered from the ground up to offer:
- **Rich 2D Charts**: Line, Spline, Area, Bar, Column, Scatter, Bubble, Pie, Donut, Gauges, Heatmaps, Financial and Statistical charts.
- **True 3D Analytical Charts**: Real 3D geometry, perspective/orthographic cameras, orbit/pan/zoom, lighting, materials, depth testing, and spatial hit testing (not fake 2D shadows or extruded slices).
- **High-Performance Architecture**: Decimation (LTTB / Min-Max), incremental geometry updates, layered invalidation, dirty rect rendering, and zero unnecessary hot-path allocations.
- **Enterprise Theming & Palettes**: First-class dark/light/system/contrast themes, accessible color-blind safe palettes, conditional coloring, and smooth animations.
- **Clean MVVM & XAML First**: Intuitive XAML syntax, strong data binding, dynamic collection change support (`INotifyCollectionChanged`), and C# fluent configuration.

---

## 🏛 Solution Architecture

```text
ChartNivra/
├── src/
│   ├── ChartNivra.Core/                  # Pure .NET: Models, Math, Transforms, Axis algorithms, Palettes
│   ├── ChartNivra.Rendering.Abstractions/# IRenderer, ICanvas, IGeometryBuffer, ICamera, IPen, IBrush
│   ├── ChartNivra.Rendering.Skia/        # SkiaSharp 2D accelerated rendering backend
│   ├── ChartNivra.Rendering.3D/          # True 3D engine: Camera, Mesh, Projection, Shaders, Depth
│   ├── ChartNivra.Maui/                  # MAUI handlers, ChartHostView, Bindable properties, Gestures
│   ├── ChartNivra.Cartesian/             # Cartesian 2D series (Line, Bar, Column, Area, Scatter, Spline)
│   ├── ChartNivra.Pie/                   # Pie, Donut, Sunburst
│   ├── ChartNivra.Financial/             # Candlestick, OHLC, Volume, Technical Indicators
│   ├── ChartNivra.ThreeD/                # 3D Cartesian, 3D Surface/Wireframe, 3D Scatter
│   └── ChartNivra.Export/                # Raster (PNG/JPEG) and Vector (SVG/PDF) export engine
├── tests/
│   └── ChartNivra.Tests/                 # Unit, rendering, visual, integration, performance and cross-platform tests
├── samples/
│   └── ChartNivra.Gallery/               # Multi-platform .NET MAUI showcase app
└── docs/                                 # Architecture specs, guides, API documentation
```

Run the consolidated test suite with `dotnet test tests/ChartNivra.Tests/ChartNivra.Tests.csproj`.

---

## 🚀 Quick Start (XAML Example)

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:charts="clr-namespace:ChartNivra.Maui;assembly=ChartNivra.Maui"
             x:Class="DemoApp.MainPage">

    <charts:CartesianChart Title="Revenue Growth"
                           Theme="Dark"
                           Palette="Emerald"
                           IsLegendVisible="True">
        <charts:CartesianChart.XAxes>
            <charts:CategoryAxis />
        </charts:CartesianChart.XAxes>
        
        <charts:CartesianChart.YAxes>
            <charts:NumericAxis LabelFormat="C0" />
        </charts:CartesianChart.YAxes>

        <charts:CartesianChart.Series>
            <charts:LineSeries Label="2026 Sales"
                               ItemsSource="{Binding MonthlySales}"
                               XBinding="Month"
                               YBinding="Amount"
                               ShowMarkers="True" />
        </charts:CartesianChart.Series>
    </charts:CartesianChart>
</ContentPage>
```

---

## 🗺 Implementation Roadmap

- [x] **Phase 0**: Project specification, architectural definitions, Git & repo setup.
- [ ] **Phase 1**: Solution foundation, Core math/transforms, SkiaSharp 2D abstraction, MAUI `ChartHostView`, Basic Numeric Axis & Line Series.
- [ ] **Phase 2**: Full 2D Cartesian suite (Bar, Column, Spline, Area, Scatter), Pie/Donut, Legend, Tooltip.
- [ ] **Phase 3**: Gestures (Pinch zoom, drag pan, crosshair, trackball, selection) & easing animation subsystem.
- [ ] **Phase 4**: Real-time batching, LTTB decimation, streaming ring buffers, memory benchmarks.
- [ ] **Phase 5**: Advanced 2D (Heatmaps, Radar, BoxPlot, Gauges) & Image/PDF export.
- [ ] **Phase 6**: True 3D Subsystem (Camera, 3D Cartesian coordinates, Surface/Wireframe mesh, 3D Column, Orbit/Pan/Zoom).
- [ ] **Phase 7**: Financial charts (Candlestick, OHLC, MACD, RSI, Bollinger Bands).
- [ ] **Phase 8**: Production hardening, automated visual regression CI, NuGet packaging.

---

## 📄 License

MIT License. See [LICENSE](LICENSE) for details.
