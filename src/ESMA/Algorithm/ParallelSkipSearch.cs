// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParallelSkipSearch.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    ///     The Parallel Skip Search.
    /// </summary>
    [Algorithm("Parallel Skip Search")]
    public class ParallelSkipSearch : BaseMatch
    {
        /// <summary>
        ///     The skip search list.
        /// </summary>
        private GenericList<int>.Node[] skipSearchList;

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
        /// The array of index.
        /// </returns>
        protected override int InternalMatch(byte[] data, List<long> indexes, int length, long offset = 0)
        {
            var pattern = this.Pattern;
            var patternLength = pattern.Length;
            var list = this.skipSearchList;

            Parallel.ForEach(
                Iterator(pattern.Length - 1, length, patternLength),
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 2 },
                i =>
                {
                    for (var node = list[data[i]]; node != null; node = node.Next)
                    {
                        int j;
                        var index = i - node.Data;
                        for (j = 0; j < patternLength && index + j < length && pattern[j] == data[index + j]; j++)
                        {
                        }

                        if (j >= patternLength)
                        {
                            indexes.Add(index + offset);
                        }
                    }
                });

            return (((length / patternLength) + 1) * patternLength) - 1;
        }

        /// <summary>
        ///     The prepare.
        /// </summary>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        protected override bool Prepare()
        {
            this.skipSearchList = new GenericList<int>.Node[256];
            for (var i = 0; i < this.Pattern.Length; i++)
            {
                this.skipSearchList[this.Pattern[i]] =
                    new GenericList<int>.Node { Data = i, Next = this.skipSearchList[this.Pattern[i]] };
            }

            return true;
        }

        /// <summary>
        /// The iterator.
        /// </summary>
        /// <param name="startIndex">
        /// The start index.
        /// </param>
        /// <param name="endIndex">
        /// The end index.
        /// </param>
        /// <param name="stepSize">
        /// The step size.
        /// </param>
        /// <returns>
        /// The Iterator with steps.
        /// </returns>
        private static IEnumerable<int> Iterator(int startIndex, int endIndex, int stepSize)
        {
            for (var i = startIndex; i < endIndex; i += stepSize)
            {
                yield return i;
            }
        }
    }
}