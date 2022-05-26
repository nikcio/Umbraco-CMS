``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1706 (21H1/May2021Update)
AMD FX(tm)-8350, 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  DefaultJob : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT


```
|                     Method |        Mean |     Error |    StdDev |
|--------------------------- |------------:|----------:|----------:|
|     ContentLifeCycleSingle |    262.6 ms |   5.40 ms |  15.67 ms |
| BulkContentLifeCycleSingle |    265.0 ms |   5.27 ms |  13.33 ms |
|        ContentLifeCycleTen |  2,221.9 ms |  43.75 ms |  55.32 ms |
|    BulkContentLifeCycleTen |  1,828.1 ms |  36.49 ms |  94.19 ms |
|     ContentLifeCycleTwenty |  4,496.2 ms |  88.43 ms | 121.04 ms |
| BulkContentLifeCycleTwenty |  3,497.3 ms |  69.61 ms | 123.73 ms |
|      ContentLifeCycleFifty | 10,979.0 ms | 197.44 ms | 175.02 ms |
|  BulkContentLifeCycleFifty |  8,635.4 ms | 167.80 ms | 218.19 ms |
