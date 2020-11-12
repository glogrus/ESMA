// --------------------------------------------------------------------------------------------------------------------
// <copyright file="List.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Cmd.Runners
{
    using System;

    using ESMA.Cmd.Options;

    /// <summary>
    ///     The find.
    /// </summary>
    internal static class List
    {
        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Run(VerbList args)
        {
            var algorithms = AlgorithmEnumerator.Algorithms;
            Console.WriteLine($"Found {algorithms.Count} algorithms:");
            foreach (var entry in algorithms)
            {
                Console.WriteLine($"{entry.Key}");
            }

            return 0;
        }
    }
}