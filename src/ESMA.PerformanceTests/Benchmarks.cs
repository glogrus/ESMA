// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmarks.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.PerformanceTests
{
    using System.Collections.Generic;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Order;

    /// <summary>
    ///     The benchmarks.
    /// </summary>
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]

    ////[SimpleJob(RunStrategy.ColdStart, RuntimeMoniker.Net472, 4, 3, 20, -1, "Net472")]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MarkdownExporterAttribute.GitHub]
    [HtmlExporter]
    [RankColumn]
    public class Benchmarks
    {
        /// <summary>
        ///     Gets or sets the algorithm.
        /// </summary>
        [ParamsSource(nameof(Algorithms))]
        public string Algorithm { get; set; }

        /// <summary>
        ///     The algorithm enumerator.
        /// </summary>
        public IEnumerable<string> Algorithms => AlgorithmEnumerator.Algorithms.Keys;

        /// <summary>
        /// Gets the file fixtures.
        /// </summary>
        public IEnumerable<FileFixture> FileFixtures
        {
            get
            {
                FileFixture.ConfigFile = "FileFixture.xml";
                return FileFixture.FileFixtures;
            }
        }

        /// <summary>
        ///     Gets or sets the fixture.
        /// </summary>
        [ParamsSource(nameof(FileFixtures))]
        public FileFixture Fixture { get; set; }

        /// <summary>
        ///     Gets or sets the source.
        /// </summary>
        [Params("Memory", "File")]
        public string Source { get; set; }

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
                    matcher.Match(this.Fixture.Data, this.Fixture.Pattern);
                    break;
                case "File":
                    matcher.BufferSize = this.Fixture.BufferSize;
                    matcher.Match(this.Fixture.DataFilePath, this.Fixture.Pattern);
                    break;
            }
        }
    }
}