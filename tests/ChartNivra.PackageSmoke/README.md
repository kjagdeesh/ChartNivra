# Local NuGet consumer test

This MAUI application is a copy of the Gallery pages and resources. It references the local `ChartNivra` NuGet package, not the source project, so the gallery can exercise the packaged chart library. Its application ID is separate from the development Gallery so both can be installed.

This is a point-in-time copy: when the development Gallery changes, update the corresponding files here to keep the package test in sync.

From the repository root, create the local preview package:

```shell
dotnet pack src/ChartNivra/ChartNivra.csproj -c Release -p:PackageVersion=0.1.0-preview.1 -o artifacts/local-feed
```

Build and run the consumer on the target platform:

```shell
# Windows
dotnet build tests/ChartNivra.PackageSmoke/ChartNivra.PackageSmoke.csproj -f net10.0-windows10.0.19041.0
dotnet build tests/ChartNivra.PackageSmoke/ChartNivra.PackageSmoke.csproj -f net10.0-windows10.0.19041.0 -t:Run

# Mac Catalyst (on a Mac with Xcode and the MAUI workload)
dotnet build tests/ChartNivra.PackageSmoke/ChartNivra.PackageSmoke.csproj -f net10.0-maccatalyst
dotnet build tests/ChartNivra.PackageSmoke/ChartNivra.PackageSmoke.csproj -f net10.0-maccatalyst -t:Run

# Android (with a configured device or emulator)
dotnet build tests/ChartNivra.PackageSmoke/ChartNivra.PackageSmoke.csproj -f net10.0-android
dotnet build tests/ChartNivra.PackageSmoke/ChartNivra.PackageSmoke.csproj -f net10.0-android -t:Run

# iOS requires a Mac build host, and device deployment requires signing.
```

The local feed is `artifacts/local-feed`; it and the package are not committed. Build the package before restoring the consumer, or copy the package into that folder on another machine. A package built on Windows contains all configured platform targets; a package built on macOS does not contain the Windows target.

When testing changed library code, increment the preview version in both the pack command and this project's package reference. NuGet caches package versions, so reusing a version can make the consumer load an older package. This test does not publish the package.
