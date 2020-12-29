// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TwoWay.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The Two Way.
    /// </summary>
    [Algorithm("Two Way")]
    public class TwoWay : BaseMatch
    {
        /// <summary>
        ///     The eliminator.
        /// </summary>
        private int eliminator;

        /// <summary>
        ///     The period.
        /// </summary>
        private int period;

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
            var patternLength = pattern.Length;
            var ell = this.eliminator;
            var per = this.period;

            int j, i;
            for (j = 0; j < ell + 1 && pattern[j] == pattern[j + per]; j++)
            {
            }

            if (j == ell + 1)
            {
                i = 0;
                var memory = -1;
                while (i <= length - pattern.Length)
                {
                    j = (ell > memory ? ell : memory) + 1;
                    while (j < pattern.Length && pattern[j] == data[i + j])
                    {
                        j++;
                    }

                    if (j >= pattern.Length)
                    {
                        j = ell;
                        while (j > memory && pattern[j] == data[i + j])
                        {
                            j--;
                        }

                        if (j <= memory)
                        {
                            indexes.Add(i + offset);
                        }

                        i += per;
                        memory = pattern.Length - per - 1;
                    }
                    else
                    {
                        i += j - ell;
                        memory = -1;
                    }
                }
            }
            else
            {
                per = (ell + 1 > pattern.Length - ell - 1 ? ell + 1 : pattern.Length - ell - 1) + 1;
                i = 0;
                while (i <= length - pattern.Length)
                {
                    j = ell + 1;
                    while (j < pattern.Length && pattern[j] == data[i + j])
                    {
                        j++;
                    }

                    if (j >= pattern.Length)
                    {
                        j = ell;
                        while (j >= 0 && pattern[j] == data[i + j])
                        {
                            j--;
                        }

                        if (j < 0)
                        {
                            indexes.Add(i + offset);
                        }

                        i += per;
                    }
                    else
                    {
                        i += j - ell;
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
            var i = this.MaxSuffix(this.Pattern, out var p);
            var j = this.MaxSuffixTilde(this.Pattern, out var q);
            if (i > j)
            {
                this.eliminator = i;
                this.period = p;
            }
            else
            {
                this.eliminator = j;
                this.period = q;
            }

            return true;
        }

        /// <summary>
        /// Computing of the maximal suffix for LE
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int MaxSuffix(byte[] x, out int p)
        {
            var ms = -1;
            var j = 0;
            var k = p = 1;
            while (j + k < x.Length)
            {
                var a = x[j + k];
                var b = x[ms + k];
                if (a < b)
                {
                    j += k;
                    k = 1;
                    p = j - ms;
                }
                else if (a == b)
                {
                    if (k != p)
                    {
                        k++;
                    }
                    else
                    {
                        j += p;
                        k = 1;
                    }
                }
                else
                {
                    ms = j;
                    j = ms + 1;
                    k = p = 1;
                }
            }

            return ms;
        }

        /// <summary>
        /// Computing of the maximal suffix for GE
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="p">
        /// The p.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private int MaxSuffixTilde(byte[] x, out int p)
        {
            var ms = -1;
            var j = 0;
            var k = p = 1;
            while (j + k < x.Length)
            {
                var a = x[j + k];
                var b = x[ms + k];
                if (a > b)
                {
                    j += k;
                    k = 1;
                    p = j - ms;
                }
                else
                {
                    if (a == b)
                    {
                        if (k != p)
                        {
                            k++;
                        }
                        else
                        {
                            j += p;
                            k = 1;
                        }
                    }
                    else
                    {
                        ms = j;
                        j = ms + 1;
                        k = p = 1;
                    }
                }
            }

            return ms;
        }
    }
}