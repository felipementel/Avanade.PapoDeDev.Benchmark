using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Jobs;
using System.Buffers;

namespace Benchmarks.Cases
{
    //[InProcess]
    //[Config(typeof(FastRunConfig))]
    [MemoryDiagnoser, MarkdownExporter]
    public class ArrayAllocation
    {
        [Params((int)1E+2, // 100 bytes
            (int)1E+3, // 1 000 bytes = 1 KB
            (int)1E+4, // 10 000 bytes = 10 KB
            (int)1E+5, // 100 000 bytes = 100 KB
            (int)1E+6, // 1 000 000 bytes = 1 MB
            (int)1E+7)] // 10 000 000 bytes = 10 MB
        public int SizeInBytes { get; set; }

        private ArrayPool<byte> sizeAwarePool;

        [GlobalSetup]
        public void GlobalSetup()
            => sizeAwarePool = ArrayPool<byte>.Create(SizeInBytes + 1, 10); // let's create the pool that knows the real max size

        [Benchmark]
        public void Allocate()
            => DeadCodeEliminationHelper.KeepAliveWithoutBoxing(new byte[SizeInBytes]);

        [Benchmark]
        public void RentAndReturn_Shared()
        {
            var pool = ArrayPool<byte>.Shared;
            byte[] array = pool.Rent(SizeInBytes);
            pool.Return(array);
        }

        [Benchmark]
        public void RentAndReturn_Aware()
        {
            var pool = sizeAwarePool;
            byte[] array = pool.Rent(SizeInBytes);
            pool.Return(array);
        }
    }

    //public class DontForceGcCollectionsConfig : ManualConfig
    //{
    //    public DontForceGcCollectionsConfig()
    //    {
    //        Add(Job.Default
    //            .With(new GcMode()
    //            {
    //                Force = false // tell BenchmarkDotNet not to force GC collections after every iteration
    //        }));
    //    }
    //}

    public class FastRunConfig : ManualConfig
    {
        public FastRunConfig()
        {
            var job = new Job();
            job.Run.IterationTime = new Perfolizer.Horology.TimeInterval(100);
            job.Run.LaunchCount = 1;
            job.Run.WarmupCount = 1;
            job.Accuracy.EvaluateOverhead = false;
            job.Run.RunStrategy = RunStrategy.ColdStart;
            job.Run.MinWarmupIterationCount = 1;
            job.Run.MaxWarmupIterationCount = 2;
            job.Run.MinIterationCount = 1;
            job.Run.MinIterationCount = 2;

            AddJob(job);
        }
    }


    //public class ArrayAllocation
    //{
    //    [Params(
    //        100,
    //        1000,
    //        10000,
    //        100000,
    //        1000000,
    //        10000000)]
    //    public int Size { get; set; }

    //    private ArrayPool<byte> Pool;

    //    [GlobalSetup(Targets = new[] { nameof(UseArray), nameof(UseArrayPool), nameof(UseArrayPoolShared) })]
    //    public void Setup()
    //    {
    //        Pool = ArrayPool<byte>.Create(Size + 1, 10);

    //    }

    //    [Benchmark]
    //    public void UseArray()
    //    {
    //        //Necessário dizer ao benchmark que não desejamos que os "dead codes" sejam eliminados
    //        DeadCodeEliminationHelper.KeepAliveWithoutBoxing(new byte[Size]);
    //    }

    //    [Benchmark]
    //    public void UseArrayPool()
    //    {
    //        var pool = Pool;
    //        byte[] array = pool.Rent(Size);
    //        pool.Return(array);
    //    }

    //    [Benchmark]
    //    public void UseArrayPoolShared()
    //    {
    //        var pool = ArrayPool<byte>.Shared;
    //        byte[] array = pool.Rent(Size);
    //        pool.Return(array);
    //    }


    //    public class MyConfig : ManualConfig
    //    {
    //        public MyConfig()
    //        {
    //            // Configuração para que o benchmark não force execuções do GC
    //            AddJob();
    //        }
    //    }
    //    public class FastRunConfig : ManualConfig
    //    {
    //        public FastRunConfig()
    //        {
    //            var job = new Job();
    //            job.Run.IterationTime = new Perfolizer.Horology.TimeInterval(100);
    //            job.Run.LaunchCount = 1;
    //            job.Run.WarmupCount = 1;
    //            job.Accuracy.EvaluateOverhead = false;
    //            job.Run.RunStrategy = RunStrategy.ColdStart;
    //            job.Run.MinWarmupIterationCount = 1;
    //            job.Run.MaxWarmupIterationCount = 2;
    //            job.Run.MinIterationCount = 1;
    //            job.Run.MinIterationCount = 2;

    //            AddJob(job);
    //        }
    //    }

    //}
}
