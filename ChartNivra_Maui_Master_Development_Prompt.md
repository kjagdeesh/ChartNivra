# ChartNivra.Maui — Product Vision, Architecture and Master Development Prompt

**Recommended working name:** `ChartNivra.Maui`  
**NuGet package name:** `ChartNivra.Maui`  
**Suggested root namespace:** `ChartNivra`  
**Tagline:** *A modern, high-performance 2D and 3D charting framework for .NET MAUI.*

> Naming note: `ChartNivra.Maui` is a working recommendation, not a verified available name. Before committing, check NuGet.org, GitHub, domain availability and relevant trademarks. Do not use the name publicly until those checks are complete.

---

# Part 1 — Product direction

Build a completely new .NET MAUI charting framework. Use Microcharts only as a reference for understanding an existing lightweight SkiaSharp charting implementation. Do not copy its architecture or assume its limitations should define this project.

The goal is a modular, extensible, high-performance charting ecosystem that offers a strong free/open-source alternative to commercial charting products such as Telerik UI for .NET MAUI and Syncfusion Charts.

The library must support:

- A broad 2D chart collection.
- A carefully designed 3D chart collection with real depth and perspective.
- Professional themes, palettes, gradients and per-element styling.
- High-performance rendering and large datasets.
- Real-time streaming and incremental updates.
- Multiple series and multiple axes.
- Touch, mouse, keyboard and accessibility interactions.
- XAML, C#, MVVM and data binding.
- Animation, annotations, legends, tooltips and data labels.
- Export to image and vector/document formats where technically supported.
- Extensibility for custom series, renderers, effects and themes.
- Android, iOS, Mac Catalyst and Windows support, subject to platform capability testing.

The product must be engineered as a maintainable framework, not as a collection of unrelated drawing controls.

## Product principles

1. Performance and correctness before feature count.
2. Simple defaults; deep customization when needed.
3. Consistent APIs across chart families.
4. Rendering and calculations separated from MAUI UI concerns.
5. 2D and 3D share data, styling and interaction concepts where appropriate, but do not force incompatible rendering models into one implementation.
6. Cross-platform consistency with explicit documentation of platform differences.
7. No unverified performance claims.
8. Automated tests and benchmarks from the beginning.
9. Public APIs should be stable, documented and versioned carefully.
10. Optional features should not impose unnecessary dependencies on basic users.

---

# Part 2 — Critical 3D requirement

3D must mean actual three-dimensional geometry and depth, not merely gradients, shadows, perspective-looking 2D shapes or decorative extrusion.

Implement a distinct 3D rendering subsystem with:

- 3D coordinates and world space.
- Camera position, target, field of view and projection.
- Perspective and orthographic projection.
- Depth testing and depth buffering where supported.
- Lighting, materials and surface normals.
- Ambient, diffuse and specular lighting.
- Shadows where supported and performant.
- Rotation, orbit, pan and zoom camera controls.
- Depth sorting and occlusion handling.
- Picking/hit testing against 3D geometry.
- 3D labels and callouts with collision management.
- Responsive level-of-detail and geometry simplification.
- GPU acceleration where a stable, supported implementation is available.
- A tested software/fallback path for platforms where GPU capabilities differ.

## 3D technology strategy

Do not assume SkiaSharp alone is a complete 3D engine. SkiaSharp is suitable for the 2D renderer and may be useful for overlays, labels and some 2D projections, but true 3D should use a separate rendering abstraction.

Evaluate candidate approaches through small technical prototypes before selecting one:

- A supported GPU graphics API/backend suitable for the target platforms.
- A cross-platform 3D engine or rendering library whose licensing, maintenance, platform support and MAUI integration are acceptable.
- A software-rendered fallback for limited or unsupported scenarios, if feasible.

Do not hard-code the entire public API to one graphics backend. Define abstractions such as `I3DChartRenderer`, `IGraphicsDevice`, `ICamera`, `IMaterial`, `ILight`, `IGeometryBuffer` and `IHitTester`. Keep backend-specific implementation in separate projects.

Document platform-specific limitations honestly. Do not promise identical GPU features on every platform until validated on real devices.

## 3D chart types

Plan the following chart types. Prioritize the useful, well-defined types first:

### 3D Cartesian charts
- 3D Column Chart.
- 3D Bar Chart.
- 3D Stacked Column Chart.
- 3D Stacked Bar Chart.
- 3D Surface Chart.
- 3D Wireframe Surface.
- 3D Scatter Chart.
- 3D Bubble Chart.
- 3D Line Chart.
- 3D Area/Surface Chart.
- 3D Waterfall-style chart only if a meaningful 3D representation is defined.

### 3D statistical and scientific charts
- 3D Mesh/Surface Plot.
- 3D Contour Surface.
- 3D Density/Volume visualization where the data model supports it.
- 3D Point Cloud.
- 3D Vector Field, as an advanced module.

