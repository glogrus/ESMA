// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParallelSkipSearch.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    ///     The Parallel Skip Search.
    /// </summary>
    [Algorithm("Parallel Skip Search")]
    public class ParallelSkipSearch : BaseMatch
    {
        /// <summary>
        ///     The skip search list.
        /// </summary>
        private Node[] skipList;

        /// <summary>
        /// The internal match.
        /// </summary>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <param name="indexes">
        /// The indexes.
        /// </param>
        /// <param name="length">
        /// The length.
        /// </param>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <returns>
        /// The array of index.
        /// </returns>
        protected override int InternalMatch(byte[] data, List<long> indexes, int length, long offset = 0)
        {
            var pattern = this.Pattern;
            var patternLength = pattern.Length;
            var upper = length - patternLength + 1;
            var list = this.skipList;
            var bag = new ConcurrentBag<long>();

            Parallel.ForEach(
                this.Iterator(pattern.Length - 1, length, patternLength, data),
                i =>
                {
                    for (var node = list[data[i]]; node != null; node = node.Next)
                    {
                        int j;
                        var index = i - node.Value;
                        for (j = 0; j < patternLength && j < length - index && pattern[j] == data[index + j]; j++)
                        {
                        }

                        if (j >= patternLength)
                        {
                            bag.Add(index + offset);
                        }
                    }
                });

            if (!bag.IsEmpty)
            {
                var sorted = bag.ToList();
                sorted.Sort();
                indexes.AddRange(sorted);
            }

            return upper;
        }

        /// <summary>
        ///     The prepare.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected override bool Prepare()
        {
            this.skipList = new Node[256];
            for (var i = 0; i < this.Pattern.Length; i++)
            {
                this.skipList[this.Pattern[i]] = new Node { Value = i, Next = this.skipList[this.Pattern[i]] };
            }

            return true;
        }

        /// <summary>
        /// The iterator.
        /// </summary>
        /// <param name="startIndex">
        /// The start index.
        /// </param>
        /// <param name="endIndex">
        /// The end index.
        /// </param>
        /// <param name="stepSize">
        /// The step size.
        /// </param>
        /// <param name="data">
        /// The data.
        /// </param>
        /// <returns>
        /// The Iterator with steps.
        /// </returns>
        private IEnumerable<int> Iterator(int startIndex, int endIndex, int stepSize, IReadOnlyList<byte> data)
        {
            for (var i = startIndex; i < endIndex; i += stepSize)
            {
                if (this.skipList[data[i]] != null)
                {
                    yield return i;
                }
            }
        }

        /// <summary>
        ///     The node.
        /// </summary>
        internal class Node
        {
            /// <summary>
            ///     Gets or sets the next.
            /// </summary>
            public Node Next { get; set; }

            /// <summary>
            ///     Gets or sets the value.
            /// </summary>
            public int Value { get; set; }
        }
    }
}