// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BruteForceUnsafe.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// <summary>
//   The brute force.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Brute Force Unsafe")]
    public class BruteForceUnsafe : BaseMatch
    {
        /// <summary>
        ///     The internal match.
        /// </summary>
        /// <param name="data">
        ///     The data.
        /// </param>
        /// <param name="indexes">
        ///     The indexes.
        /// </param>
        /// <param name="length">
        ///     The length.
        /// </param>
        /// <param name="offset">
        ///     The offset.
        /// </param>
        /// <returns>
        ///     The <see cref="long" />.
        /// </returns>
        protected override unsafe int InternalMatch(byte[] data, List<long> indexes, int length, long offset = 0)
        {
            var patternLength = this.Pattern.Length;

            fixed (byte* ptrData = data, ptrPattern = this.Pattern)
            {
                var ptrDataSeek = ptrData;
                var ptrDataStop = ptrData + (length - patternLength);
                var ptrPatternStop = ptrPattern + patternLength;
                while (ptrDataSeek <= ptrDataStop)
                {
                    var ptrPatternSeek = ptrPattern;
                    for (;
                        ptrPatternSeek < ptrPatternStop && *ptrPatternSeek == *ptrDataSeek;
                        ptrPatternSeek++, ptrDataSeek++)
                    {
                    }

                    if (ptrPatternSeek == ptrPatternStop)
                    {
                        indexes.Add(ptrDataSeek - ptrData + offset - patternLength);
                    }

                    ptrDataSeek++;
                }

                return (int)(ptrDataSeek - ptrData);
            }
        }
    }
}