### 3D pie and decorative charts
- Exploded 3D Pie and Donut may be offered as optional presentation-oriented charts.
- Clearly distinguish these from analytical 3D plots; do not prioritize misleading 3D encodings over readable 2D charts.

## 3D interaction
- Orbit/rotate with one-finger or mouse drag.
- Pan camera.
- Pinch and wheel zoom.
- Reset camera.
- Preset camera angles.
- Select/highlight 3D data points and surfaces.
- Tooltip anchored to selected geometry.
- Axis and grid visibility toggles.
- Lighting/material presets.
- Camera animation.
- Selection and hit testing that remain accurate under projection.

---

# Part 3 — Complete feature catalogue

## A. 2D chart families

### Core charts
- Line, spline, step line.
- Area, spline area, step area.
- Column and bar.
- Grouped, stacked and 100% stacked bar/column.
- Pie and donut.
- Scatter and bubble.
- Range area and range column/bar.
- Combination charts with different series types.

### Advanced charts
- Radar and polar.
- Funnel and pyramid.
- Histogram.
- Box plot.
- Error bars.
- Waterfall.
- Pareto.
- Heatmap.
- Treemap.
- Sunburst.
- Calendar heatmap.
- Timeline.
- Sankey.
- Chord.
- Network/graph visualization, as a separate specialized module.

### Gauges
- Radial and linear gauges.
- Circular and arc progress.
- Speedometer.
- Bullet chart.
- Thermometer.
- Multiple needles, ranges, targets and thresholds.

### Financial charts
- Candlestick.
- OHLC.
- Volume.
- Heikin-Ashi.
- Renko, Kagi and Point & Figure only after the core financial model is stable.
- Technical indicators: SMA, EMA, RSI, MACD, Bollinger Bands, ATR, Stochastic, VWAP and OBV.
- Multiple panes, synchronized time axes and price scales.

## B. Axes and coordinate systems

Support:
- Category, numeric, date-time, date-time category, logarithmic, percentage and time-span axes.
- Multiple axes and series-to-axis assignment.
- Automatic and explicit minimum/maximum.
- Smart interval calculation and nice-number ticks.
- Major/minor ticks and gridlines.
- Axis titles, opposed axes and crossing.
- Inversion and custom ranges.
- Label format strings, culture-aware formatting and custom formatters.
- Rotation, alignment, trimming, wrapping and collision avoidance.
- Breaks only where the semantics and user experience are clearly defined.
- Zoomable and pannable visible ranges.

## C. Series and data model

Support:
- Multiple series per chart.
- Different series types in a combination chart.
- Per-series axes.
- Strongly typed data binding where possible.
- X/Y/Z/value/size/color bindings as relevant to chart type.
- `IEnumerable<T>`, lists and observable collections.
- Collection and property change notifications.
- Dynamic add, remove, replace and append.
- Sorting, filtering, grouping and transformations through optional data-processing APIs.
- Per-point styles and conditional coloring.
- Null/missing-value handling.
- Empty-state handling.
- Custom series extension points.

Do not force every chart into a Cartesian series hierarchy. Use shared contracts for common concepts and specialized models where chart semantics differ.

## D. Interaction

- Tap/click selection.
- Long-press selection on touch.
- Hover on desktop.
- Keyboard navigation.
- Single and multi-selection.
- Tooltips and shared tooltips.
- Trackball and crosshair.
- Pinch zoom, wheel zoom, drag pan and selection zoom.
- Zoom reset and zoom history.
- Scrollbar/range navigator.
- 3D orbit, pan, zoom and picking.
- Commands/events and bindable selection state.
- Gesture arbitration that does not break parent scrolling.

## E. Tooltips, legends and labels

### Tooltips
- Custom content/templates.
- Shared multi-series tooltip.
- Smart placement and clipping avoidance.
- Formatting, borders, backgrounds, shadows and animation.
- Pinning and selection persistence.

### Legends
- Top, bottom, left, right and custom placement.
- Horizontal, vertical and scrollable layouts.
- Interactive series visibility.
- Custom symbols, templates and grouping.

### Data labels
- Inside/outside/center placement.
- Custom formatting and templates.
- Connector lines.
- Collision avoidance and smart visibility.
- Per-point and per-series configuration.

## F. Theme, colors and styling system — mandatory

Theme and coloring must be first-class architecture features, not a collection of hard-coded defaults.

### Built-in themes
- Light.
- Dark.
- System-following.
- High Contrast.
- Material-inspired.
- Fluent-inspired.
- Minimal.
- Corporate.
- A neutral default theme.

Avoid copying proprietary theme assets or branding. Theme names should describe visual styles, not imply affiliation.

### Theme system
Provide:
- Global application theme.
- Per-chart theme override.
- Per-series overrides.
- Per-element overrides.
- Runtime theme switching.
- Theme inheritance and fallback.
- Theme resources/tokens.
- Light/dark variants.
- High-contrast mode.
- Theme serialization if useful.
- A documented custom-theme API.

