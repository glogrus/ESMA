// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BruteForceFast.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// <summary>
//   The brute force fast.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    ///     The brute force fast.
    /// </summary>
    [Algorithm("Brute Force Fast")]
    public class BruteForceFast : BaseMatch
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
                var ptrDataStop = ptrData + length - patternLength;
                while (ptrDataSeek <= ptrDataStop)
                {
                    if (UnsafeCompare(ptrDataSeek, ptrPattern, patternLength))
                    {
                        indexes.Add(ptrDataSeek - ptrData + offset);
                    }

                    ptrDataSeek++;
                }

                return (int)(ptrDataSeek - ptrData);
            }
        }

        /// <summary>
        /// The unsafe compare.
        /// </summary>
        /// <param name="p1">
        /// The p 1.
        /// </param>
        /// <param name="p2">
        /// The p 2.
        /// </param>
        /// <param name="l">
        /// The l.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool UnsafeCompare(byte* p1, byte* p2, int l)
        {
            byte* x1 = p1, x2 = p2;
            for (var i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
            {
                if (*(long*)x1 != *(long*)x2)
                {
                    return false;
                }
            }

            if ((l & 4) != 0)
            {
                if (*(int*)x1 != *(int*)x2)
                {
                    return false;
                }

                x1 += 4;
                x2 += 4;
            }

            if ((l & 2) != 0)
            {
                if (*(short*)x1 != *(short*)x2)
                {
                    return false;
                }

                x1 += 2;
                x2 += 2;
            }

            if ((l & 1) == 0)
            {
                return true;
            }

            return *x1 == *x2;
        }
    }
}