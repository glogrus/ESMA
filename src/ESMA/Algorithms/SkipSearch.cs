// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SkipSearch.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

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
            var list = this.skipSearchList;

            var i = pattern.Length - 1;
            for (; i < length; i += this.Pattern.Length)
            {
                for (var node = list[data[i]]; node != null; node = node.Next)
                {
                    int j;
                    for (j = 0; j < pattern.Length; j++)
                    {
                        var index = i + j - node.Data;
                        if (index >= data.Length)
                        {
                            break;
                        }

                        if (pattern[j] != data[index])
                        {
                            break;
                        }
                    }

                    if (j >= pattern.Length)
                    {
                        indexes.Add(i - node.Data + offset);
                    }
                }
            }

            return i;
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