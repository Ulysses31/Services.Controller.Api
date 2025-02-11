
using System.Text;
using BenchmarkDotNet.Attributes;

namespace Services.Controllers.API;


[MemoryDiagnoser]
public class MemoryBenchmarkerDemo
{
  int NumberOfItems = 10;
  
  [Benchmark]
  public string ConcatStringsUsingStringBuilder()
  {
    var sb = new StringBuilder();
    for (int i = 0; i < NumberOfItems; i++)
    {
      sb.Append("Hello World!" + i);
    }
    return sb.ToString();
  }

  [Benchmark]
  public string ConcatStringsUsingGenericList()
  {
    var list = new List<string>(NumberOfItems);
    for (int i = 0; i < NumberOfItems; i++)
    {
      list.Add("Hello World!" + i);
    }
    return list.ToString()!;
  }
}
