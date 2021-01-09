// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KnuthMorrisPratt.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
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
            var next = this.kmpNext;
            var last = pattern.Length - 1;

            var i = 0;
            var j = 0;
            while (i < length)
            {
                while (j > -1 && data[i] != pattern[j])
                {
                    j = next[j];
                }

                if (j >= last)
                {
                    indexes.Add((i - j) + offset);
                    j = next[j];
                }

                i++;
                j++;
            }

            return i - j;
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
            var i = 0;
            var j = next[0] = -1;
            while (i < pattern.Length)
            {
                while (j > -1 && pattern[i] != pattern[j])
                {
                    j = next[j];
                }

                i++;
                j++;
                if (i >= pattern.Length)
                {
                    break;
                }

                next[i] = pattern[i] == pattern[j] ? next[j] : j;
            }

            return next;
        }
    }
}