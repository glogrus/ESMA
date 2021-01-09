// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Smith.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The Smith.
    /// </summary>
    [Algorithm("Smith")]
    public class Smith : BaseMatch
    {
        /// <summary>
        ///     The Boyer-Moore bad character shift.
        /// </summary>
        private int[] bmBcs;

        /// <summary>
        ///     The Quick Search bad character shift.
        /// </summary>
        private int[] qsBcs;

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
            var patternLength = pattern.Length;
            var patternLengthMinusOne = patternLength - 1;

            var i = 0;
            while (i <= length - patternLength)
            {
                int j;
                for (j = 0; j < patternLength && pattern[j] == data[i + j]; j++)
                {
                }

                if (j >= patternLength)
                {
                    indexes.Add(i + offset);
                }

                var i1 = this.bmBcs[data[i + patternLengthMinusOne]];
                var i2 = i + patternLength >= length ? 1 : this.qsBcs[data[i + patternLength]];
                i += i1 > i2 ? i1 : i2;
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
            this.qsBcs = QuickSearch.QuickSearchBadSCharacterShift(this.Pattern);
            this.bmBcs = BoyerMoore.BoyerMooreBadCharacterShift(this.Pattern);
            return true;
        }
    }
}