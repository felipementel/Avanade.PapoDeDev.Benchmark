using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnosers;
using System.Threading;
using System.Threading.Tasks;

namespace Benchmarks.Cases
{
    //[InProcess]
    //[Config(typeof(FastRunConfig))]
    [MemoryDiagnoser, MarkdownExporter]
    public class ThreadVsTask
    {
        [Params(10, 100)]
        public int Size { get; set; }

        [Benchmark]
        public void UsingThread()
        {
            var t = new Thread(Run);

            t.Start();
        }

        [Benchmark]
        public void UsingTask()
        {
            Task.Factory.StartNew(() => Run());
        }

        public void Run()
        {
            for (int i = 0; i < Size; i++)
            {

            }
        }
    }
}
