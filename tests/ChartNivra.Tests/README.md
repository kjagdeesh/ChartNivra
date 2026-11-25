# ChartNivra tests

This is the consolidated .NET 10 xUnit test project. Tests are organized into `Unit`, `Rendering`, `Visual`, `Integration`, `Performance`, and `CrossPlatform` folders and reference the library project directly.

Run from the repository root:

```sh
dotnet test tests/ChartNivra.Tests/ChartNivra.Tests.csproj
```

To save a Visual Studio test report:

```sh
dotnet test tests/ChartNivra.Tests/ChartNivra.Tests.csproj --logger "trx;LogFileName=results.trx"
```

Results are written to the project's `TestResults` directory. Test parallelization is disabled so timing-sensitive tests do not compete with other tests in this assembly.

The visual and cross-platform folders include headless checks; they are not a replacement for screenshot comparisons, device testing, or MAUI UI tests. In a previous run, the test runner in this environment could not connect to the test host, so the latest suite result should be verified on your machine.
