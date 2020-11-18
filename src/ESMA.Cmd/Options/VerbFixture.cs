// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VerbCreate.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Cmd.Options
{
    using CommandLine;

    /// <summary>
    ///     The storage options.
    /// </summary>
    [Verb("fixture", HelpText = "Create tests files.")]
    internal class VerbFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VerbFixture"/> class.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="patternCount">
        /// The pattern count.
        /// </param>
        /// <param name="patternSize">
        /// The pattern size.
        /// </param>
        /// <param name="size">
        /// The size.
        /// </param>
        public VerbFixture(string file, string path, int patternCount, int patternSize, int size)
        {
            this.File = file;
            this.Path = path;
            this.PatternCount = patternCount;
            this.PatternSize = patternSize;
            this.Size = size;
        }

        /// <summary>
        ///     Gets the file.
        /// </summary>
        [Option('f', "fixture", Default = "random", Required = false, HelpText = "Fixture file name.")]
        public string File { get; }

        /// <summary>
        ///     Gets the path.
        /// </summary>
        [Option('d', "directory", Default = ".", Required = false, HelpText = "Path to data directory.")]
        public string Path { get; }

        /// <summary>
        ///     Gets the pattern count.
        /// </summary>
        [Option('c', "count", Default = 5, Required = false, HelpText = "Pattern count.")]
        public int PatternCount { get; }

        /// <summary>
        ///     Gets the pattern size.
        /// </summary>
        [Option('p', "patternsize", Default = 64, Required = false, HelpText = "Pattern size.")]
        public int PatternSize { get; }

        /// <summary>
        ///     Gets the size.
        /// </summary>
        [Option('s', "size", Default = 1048576, Required = false, HelpText = "Data size.")]
        public int Size { get; }
    }
}