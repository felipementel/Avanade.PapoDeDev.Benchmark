using BenchmarkDotNet.Attributes;
using System;

namespace Benchmarks.Cases
{
    //[InProcess]
    //[Config(typeof(FastRunConfig))]
    [MemoryDiagnoser, MarkdownExporter]
    public class BooleanVsException
    {

        [Benchmark]
        public bool ReturnsBoolean()
        {
            return false;
        }

        [Benchmark]
        public void ThrowsException()
        {
            try
            {
                throw new Exception("Error!");
            }
            catch (Exception)
            {
                
            }
            
        }
    }
}
