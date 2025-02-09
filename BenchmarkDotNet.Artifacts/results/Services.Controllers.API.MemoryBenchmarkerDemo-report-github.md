```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26120.3073)
11th Gen Intel Core i7-1165G7 2.80GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.102
  [Host]     : .NET 9.0.1 (9.0.124.61010), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 9.0.1 (9.0.124.61010), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


```
| Method                          | Mean     | Error    | StdDev   | Gen0   | Allocated |
|-------------------------------- |---------:|---------:|---------:|-------:|----------:|
| ConcatStringsUsingStringBuilder | 530.7 ns | 33.56 ns | 95.76 ns | 0.0763 |    1640 B |
| ConcatStringsUsingGenericList   | 218.9 ns |  8.41 ns | 24.27 ns | 0.0277 |     616 B |
