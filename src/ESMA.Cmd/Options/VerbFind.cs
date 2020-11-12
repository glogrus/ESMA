// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerbFind.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Cmd.Options
{
    using CommandLine;

    /// <summary>
    ///     The verb find.
    /// </summary>
    [Verb("find", HelpText = "Find pattern in file.")]
    internal class VerbFind
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerbFind"/> class.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="algorithm">
        /// The algorithm.
        /// </param>
        public VerbFind(string file, string pattern, string algorithm)
        {
            this.File = file;
            this.Pattern = pattern;
            this.Algorithm = algorithm;
        }

        /// <summary>
        ///     Gets the algorithm.
        /// </summary>
        [Option('a', "algorithm", Required = true, HelpText = "Algorithm.")]
        public string Algorithm { get; }

        /// <summary>
        ///     Gets the file.
        /// </summary>
        [Option('f', "file", Required = true, HelpText = "File name.")]
        public string File { get; }

        /// <summary>
        ///     Gets the pattern.
        /// </summary>
        [Option('p', "pattern", Required = true, HelpText = "Pattern file name.")]
        public string Pattern { get; }
    }
}