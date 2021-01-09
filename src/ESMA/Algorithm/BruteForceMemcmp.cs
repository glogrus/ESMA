// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BruteForceMemcmp.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Brute Force Memcmp")]
    public class BruteForceMemcmp : BaseMatch
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
        protected override unsafe int InternalMatch(byte[] data, List<long> indexes, int length, long offset = 0)
        {
            var patternLength = this.Pattern.Length;

            fixed (byte* ptrData = data, ptrPattern = this.Pattern)
            {
                var ptrDataSeek = ptrData;
                var ptrDataStop = ptrData + (length - patternLength);
                while (ptrDataSeek <= ptrDataStop)
                {
                    if (Native.memcmp(ptrDataSeek, ptrPattern, patternLength) == 0)
                    {
                        indexes.Add(ptrDataSeek - ptrData + offset);
                    }

                    ptrDataSeek++;
                }

                return (int)(ptrDataSeek - ptrData);
            }
        }
    }
}