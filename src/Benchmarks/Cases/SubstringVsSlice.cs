using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Benchmarks.Cases
{
    //[InProcess]
    //[Config(typeof(FastRunConfig))]
    [MemoryDiagnoser, MarkdownExporter]
    public class SubstringVsSlice
    {
        private static string Path { get => "TestFile.txt"; }
        private readonly static StreamReader Reader = new StreamReader(Path);
        private readonly List<int> ListInt = new List<int>();
        private readonly List<string> ListString = new List<string>();

        [Benchmark]
        public void UseSubstring()
        {
            TextReader Reader = GetReader();
            {
                string line;

                while ((line = Reader.ReadLine()) != null)
                {
                    line.Substring(4, 10);
                }

            }
        }

        [Benchmark]
        public void UseSlice()
        {
            Span<char> span = stackalloc char[21];

            TextReader Reader = GetReader();
            {
                while (Reader.Read(span) > 0)
                {
                    span.Slice(4, 10);
                }
            }
        }

        [Benchmark]
        public void UseSubstringToAddString()
        {
            TextReader Reader = GetReader();
            {
                string line;

                while ((line = Reader.ReadLine()) != null)
                {
                    ListString.Add(line.Substring(4, 10));
                }

            }
        }

        [Benchmark]
        public void UseSliceToAddString()
        {
            Span<char> span = stackalloc char[21];

            TextReader Reader = GetReader();
            {
                while (Reader.Read(span) > 0)
                {
                    ListString.Add(span.Slice(4, 10).ToString());
                }
            }
        }

        [Benchmark]
        public void UseSubstringToAddInt()
        {
            TextReader Reader = GetReader();
            {
                string line;

                while ((line = Reader.ReadLine()) != null)
                {
                    ListInt.Add(int.Parse(line.Substring(0, 4)));
                }

            }
        }

        [Benchmark]
        public void UseSliceToAddInt()
        {
            Span<char> span = stackalloc char[21];

            TextReader Reader = GetReader();
            {
                while (Reader.Read(span) > 0)
                {
                    ListInt.Add(int.Parse(span.Slice(0, 4)));
                }
            }
        }

        public StreamReader GetReader()
        {
            Reader.BaseStream.Position = 0;
            Reader.DiscardBufferedData();
            return Reader;
        }

        [GlobalSetup]
        public void Setup()
        {

            if (!File.Exists(Path))
            {
                Random rn = new Random();

                using (StreamWriter writer = new StreamWriter(Path))
                {
                    for (int i = 0; i < 100_000; i++)
                    {
                        StringBuilder sb = new StringBuilder();

                        sb.Append(rn.Next(0, 10000).ToString().PadLeft(4, '0'));
                        sb.Append(RandomString(rn.Next(1, 10)).PadRight(10, ' '));
                        sb.Append(rn.Next(0, 100000).ToString().PadLeft(5, '0'));

                        writer.WriteLine(sb.ToString());
                    }
                }
            }
        }

        public static string RandomString(int length)
        {
            Random rn = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rn.Next(s.Length)]).ToArray());
        }
    }
}
