``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1706 (21H1/May2021Update)
AMD FX(tm)-8350, 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.202
  [Host]     : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT
  DefaultJob : .NET 6.0.4 (6.0.422.16404), X64 RyuJIT


```
|                          Method |       Mean |     Error |      StdDev |
|-------------------------------- |-----------:|----------:|------------:|
|     ContentSaveAndPublishSingle |   142.7 ms |   2.83 ms |     7.80 ms |
| BulkContentSaveAndPublishSingle |   140.2 ms |   2.79 ms |     7.14 ms |
|        ContentSaveAndPublishTen | 1,265.5 ms |  25.08 ms |    49.50 ms |
|    BulkContentSaveAndPublishTen | 1,239.9 ms |  24.52 ms |    46.06 ms |
|     ContentSaveAndPublishTwenty | 2,810.7 ms |  88.46 ms |   260.82 ms |
|  BulkContenSaveAndPublishTwenty | 2,532.6 ms |  70.60 ms |   208.17 ms |
|      ContentSaveAndPublishFifty | 8,508.8 ms | 412.43 ms | 1,216.05 ms |
|  BulkContentSaveAndPublishFifty | 7,576.4 ms | 389.91 ms | 1,149.67 ms |