### Color system
Support:
- Solid colors.
- Palette-based automatic series colors.
- Custom palettes.
- Sequential, diverging and categorical palettes.
- Linear and radial gradients where appropriate.
- Opacity/transparency.
- Per-point coloring.
- Conditional coloring based on values.
- Color mapping for heatmaps and surfaces.
- Color scales with legends.
- Color interpolation.
- Accessible palettes and contrast checks.
- Color-blind-friendly palette options.
- Consistent color assignment when series are added or removed.

### Styling targets
Every relevant chart element should be styleable:
- Chart background and plot background.
- Plot border and corner radius.
- Series fill and stroke.
- Line width, dash, caps and joins.
- Markers and marker shapes.
- Axes, labels, ticks and gridlines.
- Legends and symbols.
- Tooltips.
- Data labels.
- Annotations.
- 3D materials, lights, surface colors and background.
- Selection, hover and focus states.

Provide a centralized theme object and allow local overrides without forcing users to restyle every chart.

## G. Annotations
- Horizontal/vertical reference lines.
- Plot bands.
- Text, rectangle, circle and arrow annotations.
- Image annotations.
- Data-coordinate and pixel-coordinate positioning.
- Draggable annotations where appropriate.
- Financial event/price annotations.
- 3D world-coordinate annotations where technically feasible.

## H. Animation
- Initial chart reveal.
- Line draw.
- Bar/column growth.
- Area reveal.
- Pie/donut sweep.
- Point appearance.
- Data transitions.
- Camera/orbit animation for 3D.
- Easing functions.
- Cancellation and lifecycle handling.
- Reduced-motion accessibility preference.
- Avoid background animation when the chart is not visible.

## I. Real-time and large-data support
- Batched incoming updates.
- Append, replace and remove operations.
- Ring buffers and rolling windows.
- Time-window filtering.
- Thread-safe ingestion queues.
- Backpressure policies.
- Incremental range and geometry updates.
- Viewport-based rendering.
- Min-Max and LTTB decimation for suitable series.
- Pixel-column aggregation.
- Level-of-detail rendering.
- Specialized aggregation for financial OHLC data.
- Spatial indexes for dense scatter plots.
- Bounded memory usage.

Decimation must preserve the meaning of the chart. Do not apply one algorithm indiscriminately to all chart types.

## J. Export and printing
- PNG and JPEG raster export.
- SVG vector export where the chart primitives can be represented faithfully.
- PDF export through an appropriate PDF backend.
- Configurable dimensions and resolution.
- Transparent background.
- Optional legend, labels and annotations.
- Export selected series or the complete chart.
- CSV/JSON data export as separate data features.

Do not claim an export format is vector-based if it merely embeds a raster screenshot.

## K. Accessibility and localization
- Screen reader chart summaries.
- Accessible titles and data-point descriptions.
- Keyboard focus and navigation.
- High-contrast palettes.
- Reduced-motion support.
- Culture-aware number/date/currency formatting.
- RTL layout.
- Unicode and font fallback.
- Scalable text and accessibility font sizes.

## L. Dashboard features
- Multiple charts in a dashboard.
- Shared filters and date ranges.
- Synchronized crosshair and zoom.
- Linked selections.
- Shared data sources.
- Responsive layouts.
- Dashboard export.
- Avoid coupling the core chart engine to a specific dashboard layout framework.

---

# Part 4 — Architecture and solution structure

Use a modular solution. The following is a starting proposal; refine it after architecture spikes.

```text
ChartNivra/
├── src/
│   ├── ChartNivra.Core/
│   ├── ChartNivra.Data/
│   ├── ChartNivra.Rendering.Abstractions/
│   ├── ChartNivra.Rendering.Skia/
│   ├── ChartNivra.Rendering.3D.Abstractions/
│   ├── ChartNivra.Rendering.3D.Backend/
│   ├── ChartNivra.Maui/
│   ├── ChartNivra.Cartesian/
│   ├── ChartNivra.Pie/
│   ├── ChartNivra.Gauges/
│   ├── ChartNivra.Financial/
│   ├── ChartNivra.Statistical/
│   ├── ChartNivra.ThreeD/
│   ├── ChartNivra.Annotations/
│   ├── ChartNivra.Themes/
│   └── ChartNivra.Export/
├── tests/
│   └── ChartNivra.Tests/                 # Unit, Rendering, Visual, Integration, Performance, CrossPlatform folders
├── samples/
│   └── ChartNivra.Gallery/              # All sample pages and demos
├── benchmarks/
├── docs/
├── .github/
├── Directory.Build.props
├── Directory.Packages.props
└── ChartNivra.sln
```

## Architectural boundaries

### Core
Owns chart models, data contracts, calculations, coordinates, axis algorithms, layout models and shared abstractions. It must not depend on MAUI controls or a specific graphics backend.

### Rendering abstractions
Owns renderer contracts, render context, geometry contracts, clipping, text measurement contracts and invalidation concepts.

