# ChartNivra Feature Coverage & Architecture Alignment Matrix

Last Updated: November 2026

## 1. Project & Directory Structure Status

The master specification outlines the full target architecture below. Here is the current progress status across all modules:

| Target Project / Folder | Status | Current Implementation Notes |
|---|---|---|
| **`src/ChartNivra.Core/`** | 🟢 Implemented | Primitives, Axes (`NiceScaleCalculator`), Transforms (`CartesianTransform`), Themes & Palettes, Decimation (`LttbDecimation`), Base series & data structures. |
| **`src/ChartNivra.Data/`** | 🟡 In Core | Thread-safe `StreamingRingBuffer` currently sits in `ChartNivra.Core.Data`. Can be split into standalone assembly. |
| **`src/ChartNivra.Rendering.Abstractions/`** | 🟢 Implemented | `IChartCanvas`, `ChartPen`, `ChartBrush`, `IChartRenderer`, `CartesianChartRenderer`, `ThreeDChartRenderer`. |
| **`src/ChartNivra.Rendering.Skia/`** | 🟢 Implemented | Hardware-accelerated `SkiaChartCanvas` implementation of `IChartCanvas`. |
| **`src/ChartNivra.Rendering.3D.Abstractions/`** | 🟡 In Core/Rendering | `Camera3D`, `Vector3D`, `Triangle3D`, `Light3D` implemented in Core/Abstractions. |
| **`src/ChartNivra.Rendering.3D.Backend/`** | ⏳ Planned | Full GPU shader pipeline / native cross-platform backend. |
| **`src/ChartNivra.Maui/`** | 🟢 Implemented | `ChartView` (SKCanvasView host control with bindable properties, theme awareness). |
| **`src/ChartNivra.Cartesian/`** | 🟡 In Core | `LineSeriesModel`, `BarSeriesModel`, `AreaSeriesModel`, `ScatterSeriesModel` currently inside Core. |
| **`src/ChartNivra.Pie/`** | 🟡 In Core | `PieChartModel`, `PieSliceModel` (supports Pie & Donut) inside Core. |
| **`src/ChartNivra.Gauges/`** | ⏳ Planned | Radial, Linear, Bullet gauges. |
| **`src/ChartNivra.Financial/`** | 🟡 In Core | `CandlestickSeriesModel`, `FinancialPoint`, `TechnicalIndicators` (SMA, EMA) in Core. |
| **`src/ChartNivra.Statistical/`** | ⏳ Planned | Box Plot, Histogram, Error Bars, Heatmaps. |
| **`src/ChartNivra.ThreeD/`** | 🟡 In Core | `Column3DMeshGenerator`, `ThreeDChartRenderer`. |
| **`src/ChartNivra.Annotations/`** | ⏳ Planned | Reference lines, threshold bands, text callouts. |
| **`src/ChartNivra.Themes/`** | 🟡 In Core | `ChartTheme` (Dark, Light), `ChartPalette` (Vibrant, Ocean) in Core. |
| **`src/ChartNivra.Export/`** | ⏳ Planned | Off-screen PNG/JPEG raster and SVG/PDF vector exporter. |

---

## 2. Tests & Samples Matrix

| Target Project / Folder | Status | Current Implementation Notes |
|---|---|---|
| **`tests/ChartNivra.Tests/`** | 🟡 In progress | One xUnit project organized into Unit, Rendering, Visual, Integration, Performance, and CrossPlatform folders. Device and end-to-end visual coverage remain planned. |
| **`samples/ChartNivra.Gallery/`** | 🟢 Implemented | .NET MAUI gallery containing chart, streaming, and 3D samples. |
| **`benchmarks/`** | ⏳ Planned | Large datasets benchmark suite (10k to 1M points). |
| **`docs/`** | 🟢 Started | Architecture & README created. |
| **`.github/`** | ⏳ Planned | CI/CD build workflows for .NET 10 MAUI. |

---

## 3. Recommended Next Implementation Steps

To reach the full modular target architecture:
1. **Module Extraction**: Extract specialized assemblies from Core (`ChartNivra.Cartesian`, `ChartNivra.Pie`, `ChartNivra.Financial`, `ChartNivra.ThreeD`, `ChartNivra.Data`).
2. **Additional Chart Families**: Implement Gauges, Statistical (Histogram, BoxPlot), and Annotations.
3. **Gallery Samples**: Expand financial, real-time, and 3D examples within `ChartNivra.Gallery`.
4. **CI/CD Pipeline**: Add GitHub Actions workflow for automated builds and testing on every pull request.
