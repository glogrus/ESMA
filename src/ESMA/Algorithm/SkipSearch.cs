// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkipSearch.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    ///     The Skip Search.
    /// </summary>
    [Algorithm("Skip Search")]
    public class SkipSearch : BaseMatch
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

            int i;
            for (i = pattern.Length - 1; i < length; i += patternLength)
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
            }

            return i > length ? i - patternLength - 1 : i;
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
    }
}