### 2D renderer
Use SkiaSharp as an implementation choice behind the rendering abstraction. Reuse resources carefully and dispose native resources deterministically.

### 3D renderer
Owns camera, projection, geometry, materials, lights, depth, picking and backend integration. Do not pretend the 3D renderer is just another SkiaSharp series renderer.

### MAUI integration
Owns controls, bindable properties, handlers, gestures, lifecycle, accessibility and platform integration.

### Chart modules
Own specialized series, chart layout and chart-specific rendering logic without duplicating the common engine.

### Themes
Own theme tokens, palettes, color scales, built-in themes and theme resolution.

### Export
Own off-screen rendering and output encoders. It should not require a chart to be visible on screen.

---

# Part 5 — Public API design requirements

Design an API that is easy for a beginner but extensible for advanced users.

The following is illustrative only. Validate naming and MAUI binding behavior before committing to the API.

## XAML example

```xml
<charts:CartesianChart
    Title="Monthly Revenue"
    Theme="Dark"
    Palette="Ocean"
    IsLegendVisible="True">

    <charts:CartesianChart.XAxes>
        <charts:DateTimeAxis />
    </charts:CartesianChart.XAxes>

    <charts:CartesianChart.YAxes>
        <charts:NumericAxis
            Minimum="0"
            LabelFormat="C0" />
    </charts:CartesianChart.YAxes>

    <charts:CartesianChart.Series>
        <charts:LineSeries
            Label="Revenue"
            ItemsSource="{Binding Sales}"
            XBinding="Date"
            YBinding="Revenue"
            ShowMarkers="True" />
    </charts:CartesianChart.Series>
</charts:CartesianChart>
```

## 3D XAML example

```xml
<charts:ThreeDChart
    Title="Regional Performance"
    Theme="Dark"
    Palette="Vibrant"
    CameraPreset="Perspective"
    IsRotationEnabled="True"
    IsZoomEnabled="True">

    <charts:ThreeDChart.Series>
        <charts:Column3DSeries
            ItemsSource="{Binding RegionalSales}"
            XBinding="Region"
            YBinding="Quarter"
            ZBinding="Revenue"
            ColorBinding="Category" />
    </charts:ThreeDChart.Series>
</charts:ThreeDChart>
```

This example illustrates the intended developer experience. Refine the data dimensions and series model so they are mathematically meaningful for each chart type.

## C# example

```csharp
var chart = new CartesianChart
{
    Title = "Monthly Revenue",
    Theme = ChartTheme.Dark,
    Palette = ChartPalette.Ocean
};

chart.Series.Add(new LineSeries
{
    Label = "Revenue",
    ItemsSource = sales,
    XBinding = "Date",
    YBinding = "Revenue",
    ShowMarkers = true
});
```

## API requirements
- Use MAUI bindable properties for bindable control properties.
- Support XAML collection syntax.
- Support C# object initialization.
- Provide useful defaults.
- Avoid excessive boolean properties when a configuration object or enum is clearer.
- Use consistent naming and nullability annotations.
- Provide events and commands for interaction.
- Avoid exposing backend-specific objects in the core public API.
- Document threading rules.
- Define disposal and lifecycle behavior.
- Keep breaking changes controlled and documented.

---

# Part 6 — Performance engineering requirements

Performance must be measured continuously.

## Rendering optimization
- Reuse paints, paths and buffers where safe.
- Cache text measurements and static geometry.
- Separate data processing, layout and drawing.
- Track invalidation by layer or region.
- Avoid full geometry rebuilds for tooltip-only changes.
- Avoid unnecessary LINQ and allocations in hot paths.
- Use pooled buffers only where benchmarks justify them.
- Dispose native graphics resources deterministically.
- Do not access UI-bound collections unsafely from background threads.

## Rendering layers
Consider separate invalidation/render layers:
1. Background.
2. Plot area and grid.
3. Axes.
4. Series.
5. Annotations.
6. Interaction overlay.
7. Tooltip/selection overlay.

## Benchmark workloads
Test at minimum:
- 100 points.
- 1,000 points.
- 10,000 points.
- 100,000 points.
- 1,000,000 points where the chart type and device make it meaningful.
- Continuous streaming.
- Multiple simultaneous series.
- Repeated zoom and pan.
- 3D geometry with increasing vertex/triangle counts.

Record:
- Initial render latency.
- Frame-time distribution, not just average FPS.
- CPU use.
- Managed allocations.
- Managed and native memory.
- Data update throughput.
- Geometry rebuild time.
- Interaction latency.
- Export time.

Use BenchmarkDotNet for isolated .NET calculations and a real-device benchmark app for end-to-end MAUI rendering. Compare equivalent workloads and disclose test devices, versions and configurations.

---

# Part 7 — Testing and quality requirements

## Unit tests
Test axis ranges, tick generation, transforms, data mapping, formatting, decimation, statistical calculations, camera projection, 3D picking and layout.

## Rendering tests
Use deterministic inputs and screenshot/golden-image comparisons. Account for platform font and antialiasing differences.

