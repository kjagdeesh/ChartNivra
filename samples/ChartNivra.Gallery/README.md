# ChartNivra Gallery

The Gallery is the development sample app for ChartNivra. It references the library project directly and provides a dashboard of chart samples and interactive demos.

The dashboard currently lists 23 chart types and five demo cards. Sample pages show a chart and its displayed C#/XAML examples where available; the streaming and 3D studio pages are implemented in C#. The app includes light and dark appearance options and responsive desktop/mobile layouts.

## Run

Set `ChartNivra.Gallery` as the startup project in Visual Studio and choose a target, or run from the repository root:

```sh
dotnet build samples/ChartNivra.Gallery/ChartNivra.Gallery.csproj -f net10.0-android
```

On macOS, with a compatible Xcode and MAUI workload:

```sh
dotnet build samples/ChartNivra.Gallery/ChartNivra.Gallery.csproj -f net10.0-maccatalyst -c Debug
dotnet build samples/ChartNivra.Gallery/ChartNivra.Gallery.csproj -f net10.0-maccatalyst -c Debug -t:Run
```

For Windows, use `net10.0-windows10.0.19041.0`. iOS builds require a Mac; physical-device deployment also requires signing and provisioning.

## Source layout

- `Pages/HomePage.cs`: dashboard card definitions and navigation.
- `Pages/ChartPageBase.cs` and individual pages: chart previews and displayed snippets.
- `Pages/ChartCatalogPreviewPage.cs`: previews and snippets for catalog entries without dedicated pages.
- `Pages/Demos`: streaming and 3D studio pages.
- `Controls`: chart thumbnails, code viewer, appearance, and responsive helpers.
- `Platforms` and `Resources`: platform setup and shared app assets.

The website's gallery data is exported from these files with `python tools/sync_gallery_site.py` from the repository root. The separate [PackageSmoke app](../../tests/ChartNivra.PackageSmoke/README.md) is a point-in-time copy and must be updated when Gallery changes.
