// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseMatch.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     The Searcher interface.
    /// </summary>
    public abstract class BaseMatch
    {
        /// <summary>
        ///     The pattern.
        /// </summary>
        private byte[] pattern;

        /// <summary>
        ///     Initializes static members of the <see cref="BaseMatch" /> class.
        /// </summary>
        static BaseMatch()
        {
            FileBufferSize = 64 * 1024;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMatch"/> class.
        /// </summary>
        /// <param name="patternArray">
        /// The pattern array.
        /// </param>
        protected BaseMatch(byte[] patternArray)
        {
            this.Pattern = patternArray;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseMatch" /> class.
        /// </summary>
        protected BaseMatch()
        {
        }

        /// <summary>
        ///     Gets or sets the file buffer size.
        /// </summary>
        public static int FileBufferSize { get; set; }

        /// <summary>
        ///     Gets or sets the pattern.
        /// </summary>
        public byte[] Pattern
        {
            get => this.pattern;
            set
            {
                this.pattern = value;
                this.Prepared = this.Prepare();
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether prepared.
        /// </summary>
        private bool Prepared { get; set; }

        /// <summary>
        /// The search.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The array of index.
        /// </returns>
        public long[] Match(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (this.pattern == null)
            {
                throw new ArgumentNullException(nameof(this.pattern));
            }

            if (data.Length < this.pattern.Length)
            {
                return new long[0];
            }

            if (!this.Prepared)
            {
                throw new ArgumentException("Pattern not prepared.");
            }

            var matchIndexes = new List<long>();
            var index = this.InternalMatch(data, matchIndexes);
            Console.WriteLine($"{index}");
            return matchIndexes.ToArray();
        }

        /// <summary>
        /// The search.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="patternArray">
        /// The pattern array.
        /// </param>
        /// <returns>
        /// The array of index.
        /// </returns>
        public long[] Match(byte[] data, byte[] patternArray)
        {
            this.Pattern = patternArray;
            return this.Match(data);
        }

        /// <summary>
        /// The search.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <param name="patternArray">
        /// The pattern array.
        /// </param>
        /// <returns>
        /// The array of index.
        /// </returns>
        public long[] Match(string path, byte[] patternArray)
        {
            this.Pattern = patternArray;
            return this.Match(path);
        }

        /// <summary>
        /// The search.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The array of index.
        /// </returns>
        public long[] Match(string path)
        {
            var matchIndexes = new List<long>();
            if (this.pattern == null)
            {
                throw new ArgumentNullException(nameof(this.pattern));
            }

            if (!this.Prepared)
            {
                throw new ArgumentException("Pattern not prepared.");
            }

            using (var fileStream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                FileBufferSize,
                FileOptions.SequentialScan))
            {
                var patternLength = this.Pattern.Length;
                var bufferSize = FileBufferSize > patternLength ? FileBufferSize : patternLength;
                var buffer = new byte[bufferSize];
                long filePosition = 0;
                var total = bufferSize;

                while (total >= patternLength)
                {
                    total = fileStream.Read(buffer, 0, buffer.Length);

                    if (total < patternLength)
                    {
                        break;
                    }

                    if (total < bufferSize)
                    {
                        Array.Resize(ref buffer, total);
                    }

                    var nextPosition = this.InternalMatch(buffer, matchIndexes, filePosition);
                    if (nextPosition < total || nextPosition < fileStream.Length)
                    {
                        fileStream.Seek(nextPosition - total, SeekOrigin.Current);
                        filePosition += nextPosition;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return matchIndexes.ToArray();
        }

        /// <summary>
        /// The internal search.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="indexes">
        /// The list of index.
        /// </param>
        /// <param name="offset">
        /// The offset in all buffer.
        /// </param>
        /// <returns>
        /// The next offset.
        /// </returns>
        protected abstract long InternalMatch(byte[] data, List<long> indexes, long offset = 0);

        /// <summary>
        ///     The pattern prepare.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected virtual bool Prepare()
        {
            return true;
        }
    }
}