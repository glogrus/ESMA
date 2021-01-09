// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TurboBoyerMoore.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Turbo Boyer-Moore")]
    public class TurboBoyerMoore : BaseMatch
    {
        /// <summary>
        ///     The bad s character shift.
        /// </summary>
        private int[] badCharacterShift;

        /// <summary>
        ///     The good suffixes.
        /// </summary>
        private int[] goodSuffixes;

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
            var u = 0;
            var shift = pattern.Length;
            while (i <= length - pattern.Length)
            {
                var j = pattern.Length - 1;
                while (j >= 0 && pattern[j] == data[i + j])
                {
                    j--;
                    if (u != 0 && j == pattern.Length - 1 - shift)
                    {
                        j -= u;
                    }
                }

                if (j < 0)
                {
                    indexes.Add(i + offset);
                    shift = this.goodSuffixes[0];
                    u = pattern.Length - shift;
                    i += shift;
                    continue;
                }

                var v = pattern.Length - 1 - j;
                var turboShift = u - v;
                var badCharShift = (this.badCharacterShift[data[i + j]] - pattern.Length) + 1 + j;
                shift = turboShift > badCharShift ? turboShift : badCharShift;
                shift = shift > this.goodSuffixes[j] ? shift : this.goodSuffixes[j];
                if (shift == this.goodSuffixes[j])
                {
                    u = pattern.Length - shift < v ? pattern.Length - shift : v;
                }
                else
                {
                    if (turboShift < badCharShift)
                    {
                        shift = shift > u + 1 ? shift : u + 1;
                    }

                    u = 0;
                }

                i += shift;
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
            this.goodSuffixes = BoyerMoore.BoyerMooreGoodSuffixes(this.Pattern);
            this.badCharacterShift = BoyerMoore.BoyerMooreBadCharacterShift(this.Pattern);
            return true;
        }
    }
}