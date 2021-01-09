// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TurboReverseFactor.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithm
{
    using System.Collections.Generic;

    /// <summary>
    ///     The Turbo Reverse Factor.
    /// </summary>
    [Algorithm("Turbo Reverse Factor")]
    public class TurboReverseFactor : BaseMatch
    {
        /// <summary>
        ///     The algorithm next.
        /// </summary>
        private int[] algorithmNext;

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
            var init = SuffixAutomaton.InitialVertex;
            var shift = pattern.Length;
            var i = 0;

            while (i <= length - pattern.Length)
            {
                var j = pattern.Length - 1;
                var state = init;
                var u = pattern.Length - 1 - shift;
                var periodOfU = shift != pattern.Length
                                    ? pattern.Length - shift - this.algorithmNext[pattern.Length - shift]
                                    : 0;
                shift = pattern.Length;
                var displacement = 0;
                while (j > u && am.Target[state, data[i + j]] != SuffixAutomaton.Undefined)
                {
                    displacement += am.Shift[state, data[i + j]];
                    state = am.Target[state, data[i + j]];
                    if (am.Terminal[state])
                    {
                        shift = j;
                    }

                    j--;
                }

                if (j <= u)
                {
                    if (displacement == 0)
                    {
                        indexes.Add(i + offset);
                        shift = period;
                    }
                    else
                    {
                        var mu = (u + 1) / 2;
                        if (periodOfU <= mu)
                        {
                            u -= periodOfU;
                            while (i > u && am.Target[state, data[i + j]] != SuffixAutomaton.Undefined)
                            {
                                displacement += am.Shift[state, data[i + j]];
                                state = am.Target[state, data[i + j]];
                                if (am.Terminal[state])
                                {
                                    shift = j;
                                }

                                j--;
                            }

                            if (j <= u)
                            {
                                shift = displacement;
                            }
                        }
                        else
                        {
                            u = u - mu - 1;
                            while (i > u && am.Target[state, data[i + j]] != SuffixAutomaton.Undefined)
                            {
                                displacement += am.Shift[state, data[i + j]];
                                state = am.Target[state, data[i + j]];
                                if (am.Terminal[state])
                                {
                                    shift = j;
                                }

                                j--;
                            }
                        }
                    }
                }

                i += shift;
            }

            while (i <= length - pattern.Length)
            {
                var j = pattern.Length - 1;
                var vertex = SuffixAutomaton.InitialVertex;
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
            this.algorithmNext = MorrisPratt.MorrisPrattNext(this.Pattern);
            return true;
        }
    }
}