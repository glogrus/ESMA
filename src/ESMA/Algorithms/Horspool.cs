// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Horspool.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Horspool")]
    public class Horspool : BaseMatch
    {
        /// <summary>
        ///     The bad s character shift.
        /// </summary>
        private int[] badCharacterShift;

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
            var maxI = length - pattern.Length;
            var lastPatternIndex = pattern.Length - 1;
            var lastPatternByte = pattern[lastPatternIndex];
            while (i <= maxI)
            {
                var c = data[i + lastPatternIndex];
                if (c == lastPatternByte)
                {
                    int j;
                    for (j = pattern.Length - 1; j >= 0 && pattern[j] == data[i + j]; j--)
                    {
                    }

                    if (j < 0)
                    {
                        indexes.Add(i + offset);
                    }
                }

                i += this.badCharacterShift[c];
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
            this.badCharacterShift = BoyerMoore.BoyerMooreBadCharacterShift(this.Pattern);
            return true;
        }
    }
}