## Cross-platform tests
Validate Android, iOS, Mac Catalyst and Windows on supported SDK/runtime combinations.

## Lifecycle tests
Test repeated navigation, control disposal, source replacement, animation cancellation, background/foreground transitions and native resource cleanup.

## Accessibility tests
Validate screen reader output, keyboard access, contrast and reduced-motion behavior.

## Release gates
Do not publish a stable release if:
- Critical chart types have unresolved correctness defects.
- Native resource leaks are known.
- Public APIs are undocumented.
- Supported platforms have not been tested.
- Performance regressions have no explanation.
- Package dependencies or licenses are unclear.

---

# Part 8 — NuGet packaging strategy

Keep the development solution modular, but do not create an excessive number of public packages at the beginning.

Suggested initial packages:
- `ChartNivra.Maui` — main package and common chart controls.
- `ChartNivra.ThreeD` — optional 3D charts and required backend integration.
- `ChartNivra.Financial` — optional financial charts.
- `ChartNivra.Export` — optional export functionality.
- `ChartNivra.Themes` — optional built-in theme collection, if it has meaningful independent dependencies.

Keep internal projects separate where useful. Publish additional packages only when their APIs and dependency boundaries are stable.

Package requirements:
- Correct MAUI workload/platform targeting.
- XML documentation files.
- Nullable reference types.
- Semantic versioning.
- Package license metadata.
- Repository and source-link metadata.
- Symbols packages where appropriate.
- Dependency and license audits.
- Clear installation and platform support documentation.

Do not multi-target unsupported frameworks just to make the package list look larger. Select target frameworks based on the current supported .NET MAUI release and test matrix.

---

# Part 9 — Phased implementation roadmap

Do not implement every feature in one pass. Build vertical slices and keep the repository compiling.

## Phase 0: Discovery and technical spikes
- Inspect Microcharts and other reference projects.
- Review Telerik and Syncfusion feature behavior.
- Check third-party licenses.
- Prototype MAUI + SkiaSharp 2D rendering.
- Prototype the proposed 3D backend on every target platform.
- Test GPU/device lifecycle and fallback behavior.
- Decide the initial supported platform matrix.
- Finalize architecture decision records.

**Exit gate:** A proven 2D path and a technically credible 3D path, with platform limitations documented.

## Phase 1: Core foundation
- Solution and build setup.
- Core data models.
- Axis and coordinate system.
- Layout engine.
- Rendering abstractions.
- Theme tokens and palette model.
- MAUI host control.
- Unit tests and first benchmark harness.

**Exit gate:** A basic chart renders on all supported platforms.

## Phase 2: MVP 2D charts
- Line, spline, bar, column, area, pie, donut and scatter.
- Multiple series.
- Numeric, category and date-time axes.
- Data binding.
- Legends, labels and tooltips.
- Light/dark themes and custom palettes.

**Exit gate:** A useful dashboard can be built using XAML and C#.

## Phase 3: Interaction and animation
- Selection.
- Trackball/crosshair.
- Zoom and pan.
- Touch and mouse input.
- Animation engine.
- Reduced-motion behavior.
- Interaction tests.

**Exit gate:** Interactions work consistently across supported devices.

## Phase 4: Real-time and performance
- Batched updates.
- Incremental geometry.
- Viewport filtering.
- Decimation.
- Caching.
- Memory profiling.
- Performance regression suite.

**Exit gate:** Published benchmark report meets defined targets on named devices.

## Phase 5: Advanced 2D
- Radar, polar, heatmap, histogram, box plot, funnel, waterfall and range charts.
- Annotations.
- Advanced axis options.
- Custom series and renderers.
- Accessibility and localization.

## Phase 6: 3D foundation and charts
- 3D backend abstraction.
- Camera and projection.
- Geometry buffers.
- Materials and lights.
- Depth testing.
- 3D axes and grids.
- Picking and selection.
- 3D column, surface, scatter and bubble charts.
- Orbit/pan/zoom controls.
- 3D labels and tooltips.
- Device-level performance tests.

**Exit gate:** 3D works on the explicitly supported platform matrix, with documented fallback and hardware requirements.

## Phase 7: Financial, gauges and specialized modules
- Financial series and indicators.
- Gauges.
- Advanced statistical charts.
- Additional 3D chart types.
- Dashboard synchronization.

## Phase 8: Production release
- API review.
- Cross-platform regression testing.
- Visual tests.
- Accessibility review.
- Export.
- Documentation and samples.
- NuGet packaging.
- License audit.
- CI/CD.
- Release notes and migration guidance.

---

# Part 10 — MVP definition

The first stable release must be focused. Recommended 1.0 scope:

### Include
- Line, spline, bar, column, area, pie, donut and scatter.
- Multiple series.
- Numeric, category and date-time axes.
- XAML and C# APIs.
- MVVM/data binding.
- Light, dark and system themes.
- Custom palettes and per-series colors.
- Legends, data labels and tooltips.
- Selection.
- Basic zoom and pan.
- Basic animations.
- Real-time append with batching.
- PNG export.
- Automated tests and published benchmarks.

