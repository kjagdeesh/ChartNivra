# ChartNivra benchmarks

This .NET 10 console project uses BenchmarkDotNet and references the ChartNivra library project. It currently measures:

- LTTB decimation of 50,000 points to 1,000 points.
- A batch of 1,000 writes to a streaming ring buffer.
- Numeric axis scale calculation using fixed inputs.

Run from the repository root in Release mode:

```sh
dotnet run -c Release --project benchmarks/ChartNivra.Benchmarks/ChartNivra.Benchmarks.csproj -- --filter "*"
```

Results print in the terminal and are exported to `BenchmarkDotNet.Artifacts/results` relative to the working directory. BenchmarkDotNet can take several minutes for warmup and measurement.

The axis benchmark's fixed inputs may permit compiler optimizations; a previous near-zero result should not be treated as reliable without improving that benchmark. These microbenchmarks do not measure full chart rendering, UI responsiveness, memory over time, or mobile performance.
