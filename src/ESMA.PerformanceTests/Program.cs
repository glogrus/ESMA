// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.PerformanceTests
{
    using BenchmarkDotNet.Running;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        private static void Main()
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}