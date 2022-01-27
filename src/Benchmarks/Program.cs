using BenchmarkDotNet.Running;
using Benchmarks.Cases;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ArrayAllocation>();
            //BenchmarkRunner.Run<BooleanVsException>();
            //BenchmarkRunner.Run<Concatenations>();
            //BenchmarkRunner.Run<ConcatVsStringbuilder>();
            //BenchmarkRunner.Run<ForVsForeachWithLists>();
            //BenchmarkRunner.Run<SubstringVsSlice>();
            //BenchmarkRunner.Run<ThreadVsTask>();
        }
    }
}