### 3D release policy
Do not claim full 3D support in 1.0 unless the 3D backend has passed the platform validation gate. It is better to release a reliable 2D 1.0 and a clearly labelled 3D preview than to advertise 3D features that work on only one device or platform.

---

# Part 11 — Project risks and decisions

## Major risks
1. Scope is too large for one developer.
2. 3D backend support differs significantly across platforms.
3. Rendering abstractions become over-engineered.
4. Public API changes break early adopters.
5. Large datasets expose memory and latency problems.
6. 3D rendering increases package size and maintenance cost.
7. Accessibility is overlooked by custom drawing.
8. Feature count grows faster than test coverage.

## Required mitigations
- Build prototypes before committing to a 3D technology.
- Keep 3D optional.
- Make architecture decisions explicit and revisitable.
- Maintain a public roadmap.
- Benchmark before optimizing.
- Build documentation with each feature.
- Prefer a smaller stable feature set over a large incomplete one.
- Track known limitations by platform.

---

# Part 12 — Master prompt for an AI coding agent

Copy the prompt below into your coding agent after creating the repository. Use it as the project's governing specification. Do not expect an agent to implement the entire framework correctly in one response; require it to work milestone by milestone, preserve buildable commits and ask for decisions when a technology choice needs evidence.

---

## MASTER PROMPT — ChartNivra.MAUI

You are the principal software architect and senior .NET graphics engineer responsible for designing and implementing **ChartNivra.Maui**, a new, modular, high-performance 2D and 3D charting framework for .NET MAUI.

Your task is to build a production-quality NuGet ecosystem that can compete with established commercial charting products in selected capabilities while remaining maintainable, extensible, well-documented and suitable for open-source development.

### 1. Mission

Create a completely new implementation. Treat Microcharts, LiveCharts2, Telerik MAUI Charts and Syncfusion Charts as research references for capabilities and user experience only.

Do not copy proprietary source code, assets, documentation or implementation details. Respect all third-party licenses. Do not make unsupported claims about performance or feature parity.

The framework must provide:
- Extensive 2D charts.
- True 3D charts with real geometry, depth, perspective, lighting and camera interaction.
- A professional theme and color system.
- Large-data and real-time performance.
- Multiple series and axes.
- Tooltips, trackball, crosshair, zoom and pan.
- Animations, annotations, legends and data labels.
- XAML, C#, MVVM and data binding.
- Export, accessibility and localization.
- Custom series, renderers, palettes and themes.

### 2. Mandatory engineering rules

1. Do not attempt to implement the whole product in one step.
2. Work in small, coherent milestones.
3. Keep the solution compiling after every milestone.
4. Write tests with each feature.
5. Add XML documentation for public APIs.
6. Use nullable reference types and consistent naming.
7. Avoid unnecessary dependencies.
8. Do not place all logic in one MAUI control.
9. Do not mix data calculations, layout, interaction and drawing.
10. Do not hard-code colors or theme assumptions.
11. Do not expose SkiaSharp or a 3D backend throughout the public API.
12. Do not claim 3D support using fake extrusion or 2D effects.
13. Do not claim performance superiority without reproducible benchmarks.
14. Do not silently ignore unsupported platforms or graphics capabilities.
15. Never leave placeholder methods presented as completed functionality.
16. Explain trade-offs and record important architecture decisions.
17. Prefer correctness, maintainability and tested behavior over feature count.
18. Do not introduce breaking public API changes casually.
19. Do not invent platform support; validate it.
20. If a requirement is ambiguous or a backend decision is uncertain, present options and a recommendation before committing to the implementation.

### 3. First task: inspect and plan

Before writing substantial implementation code:

- Inspect the repository and existing build configuration.
- Identify installed .NET SDKs and MAUI workloads.
- Confirm the supported target frameworks and platforms.
- Inspect reference projects and their licenses.
- Create `README.md`, `ROADMAP.md`, `ARCHITECTURE.md`, `FEATURES.md` and `DECISIONS/`.
- Propose the solution structure and package boundaries.
- Identify risks and unresolved technical decisions.
- Create an implementation backlog ordered by dependencies.
- Create a platform compatibility matrix.

Do not assume a specific .NET version is installed. Inspect the environment and select a supported target framework deliberately.

### 4. Architecture

Create separate modules for:

- Core chart models and contracts.
- Data binding and processing.
- Rendering abstractions.
- SkiaSharp 2D renderer.
- 3D rendering abstractions.
- A validated 3D rendering backend.
- MAUI controls and lifecycle integration.
- Cartesian charts.
- Pie and circular charts.
- Gauges.
- Financial charts.
- Statistical charts.
- 3D charts.
- Themes and palettes.
- Annotations.
- Export.

Keep Core independent of MAUI and graphics implementations wherever practical.

