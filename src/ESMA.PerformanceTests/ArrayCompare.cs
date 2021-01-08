// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayCompare.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.PerformanceTests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Columns;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Engines;
    using BenchmarkDotNet.Exporters.Csv;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Order;
    using BenchmarkDotNet.Reports;

    using Perfolizer.Horology;

    /// <summary>
    ///     The benchmarks.
    /// </summary>
    [Orderer(SummaryOrderPolicy.Declared)]
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [Config(typeof(Config))]
    public class ArrayCompare
    {
        /// <summary>
        ///     The array 1.
        /// </summary>
        private byte[] array1;

        /// <summary>
        ///     The array 2.
        /// </summary>
        private byte[] array2;

        /// <summary>
        ///     Gets or sets the algorithm.
        /// </summary>
        [ParamsSource(nameof(Algorithms))]
        public string Algorithm { get; set; }

        /// <summary>
        ///     The algorithms.
        /// </summary>
        public IEnumerable<string> Algorithms => ArrayCompareEnumerator.Algorithms.Keys;

        /// <summary>
        ///     Gets or sets the fill factor.
        /// </summary>
        [Params(0, 50, 100)]
        public int FillFactor { get; set; }

        /// <summary>
        ///     Gets or sets the size.
        /// </summary>
        [Params(8, 16, 32, 64, 128, 256, 512, 1024)]
        public int Size { get; set; }

        /// <summary>
        ///     The file.
        /// </summary>
        [Benchmark]
        public void Compare()
        {
            var comparer = ArrayCompareEnumerator.GetClass(this.Algorithm);
            comparer.ArrayEquals(this.array1, this.array2);
        }

        /// <summary>
        ///     The global setup.
        /// </summary>
        [GlobalSetup]
        public void GlobalSetup()
        {
            this.array1 = new byte[this.Size];
            this.array2 = new byte[this.Size];
            var random = new Random();
            random.NextBytes(this.array1);
            random.NextBytes(this.array2);
            if (this.FillFactor > 0 && this.FillFactor <= 100)
            {
                Buffer.BlockCopy(this.array1, 0, this.array2, 0, this.FillFactor * this.Size / 100);
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
                        new SummaryStyle(CultureInfo.InvariantCulture, true, SizeUnit.KB, TimeUnit.Nanosecond, false)));
            }
        }
    }
}