# ChartNivra Architecture Documentation

ChartNivra is an enterprise-grade, high-performance .NET MAUI charting framework engineered for mission-critical industrial, medical, and financial visualization.

## Layered Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                 Application & Demo Layer                    │
│   ChartNivra.Demo, DashboardDemo, RealTimeDemo, Financial   │
├─────────────────────────────────────────────────────────────┤
│                    UI & Control Layer                       │
│    ChartNivra.Maui (SKCanvasView, Handlers, Visual States)  │
├─────────────────────────────────────────────────────────────┤
│                    Specialized Domains                      │
│ Cartesian | Pie | Gauges | Financial | Statistical | ThreeD │
├─────────────────────────────────────────────────────────────┤
│             Rendering Engine (Hardware/Software)            │
│  ChartNivra.Rendering.Skia | ChartNivra.Rendering.3D.Backend│
├─────────────────────────────────────────────────────────────┤
│                   Rendering Abstractions                    │
│      IChartCanvas, ChartPen, ChartBrush, Camera3D           │
├─────────────────────────────────────────────────────────────┤
│                     Data & Pipelines                        │
│   ChartNivra.Data (RingBuffer, LTTB, StreamingPipeline)     │
├─────────────────────────────────────────────────────────────┤
│                        Core Primitives                      │
│ ChartNivra.Core (Vector3D, NiceScale, Primitives, Themes)   │
└─────────────────────────────────────────────────────────────┘
```

## Key Architectural Principles

1. **Zero-Allocation Rendering**: Render loops operate on preallocated memory buffers and primitives (`ChartPoint`, `Vector3D`, `FinancialPoint`) to eliminate GC pressure at 60+ FPS.
2. **Painter's Algorithm 3D Projection**: Software rasterizer computes perspective camera projection matrices, depth sorting by triangle centroid, and Lambertian diffuse shading without requiring external GPU shader dependencies.
3. **LTTB Decimation**: Largest-Triangle-Three-Buckets algorithm decimates datasets with 100,000+ points down to viewport resolution while strictly preserving visual peaks and troughs.
4. **Decoupled Skia Backend**: Drawing routines interface exclusively via `IChartCanvas`, allowing headless unit and regression testing without graphics device drivers.
