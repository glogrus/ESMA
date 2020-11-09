// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Cmd
{
    using System;
    using System.Linq;
    using System.Text;

    /// <summary>
    ///     The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            var algorithms = AlgorithmEnumerator.Algorithms;
            var pattern = Encoding.ASCII.GetBytes("TestStringPattern");
            var data = new byte[256];
            Buffer.BlockCopy(pattern, 0, data, 50, pattern.Length);
            Buffer.BlockCopy(pattern, 0, data, 200, pattern.Length);

            foreach (var (name, algorithm) in algorithms.Select(x => (x.Key, x.Value)))
            {
                Console.WriteLine($"Algorithm: \"{name}\"");
                algorithm.Pattern = pattern;
                var indexes = algorithm.Match(data);
                foreach (var index in indexes)
                {
                    Console.WriteLine($"Found: {index}");
                }
            }
        }
    }
}