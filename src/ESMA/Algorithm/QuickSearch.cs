// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuickSearch.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Quick Search")]
    public class QuickSearch : BaseMatch
    {
        /// <summary>
        ///     The bad s character shift.
        /// </summary>
        private int[] badSCharacterShift;

        /// <summary>
        /// The Quick Search Bad shifts.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// The array of Bad shifts.
        /// </returns>
        internal static int[] QuickSearchBadSCharacterShift(byte[] pattern)
        {
            var bcs = new int[256];

            for (var i = 0; i < 256; ++i)
            {
                bcs[i] = pattern.Length + 1;
            }

            for (var i = 0; i < pattern.Length; i++)
            {
                bcs[pattern[i]] = pattern.Length - i;
            }

            return bcs;
        }

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
        /// The <see cref="long"/>.
        /// </returns>
        protected override int InternalMatch(byte[] data, List<long> indexes, int length, long offset = 0)
        {
            var pattern = this.Pattern;
            var i = 0;

            while (i <= length - pattern.Length)
            {
                int j;
                for (j = 0; j < pattern.Length && i < length && pattern[j] == data[i]; j++, i++)
                {
                }

                if (j >= pattern.Length)
                {
                    indexes.Add(i + offset - pattern.Length);
                    continue;
                }

                if (i + pattern.Length >= length)
                {
                    break;
                }

                i += this.badSCharacterShift[data[i + pattern.Length]];
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
            this.badSCharacterShift = QuickSearchBadSCharacterShift(this.Pattern);
            return true;
        }
    }
}