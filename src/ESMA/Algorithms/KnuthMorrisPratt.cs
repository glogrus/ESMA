// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KnuthMorrisPratt.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The Knuth-Morris-Pratt.
    /// </summary>
    [Algorithm("Knuth-Morris-Pratt")]
    public class KnuthMorrisPratt : BaseMatch
    {
        /// <summary>
        ///     The Knuth-Morris-Pratt next.
        /// </summary>
        private int[] kmpNext;

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
            var j = 0;
            int i;
            for (i = 0; i <= length - pattern.Length; i++)
            {
                for (j = j != 0 ? this.kmpNext[j] : 0; j < pattern.Length && pattern[j] == data[i + j]; j++)
                {
                }

                if (j < pattern.Length)
                {
                    continue;
                }

                indexes.Add(i + offset);
                j = 0;
            }

            return i;
        }

        /// <summary>
        ///     The prepare.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected override bool Prepare()
        {
            this.kmpNext = KnuthMorrisPrattNext(this.Pattern);
            return true;
        }

        /// <summary>
        /// The Knuth-Morris-Pratt Next.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// The array of next.
        /// </returns>
        private static int[] KnuthMorrisPrattNext(byte[] pattern)
        {
            var next = new int[pattern.Length];
            next[0] = -1;
            var j = 0;
            var k = -1;

            while (j < pattern.Length - 1)
            {
                if (k == -1)
                {
                    next[++j] = 0;
                    k = 0;
                }
                else if (pattern[j] == pattern[k])
                {
                    next[++j] = ++k;
                }
                else
                {
                    k = next[k];
                }
            }

            return next;
        }
    }
}