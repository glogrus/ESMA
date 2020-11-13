// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Boyer-Moore.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Boyer-Moore")]
    public class BoyerMoore : BaseMatch
    {
        /// <summary>
        /// The bad s character shift.
        /// </summary>
        private int[] badSCharacterShift;

        /// <summary>
        /// The good suffixes.
        /// </summary>
        private int[] goodSuffixes;

        /// <summary>
        /// The Boyer Moore Bad shifts.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// The <see cref="int[]"/>.
        /// </returns>
        internal static int[] BoyerMooreBadSCharacterShift(byte[] pattern)
        {
            var bcs = new int[256];
            for (var i = 0; i < 256; ++i)
            {
                bcs[i] = pattern.Length;
            }

            for (var i = 0; i < pattern.Length - 1; i++)
            {
                bcs[pattern[i]] = pattern.Length - i - 1;
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
                for (j = pattern.Length - 1; j >= 0 && pattern[j] == data[i + j]; j--)
                {
                }

                if (j < 0)
                {
                    indexes.Add(i + offset);
                    i += this.goodSuffixes[0];
                }
                else
                {
                    var i1 = this.goodSuffixes[j];
                    var i2 = (this.badSCharacterShift[data[i + j]] - pattern.Length) + 1 + j;
                    i += i1 > i2 ? i1 : i2;
                }
            }

            return i;
        }

        /// <summary>
        /// The prepare.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool Prepare()
        {
            this.goodSuffixes = BoyerMooreGoodSuffixes(this.Pattern);
            this.badSCharacterShift = BoyerMooreBadSCharacterShift(this.Pattern);
            return true;
        }

        /// <summary>
        /// The Boyer-Moore Good Suffixes.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// The Good Suffixes shift.
        /// </returns>
        private static int[] BoyerMooreGoodSuffixes(byte[] pattern)
        {
            var gs = new int[pattern.Length];
            var suffixes = BoyerMooreSuffixes(pattern);

            for (var i = 0; i < pattern.Length; i++)
            {
                gs[i] = pattern.Length;
            }

            var j = 0;
            for (var i = pattern.Length - 1; i >= 0; i--)
            {
                if (suffixes[i] != i + 1)
                {
                    continue;
                }

                for (; j < pattern.Length - 1 - i; ++j)
                {
                    if (gs[j] == pattern.Length)
                    {
                        gs[j] = pattern.Length - 1 - i;
                    }
                }
            }

            for (var i = 0; i <= pattern.Length - 2; ++i)
            {
                gs[pattern.Length - 1 - suffixes[i]] = pattern.Length - 1 - i;
            }

            return gs;
        }

        /// <summary>
        /// The suffixes.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// The Boyer Moore Suffixes.
        /// </returns>
        private static int[] BoyerMooreSuffixes(byte[] pattern)
        {
            var suffixes = new int[pattern.Length];
            suffixes[pattern.Length - 1] = pattern.Length;
            var g = pattern.Length - 1;
            for (var i = pattern.Length - 2; i >= 0; i--)
            {
                var f = 0;
                if (i > g && suffixes[(i + pattern.Length) - 1 - f] < i - g)
                {
                    suffixes[i] = suffixes[(i + pattern.Length) - 1 - f];
                }
                else
                {
                    if (i < g)
                    {
                        g = i;
                    }

                    f = i;
                    while (g >= 0 && pattern[g] == pattern[(g + pattern.Length) - 1 - f])
                    {
                        --g;
                    }

                    suffixes[i] = f - g;
                }
            }

            return suffixes;
        }
    }
}