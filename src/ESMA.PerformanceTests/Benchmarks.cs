// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmarks.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.PerformanceTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Order;

    /// <summary>
    ///     The benchmarks.
    /// </summary>
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]

    ////[SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MarkdownExporterAttribute.GitHub]
    [HtmlExporter]
    [RankColumn]
    [MedianColumn]
    public class Benchmarks
    {
        /// <summary>
        ///     The data directory.
        /// </summary>
#if NETFRAMEWORK
        private const string DataDirectory = "../../../../../data/";
#else
        private const string DataDirectory = "../../../../../../../../../data/";
#endif

        /// <summary>
        ///     The file data.
        /// </summary>
        private const string FileData = "SearchTest.bin";

        /// <summary>
        ///     The file pattern.
        /// </summary>
        private const string FilePattern = "SearchTest.pattern";

        /// <summary>
        ///     The data.
        /// </summary>
        private byte[] data;

        /// <summary>
        ///     The pattern.
        /// </summary>
        private byte[] pattern;

        /// <summary>
        ///     The file data path.
        /// </summary>
        public static string FileDataPath => $"{DataDirectory}/{FileData}";

        /// <summary>
        ///     The file pattern path.
        /// </summary>
        public static string FilePatternPath => $"{DataDirectory}/{FilePattern}";

        /// <summary>
        ///     Gets or sets the algorithm.
        /// </summary>
        [ParamsSource(nameof(Algorithms))]
        public string Algorithm { get; set; }

        /// <summary>
        ///     Gets or sets the source.
        /// </summary>
        [Params("Memory")]
        public string Source { get; set; }

        /// <summary>
        ///     The algorithm enumerator.
        /// </summary>
        public IEnumerable<string> Algorithms => AlgorithmEnumerator.Algorithms.Keys;

        /// <summary>
        ///     The file.
        /// </summary>
        [Benchmark]
        public void Match()
        {
            var matcher = AlgorithmEnumerator.GetClass(this.Algorithm);
            switch (this.Source)
            {
                case "Memory":
                    matcher.Match(this.data, this.pattern);
                    break;
                case "File":
                    matcher.Match(FileDataPath, this.pattern);
                    break;
            }
        }

        /// <summary>
        ///     The initial data.
        /// </summary>
        [GlobalSetup]
        public void InitialData()
        {
            this.pattern = ReadFile(FilePatternPath);
            this.data = ReadFile(FileDataPath);
        }

        /// <summary>
        /// The read file.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <returns>
        /// The all bytes from file.
        /// </returns>
        private static byte[] ReadFile(string filePath)
        {
            try
            {
                return File.ReadAllBytes(filePath);
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }
    }
}