Define clean contracts for:
- Chart and series models.
- Axes and coordinate transforms.
- Layout and render context.
- Renderer and canvas.
- Data sources and update notifications.
- Interaction and selection.
- Theme and palette resolution.
- Annotations.
- Geometry caching.
- 3D camera, projection, materials, lights and hit testing.

Use dependency inversion. Avoid circular project references.

### 5. 2D rendering

Use SkiaSharp behind a rendering abstraction for the initial 2D implementation.

Implement:
- Device-independent chart coordinates.
- Plot-area measurement and layout.
- Clipping.
- Text measurement and rendering.
- Paint and geometry reuse.
- Layered rendering and invalidation.
- High-DPI behavior.
- Deterministic resource disposal.
- Off-screen rendering for export.

Ensure render code is deterministic and testable where possible.

### 6. True 3D rendering

Treat 3D as a separate rendering subsystem.

First conduct technical spikes to select a cross-platform backend. Evaluate platform support, licensing, project activity, maintenance, GPU APIs, MAUI integration, packaging and fallback behavior.

Implement:
- 3D world coordinates.
- Perspective and orthographic cameras.
- Camera orbit, pan and zoom.
- Projection and view transforms.
- Geometry buffers.
- Depth testing and occlusion.
- Lighting and material models.
- Surface normals.
- Shadows where feasible.
- Picking/hit testing.
- 3D axes, grids and labels.
- Geometry caching and level-of-detail.
- Device lifecycle and resource cleanup.

Implement 3D column, surface, scatter and bubble charts first. Add other chart types only after these foundations are stable.

Do not describe a 2D chart with extruded shapes as a true 3D analytical chart. If a platform cannot support a backend feature, document it and provide a deliberate fallback or mark the feature unsupported.

### 7. Theme, palette and color architecture

This is a mandatory core feature.

Create a theme system with:
- Light, dark, system and high-contrast themes.
- Built-in professional themes.
- Custom theme creation.
- Global and per-chart theme selection.
- Runtime theme switching.
- Theme inheritance and local overrides.
- Centralized design tokens.
- Categorical, sequential and diverging palettes.
- Custom palettes.
- Gradients and opacity.
- Per-series and per-point colors.
- Conditional coloring.
- Heatmap and 3D surface color scales.
- Accessible and color-blind-friendly palettes.
- Consistent series color assignment.

All relevant visual properties must be configurable. Avoid hard-coded color values in renderers. Define theme resolution and fallback rules clearly.

### 8. Chart types

Implement in dependency order.

First:
- Line, spline and step line.
- Bar and column.
- Area and stacked area.
- Pie and donut.
- Scatter and bubble.

Then:
- Combination charts.
- Radar and polar.
- Histogram and box plot.
- Heatmap.
- Funnel and waterfall.
- Range charts.
- Gauges.
- Financial charts and indicators.
- Specialized diagrams.

3D:
- 3D column/bar.
- 3D surface/wireframe.
- 3D scatter/bubble.
- Additional scientific and presentation-oriented 3D charts where meaningful.

Do not force specialized chart semantics into an inappropriate generic base class.

### 9. Axes and series

Implement:
- Numeric, category, date-time, date-time category, logarithmic, percentage and time-span axes.
- Automatic range and interval calculation.
- Nice-number tick generation.
- Major/minor ticks and grids.
- Formatting and culture support.
- Multiple axes.
- Axis inversion and custom ranges.
- Multiple series and combination charts.
- Strongly typed data binding where feasible.
- Null and missing-data behavior.
- Per-series and per-point styling.
- Custom series extension points.

### 10. Interaction

Implement:
- Tap/click and long press.
- Desktop hover.
- Selection and multi-selection.
- Tooltip and shared tooltip.
- Trackball and crosshair.
- Pinch and wheel zoom.
- Pan and selection zoom.
- Reset and zoom history.
- Keyboard navigation.
- Commands and bindable interaction state.
- 3D camera orbit, pan, zoom and picking.

Ensure gestures cooperate with MAUI parent scrolling and navigation.

### 11. Data and performance

Support observable sources and incremental updates.

Implement:
- Batched update queues.
- Append, replace and remove operations.
- Rolling windows.
- Thread-safe ingestion.
- Backpressure.
- Incremental geometry.
- Viewport filtering.
- Appropriate decimation.
- Geometry and text caching.
- Memory-conscious buffers.
- Background calculations with safe UI synchronization.

Build a benchmark application. Measure initial render, frame-time distribution, CPU, memory, allocations, update throughput, zoom/pan latency and export time.

Test representative datasets from 100 points through 1,000,000 points where meaningful. Do not claim a target is met until it has been measured on named devices.

### 12. Animation and annotations

Implement configurable animations with easing, cancellation, lifecycle awareness and reduced-motion support.

Implement horizontal/vertical lines, plot bands, text, rectangles, circles, arrows, image annotations and data-coordinate positioning. Add 3D annotations only where coordinate semantics and hit testing are clear.

