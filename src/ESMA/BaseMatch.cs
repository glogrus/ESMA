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
            IOBufferSize = 64 * 1024;
        }

        /// <summary>
        ///     Gets or sets the io buffer size.
        /// </summary>
        public static int IOBufferSize { get; set; }

        /// <summary>
        ///     Gets or sets the file buffer size.
        /// </summary>
        public int BufferSize { get; set; } = 64 * 1024;

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
            this.InternalMatch(data, matchIndexes, data.Length);
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
            if (this.pattern == null)
            {
                throw new ArgumentNullException(nameof(this.pattern));
            }

            if (!this.Prepared)
            {
                throw new ArgumentException("Pattern not prepared.");
            }

            return this.MatchInFile(path);
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            var algorithm = (AlgorithmAttribute)Attribute.GetCustomAttribute(
                this.GetType(),
                typeof(AlgorithmAttribute));

            return algorithm == null ? this.GetType().Name : algorithm.Name;
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
        /// <param name="length">
        /// The length.
        /// </param>
        /// <param name="offset">
        /// The offset in all buffer.
        /// </param>
        /// <returns>
        /// The next offset.
        /// </returns>
        protected abstract int InternalMatch(byte[] data, List<long> indexes, int length, long offset = 0);

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

        /// <summary>
        /// The match in file.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The array of index.
        /// </returns>
        private long[] MatchInFile(string path)
        {
            var matchIndexes = new List<long>();
            using (var fileStream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                IOBufferSize,
                FileOptions.SequentialScan))
            {
                if (fileStream.Length < this.pattern.Length)
                {
                    return matchIndexes.ToArray();
                }

                var bufferSize = this.BufferSize > fileStream.Length ? fileStream.Length : this.BufferSize;
                if (bufferSize < this.pattern.Length)
                {
                    bufferSize = this.pattern.Length;
                }

                var buffer = new byte[bufferSize];
                long filePosition = 0;
                var maxFilePosition = fileStream.Length - this.pattern.Length;
                var offset = 0;
                int total;

                while ((total = fileStream.Read(buffer, offset, buffer.Length - offset)) >= this.pattern.Length - offset)
                {
                    var length = total + offset;
                    var nextPosition = this.InternalMatch(buffer, matchIndexes, length, filePosition);
                    filePosition += nextPosition;
                    if (filePosition > maxFilePosition)
                    {
                        break;
                    }

                    if (nextPosition > length)
                    {
                        fileStream.Seek(nextPosition - total, SeekOrigin.Current);
                        offset = 0;
                    }
                    else
                    {
                        offset = length - nextPosition;
                        if (offset > 0)
                        {
                            Buffer.BlockCopy(buffer, nextPosition, buffer, 0, offset);
                        }
                    }
                }
            }

            return matchIndexes.ToArray();
        }
    }
}