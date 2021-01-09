// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fixture.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Cmd.Runners
{
    using System;
    using System.IO;

    using ESMA.Cmd.Options;

    /// <summary>
    ///     The create.
    /// </summary>
    internal static class Fixture
    {
        /// <summary>
        ///     The buffer size.
        /// </summary>
        private const int BufferSize = 65535;

        /// <summary>
        /// The run create and return exit code.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int Run(VerbFixture args)
        {
            if (args.PatternSize <= 0)
            {
                Console.WriteLine($"Length of pattern: {args.PatternSize}");
                return -1;
            }

            if (args.PatternSize > args.Size && args.PatternCount > 0)
            {
                Console.WriteLine(
                    $"Count of pattern: {args.PatternCount}, but length of data: {args.Size} smaller length of pattern: {args.PatternSize}.");
                return -1;
            }

            var pattern = new byte[args.PatternSize];
            var random = new Random();
            random.NextBytes(pattern);
            try
            {
                File.WriteAllBytes(Path.Combine(args.Path, args.File) + @".pattern", pattern);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

            var patternOffsets = new int[args.PatternCount];

            for (var n = 0; n < 10; n++)
            {
                for (var i = 0; i < args.PatternCount; i++)
                {
                    patternOffsets[i] = random.Next(0, args.Size - args.PatternSize);
                }

                Array.Sort(patternOffsets);
                for (var i = 0; i < args.PatternCount - 1; i++)
                {
                    if (patternOffsets[i + 1] - patternOffsets[i] > args.PatternSize)
                    {
                        continue;
                    }

                    patternOffsets[i + 1] = patternOffsets[i] + args.PatternSize;
                }

                if (patternOffsets[args.PatternCount - 1] <= args.Size - args.PatternSize)
                {
                    break;
                }
            }

            if (patternOffsets[args.PatternCount - 1] > args.Size - args.PatternSize)
            {
                Console.WriteLine("Cannot init pattern offsets.");
                return 0;
            }

            var buffer = new byte[BufferSize];
            var position = 0;
            var patternOffsetIndex = 0;

            var patternOffsetInFile = patternOffsets[patternOffsetIndex];
            var patternPosition = 0;
            var patternCopy = false;

            using (var file = File.Create(
                Path.Combine(args.Path, args.File) + @".bin",
                BufferSize,
                FileOptions.SequentialScan))
            {
                try
                {
                    while (position < args.Size)
                    {
                        if (args.Size - position < BufferSize)
                        {
                            buffer = new byte[args.Size - position];
                        }

                        random.NextBytes(buffer);
                        var bufferLength = buffer.Length;
                        var bufOffset = 0;
                        var patternCopyCount = BufferSize;

                        if (patternOffsetInFile >= position && patternOffsetInFile < position + bufferLength)
                        {
                            patternPosition = 0;
                            patternCopy = true;
                            bufOffset = patternOffsetInFile - position;
                            patternCopyCount = (position + bufferLength) - patternOffsetInFile;
                        }

                        while (patternCopy)
                        {
                            if (args.PatternSize - patternPosition < BufferSize)
                            {
                                patternCopyCount = args.PatternSize - patternPosition;
                                patternOffsetIndex++;
                                if (patternOffsetIndex < patternOffsets.Length)
                                {
                                    patternOffsetInFile = patternOffsets[patternOffsetIndex];
                                }

                                patternCopy = false;
                            }

                            Buffer.BlockCopy(pattern, patternPosition, buffer, bufOffset, patternCopyCount);
                            patternPosition += patternCopyCount;

                            if (patternOffsetInFile < position || patternOffsetInFile >= position + bufferLength)
                            {
                                continue;
                            }

                            if (patternOffsetIndex >= patternOffsets.Length)
                            {
                                continue;
                            }

                            patternPosition = 0;
                            patternCopy = true;
                            bufOffset = patternOffsetInFile - position;
                            patternCopyCount = (position + bufferLength) - patternOffsetInFile;
                        }

                        file.Write(buffer, 0, buffer.Length);
                        position += buffer.Length;
                        var percent = (100.0 * position) / args.Size;
                        Console.Write($"Process: {percent:0.}%\r");
                    }

                    Console.WriteLine(string.Empty);

                    foreach (var index in patternOffsets)
                    {
                        Console.Write($"{index} ");
                    }

                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return -1;
                }
                finally
                {
                    file.Close();
                }
            }

            return 0;
        }
    }
}