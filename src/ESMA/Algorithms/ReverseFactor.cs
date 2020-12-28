// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReverseFactor.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The Reverse Factor.
    /// </summary>
    [Algorithm("Reverse Factor")]
    public class ReverseFactor : BaseMatch
    {
        /// <summary>
        ///     The automaton.
        /// </summary>
        private SuffixAutomaton automaton;

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
            var am = this.automaton;
            var period = pattern.Length;

            var i = 0;
            while (i <= length - pattern.Length)
            {
                var j = pattern.Length - 1;
                var vertex = SuffixAutomaton.InitialVertex;
                var shift = pattern.Length;
                while (i + j >= 0)
                {
                    vertex = am.Target[vertex, data[i + j]];
                    if (vertex == SuffixAutomaton.Undefined)
                    {
                        break;
                    }

                    if (am.Terminal[vertex])
                    {
                        period = shift;
                        shift = j;
                    }

                    j--;
                }

                if (j < 0)
                {
                    indexes.Add(i + offset);
                    shift = period;
                }

                i += shift;
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
            this.automaton = new SuffixAutomaton(this.Pattern, true);
            return true;
        }
    }
}