// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BruteSmart.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Brute Smart")]
    public class BruteSmart : BaseMatch
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
            var i = 0;
            var p0 = pattern[0];
            var last = length - pattern.Length;
            while (i <= last)
            {
                for (; i < last && p0 != data[i]; i++)
                {
                }

                int j;
                for (j = 1; j < pattern.Length && pattern[j] == data[i + j]; j++)
                {
                }

                if (j >= pattern.Length)
                {
                    indexes.Add(i + offset);
                }

                i++;
                if (i == last)
                {
                    break;
                }
            }

            return i;
        }
    }
}