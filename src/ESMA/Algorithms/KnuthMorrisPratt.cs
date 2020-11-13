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

            var i = 0;
            var j = 0;
            while (i < length)
            {
                if (data[i] == pattern[j])
                {
                    i++;
                    j++;
                }

                if (j == pattern.Length)
                {
                    indexes.Add((i + offset) - j);
                    j = this.kmpNext[j - 1];
                    continue;
                }

                if (i >= length)
                {
                    break;
                }

                if (pattern[j] == data[i])
                {
                    continue;
                }

                if (j != 0)
                {
                    j = this.kmpNext[j - 1];
                }
                else
                {
                    i++;
                }
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
            var j = 0;
            next[0] = 0;

            for (var i = 1; i < pattern.Length; i++)
            {
                if (pattern[i] == pattern[j])
                {
                    j++;
                    next[i] = j;
                }
                else
                {
                    if (j != 0)
                    {
                        j = next[j - 1];
                        i--;
                    }
                    else
                    {
                        next[i] = 0;
                    }
                }
            }

            return next;
        }
    }
}