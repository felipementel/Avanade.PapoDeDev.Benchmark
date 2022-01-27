using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benchmarks.Cases
{
    //[InProcess]
    //[Config(typeof(FastRunConfig))]
    [MemoryDiagnoser, MarkdownExporter]
    public class ForVsForeachWithLists
    {
        [Params(10, 200)]
        public int Size { get; set; }

        public List<TestObject> ListA { get; set; }
        public List<TestObject> ListB { get; set; }

        [Benchmark]
        public void UsingWhile()
        {
            var i = 0;

            while(i < Size)
            {
                var j = 0;

                while (j < Size)
                {
                    if (ListA[i].P1 == ListB[j].P1)
                    {
                        ListA[i].P2 = ListB[j].P2;
                        break;
                    }
                    j++;
                }
                i++;
            }
        }

        [Benchmark]
        public void UsingFor()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (ListA[i].P1 == ListB[j].P1)
                    {
                        ListA[i].P2 = ListB[j].P2;
                        break;
                    }
                }
            }
        }

        [Benchmark]
        public void UsingForeach()
        {
            foreach (var itemA in ListA)
            {
                foreach (var itemB in ListB)
                {
                    if (itemA.P1 == itemB.P1)
                    {
                        itemA.P2 = itemB.P2;
                        break;
                    }
                }
            }
        }

        [Benchmark]
        public void UsingLinqForeach()
        {
            ListA.ForEach(itemA =>
            {
                ListB.ForEach(itemB =>
                {
                    if (itemA.P1 == itemB.P1)
                    {
                        itemA.P2 = itemB.P2;
                    }
                });
            });
        }


        [IterationSetup]
        public void Setup()
        {
            ListA = new List<TestObject>();
            ListB = new List<TestObject>();

            var count = 1;
            Random rn = new Random();

            while (count <= Size)
            {
                var num = rn.Next(1, Size + 1);

                var item = new TestObject { P1 = num };

                if (ListA.Contains(item))
                {
                    continue;
                }

                ListA.Add(item);
                count++;
            }

            count = 1;

            while (count <= Size)
            {
                var num = rn.Next(1, Size + 1);

                var item = new TestObject { P1 = num };

                if (ListB.Contains(item))
                {
                    continue;
                }

                ListB.Add(item);
                count++;
            }
        }
    }

    public class TestObject
    {
        public int P1 { get; set; }
        public int P2 { get; set; }
    }

    
}
