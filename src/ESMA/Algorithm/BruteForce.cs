// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BruteForce.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
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
            int i;
            for (i = 0; i <= length - pattern.Length; i++)
            {
                int j;
                for (j = 0; j < pattern.Length && pattern[j] == data[i + j]; j++)
                {
                }

                if (j >= pattern.Length)
                {
                    indexes.Add(i + offset);
                }
            }

            return i;
        }
    }
}