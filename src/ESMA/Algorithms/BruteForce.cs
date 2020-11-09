// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BruteForce.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// <summary>
//   The brute force.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Brute Force")]
    public class BruteForce : BaseMatch
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
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        protected override long InternalMatch(byte[] data, List<long> indexes, long offset = 0)
        {
            var patternLength = this.Pattern.Length;
            var length = data.Length - patternLength;
            var pattern = this.Pattern;
            int i;

            for (i = 0; i <= length; i++)
            {
                int j;
                int k;
                for (j = 0, k = i; j < patternLength && pattern[j] == data[k]; j++, k++)
                {
                }

                if (j == patternLength)
                {
                    indexes.Add(i + offset);
                }
            }

            return i;
        }
    }
}