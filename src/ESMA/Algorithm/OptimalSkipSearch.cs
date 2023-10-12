// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptimalSkipSearch.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The Skip Search.
    /// </summary>
    [Algorithm("Optimal Skip Search")]
    public class OptimalSkipSearch : BaseMatch
    {
        /// <summary>
        ///     The skip list.
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
            var list = this.skipList;

            int i;
            for (i = patternLength - 1; i < length; i += patternLength)
            {
                for (var node = list[data[i]]; node != null; node = node.Next)
                {
                    int j;
                    var index = i - node.Value;
                    for (j = 0; j < patternLength && index + j < length && pattern[j] == data[index + j]; j++)
                    {
                    }

                    if (j >= patternLength)
                    {
                        indexes.Add(index + offset);
                    }
                   
                }
            }

            return i > length ? i - patternLength - 1 : i;
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