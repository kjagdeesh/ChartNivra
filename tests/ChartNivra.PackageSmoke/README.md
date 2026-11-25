# NuGet package Gallery test

This app is a point-in-time copy of the development Gallery. It uses `PackageReference` for `ChartNivra` version `0.1.0-preview.1`, not a project reference, so you can test the library as a package consumer. Its application ID is separate from Gallery's.

## Prepare the local feed

The project looks in `artifacts/local-feed` in the repository root. That directory and `.nupkg` files are ignored by Git. Place the intended preview package there before restoring this project or the complete solution. The package must contain the target platform's library; a package built on Windows includes the configured Windows target, while the project file omits that target on macOS.

When a new test package is explicitly required, build it from the repository root:

```sh
dotnet pack src/ChartNivra/ChartNivra.csproj -c Release -p:PackageVersion=0.1.0-preview.1 -o artifacts/local-feed
```

For subsequent changes, increment the preview version in both the pack command and this app's package reference. NuGet caches packages by version, so reusing a version can load an older build.

## Run

Set `ChartNivra.PackageSmoke` as the startup project in Visual Studio. For example, on Windows:

```sh
dotnet build tests/ChartNivra.PackageSmoke/ChartNivra.PackageSmoke.csproj -f net10.0-windows10.0.19041.0
```

Use the Android, iOS, or Mac Catalyst target with the appropriate workload, host, and device configuration. Compare the app's charts, themes, interactions, and code views with the development Gallery. A successful build alone does not confirm runtime behavior.

This is a copied app, not a live link to Gallery source. Copy relevant Gallery changes here when updating the package test. Creating or using a local package does not publish it.
