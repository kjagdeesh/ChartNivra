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
| **`tests/ChartNivra.UnitTests/`** | 🟢 Implemented | 9 passing unit tests (Math, Decimation, Transforms, 3D Camera Projection, Technical Indicators, Ring Buffer, 3D Cuboid & Shading). |
| **`tests/ChartNivra.RenderingTests/`** | ⏳ Planned | Deterministic visual screenshot/golden comparison tests. |
| **`tests/ChartNivra.VisualTests/`** | ⏳ Planned | End-to-end MAUI UI visual tests. |
| **`tests/ChartNivra.IntegrationTests/`** | ⏳ Planned | Integration testing between handlers and controls. |
| **`tests/ChartNivra.PerformanceTests/`** | ⏳ Planned | BenchmarkDotNet throughput and memory tests. |
| **`tests/ChartNivra.CrossPlatformTests/`** | ⏳ Planned | Device testing matrix. |
| **`samples/ChartNivra.Demo/`** | 🟢 Implemented | .NET MAUI interactive gallery showcasing multi-series charting with dark theme. |
| **`samples/ChartNivra.DashboardDemo/`** | ⏳ Planned | Multi-chart executive KPI dashboard. |
| **`samples/ChartNivra.RealTimeDemo/`** | ⏳ Planned | 60 FPS streaming sensor/market data demo. |
| **`samples/ChartNivra.FinancialDemo/`** | ⏳ Planned | Candlestick + SMA/EMA indicator demo. |
| **`samples/ChartNivra.ThreeDDemo/`** | ⏳ Planned | Interactive 3D orbit camera and 3D column/mesh demo. |
| **`benchmarks/`** | ⏳ Planned | Large datasets benchmark suite (10k to 1M points). |
| **`docs/`** | 🟢 Started | Architecture & README created. |
| **`.github/`** | ⏳ Planned | CI/CD build workflows for .NET 10 MAUI. |

---

## 3. Recommended Next Implementation Steps

To reach the full modular target architecture:
1. **Module Extraction**: Extract specialized assemblies from Core (`ChartNivra.Cartesian`, `ChartNivra.Pie`, `ChartNivra.Financial`, `ChartNivra.ThreeD`, `ChartNivra.Data`).
2. **Additional Chart Families**: Implement Gauges, Statistical (Histogram, BoxPlot), and Annotations.
3. **Dedicated Demos**: Create `ChartNivra.FinancialDemo`, `ChartNivra.RealTimeDemo`, and `ChartNivra.ThreeDDemo`.
4. **CI/CD Pipeline**: Add GitHub Actions workflow for automated builds and testing on every pull request.
