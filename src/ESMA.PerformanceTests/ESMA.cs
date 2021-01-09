// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ESMA.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.PerformanceTests
{
    using System.Collections.Generic;
    using System.Globalization;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Exporters.Csv;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Reports;

    using Perfolizer.Horology;

    /// <summary>
    ///     The benchmarks.
    /// </summary>
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [MarkdownExporterAttribute.GitHub]
    [HtmlExporter]
    [RankColumn]
    [Config(typeof(Config))]
    public class Esma
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
        ///     Gets the file fixtures.
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
        [Params("File")]
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

        /// <summary>
        ///     The config.
        /// </summary>
        private class Config : ManualConfig
        {
            /// <summary>
            ///     Initializes a new instance of the <see cref="Config" /> class.
            /// </summary>
            public Config()
            {
                this.AddExporter(
                    new CsvExporter(
                        CsvSeparator.CurrentCulture,
                        new SummaryStyle(
                            CultureInfo.InvariantCulture,
                            true,
                            SizeUnit.KB,
                            TimeUnit.Microsecond,
                            false)));
            }
        }
    }
}