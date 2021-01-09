// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotSoNaive.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Not So Naive")]
    public class NotSoNaive : BaseMatch
    {
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

            int diff, equal;

            if (pattern[0] == pattern[1])
            {
                diff = 2;
                equal = 1;
            }
            else
            {
                diff = 1;
                equal = 2;
            }
           
            var i = 0;
            while (i <= length - pattern.Length)
            {
                if (pattern[1] != data[i + 1])
                {
                    i += diff;
                }
                else
                {
                    int j;
                    for (j = 1; j < pattern.Length && pattern[j] == data[i + j]; j++)
                    {
                    }

                    if (j >= pattern.Length && pattern[0] == data[i])
                    {
                        indexes.Add(i + offset);
                        i += j - 1;
                    }

                    i += equal;
                }
            }

            return i;
        }
    }
}