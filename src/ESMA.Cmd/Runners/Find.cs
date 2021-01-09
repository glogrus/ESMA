// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Find.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Cmd.Runners
{
    using System;
    using System.IO;

    using ESMA.Cmd.Options;

    /// <summary>
    ///     The find.
    /// </summary>
    internal static class Find
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
        public static int Run(VerbFind args)
        {
            var matcher = AlgorithmEnumerator.GetClass(args.Algorithm);

            byte[] pattern;
            try
            {
                pattern = File.ReadAllBytes(args.Pattern);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            try
            {
                matcher.BufferSize = 65536;
                var indexes = matcher.Match(args.File, pattern);
                var i = 1;
                foreach (var index in indexes)
                {
                    Console.WriteLine($"{i}: {index}");
                    i++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            return 0;
        }
    }
}