```

BenchmarkDotNet v0.15.2, macOS Sequoia 15.5 (24F74) [Darwin 24.5.0]
Apple M2, 1 CPU, 8 logical and 8 physical cores
.NET SDK 8.0.412
  [Host]     : .NET 8.0.18 (8.0.1825.31117), Arm64 RyuJIT AdvSIMD
  Job-VIRJHV : .NET 8.0.18 (8.0.1825.31117), Arm64 RyuJIT AdvSIMD

Runtime=.NET 8.0  Server=True  InvocationCount=500  
LaunchCount=1  UnrollFactor=1  WarmupCount=1  

```
| Method     | Mean     | Error   | StdDev  |
|----------- |---------:|--------:|--------:|
| GetRecipes | 108.7 ms | 0.37 ms | 0.34 ms |