### 13. MAUI developer experience

Provide:
- XAML collection syntax.
- C# initialization.
- Bindable properties.
- MVVM support.
- Observable data support.
- Commands and events.
- Consistent naming.
- Useful defaults.
- Theme and palette configuration.
- Samples for every major feature.
- Clear threading and lifecycle documentation.

Keep backend-specific types out of ordinary application code.

### 14. Export, accessibility and localization

Implement raster export first, then vector/document export with honest format guarantees.

Support:
- PNG/JPEG.
- SVG when faithfully representable.
- PDF through an appropriate backend.
- Configurable size and resolution.
- Screen reader summaries.
- Accessible data point descriptions.
- Keyboard navigation.
- High contrast.
- Reduced motion.
- Culture-aware formatting.
- RTL layouts and Unicode text.

### 15. Testing and CI

Create:
- Unit tests.
- Rendering tests.
- Visual regression tests.
- Integration tests.
- Cross-platform tests.
- Performance tests.
- Lifecycle and memory tests.

Set up CI to build all supported target frameworks, run tests, validate package metadata and detect dependency/license issues.

Do not release a stable version with known critical rendering defects, native resource leaks or undocumented public APIs.

### 16. NuGet packaging

Start with a small number of stable packages:
- `ChartNivra.Maui`.
- `ChartNivra.ThreeD`.
- `ChartNivra.Financial` when ready.
- `ChartNivra.Export` when ready.

Use semantic versioning, XML documentation, repository metadata, license metadata, symbols and dependency audits.

Do not split every internal project into a public NuGet package without a real consumer benefit.

### 17. Documentation and samples

Create a documentation site or structured documentation covering:
- Installation.
- Quick start.
- XAML and C#.
- Every chart type.
- Axes and series.
- Themes and palettes.
- Interactions.
- Real-time updates.
- Large datasets.
- 3D camera and lighting.
- Export.
- Accessibility.
- Custom series.
- Troubleshooting.
- Platform limitations.
- Migration and versioning.

Build a demo application with a chart gallery, theme switcher, data-size controls, real-time examples, financial examples and 3D examples.

### 18. Required delivery workflow

For every milestone:

1. State the objective and acceptance criteria.
2. Inspect existing code before editing.
3. Propose the files and projects to change.
4. Implement a small coherent slice.
5. Build all relevant target frameworks.
6. Run tests.
7. Fix regressions.
8. Update documentation and samples.
9. Report what is complete, what is not complete and known limitations.
10. Identify the next dependency-safe milestone.

Never claim a feature is complete merely because its public class or property exists. A feature is complete only when its behavior is implemented, tested, documented and demonstrated.

### 19. First implementation milestone

Begin with architecture and a minimal working vertical slice:

1. Create the solution and project structure.
2. Configure target frameworks based on the installed SDK and supported MAUI release.
3. Create Core chart contracts.
4. Create rendering abstractions.
5. Implement the SkiaSharp 2D backend.
6. Create a MAUI chart host control.
7. Implement plot layout and coordinate transforms.
8. Implement a numeric axis.
9. Implement a basic line series.
10. Render the chart on each supported platform.
11. Add unit and rendering tests.
12. Add a minimal sample app.
13. Add a basic theme model with light and dark themes.
14. Add a benchmark baseline.
15. Document architecture decisions.

Do not start implementing every chart type until this vertical slice is stable.

### 20. Final quality standard

The finished product must feel like a coherent charting framework rather than a set of demos.

Prioritize:
- Correctness.
- Stable public APIs.
- True cross-platform behavior.
- Measurable performance.
- Real 3D depth where advertised.
- Comprehensive theme and color customization.
- Extensibility.
- Accessibility.
- Documentation.
- Maintainability.

Start by presenting the architecture proposal, unresolved technical decisions (especially the 3D backend), initial project tree, milestone backlog and acceptance criteria. Then implement only the first milestone.

---

# Part 13 — Recommended first decisions

Before coding, make these decisions explicitly:

| Decision | Recommendation |
|---|---|
| Working product name | ChartNivra.Maui |
| Core 2D renderer | SkiaSharp behind abstractions |
| 3D renderer | Separate backend; choose after platform prototypes |
| Theme model | Central tokens, palettes and local overrides |
| First release | High-quality 2D MVP |
| 3D release | Optional preview until platform-tested |
| Package strategy | Few public packages, modular internal projects |
| License | MIT is a reasonable option if all included dependencies and assets are compatible |
| Development style | Milestone-based, test-first where practical |
| Benchmark policy | Publish reproducible results, not unsupported claims |

## Final direction

Build the foundation first: rendering abstractions, data model, axes, coordinate transforms, theme resolution and MAUI lifecycle integration. Prove the 3D technology early with a small prototype because true 3D is a separate engineering challenge, not a cosmetic extension of a 2D chart.

A reliable, polished set of charts with excellent themes and measured performance is a stronger starting product than a huge feature list that has not been validated.
