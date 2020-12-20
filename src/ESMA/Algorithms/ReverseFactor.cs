// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReverseFactor.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    using ESMA.Graphs;

    /// <summary>
    ///     The Morris-Pratt.
    /// </summary>
    [Algorithm("Reverse Factor")]
    public class ReverseFactor : BaseMatch
    {
        /// <summary>
        ///     The automat.
        /// </summary>
        private SuffixAutomaton automat;

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
            var period = pattern.Length;

            var i = 0;
            while (i <= length - pattern.Length)
            {
                var j = pattern.Length - 1;
                var vertex = this.automat.InitialVertex;
                var shift = pattern.Length;
                while (i + j >= 0 && this.automat.GetTarget(vertex, data[i + j]) != Graph.Undefined)
                {
                    vertex = this.automat.GetTarget(vertex, data[i + j]);
                    if (this.automat.IsTerminal(vertex))
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
            this.automat = new SuffixAutomaton(2 * (this.Pattern.Length + 2), 2 * (this.Pattern.Length + 2) * 256);
            var reverse = new byte[this.Pattern.Length];
            for (var i = 0; i < this.Pattern.Length; i++)
            {
                reverse[i] = this.Pattern[this.Pattern.Length - i - 1];
            }

            BuildSuffixAutomaton(reverse, this.automat);
            return true;
        }

        /// <summary>
        /// The build suffix automaton.
        /// </summary>
        /// <param name="reverse">
        /// The reverse.
        /// </param>
        /// <param name="automat">
        /// The automat.
        /// </param>
        private static void BuildSuffixAutomaton(byte[] reverse, SuffixAutomaton automat)
        {
            var init = automat.InitialVertex;
            var vertex = automat.NextVertex;
            automat.SetSuffixLink(init, vertex);
            var last = init;
            for (var i = 0; i < reverse.Length; i++)
            {
                var ch = reverse[i];
                var p = last;
                var q = automat.NextVertex;
                automat.Length[q] = automat.Length[p] + 1;
                automat.Position[q] = automat.Position[p] + 1;
                while (p != init && automat.GetTarget(p, ch) == Graph.Undefined)
                {
                    automat.SetTarget(p, ch, q);
                    automat.SetShift(p, ch, automat.Position[q] - automat.Position[p] - 1);
                    p = automat.GetSuffixLink(p);
                }

                if (automat.GetTarget(p, ch) == Graph.Undefined)
                {
                    automat.SetTarget(init, ch, q);
                    automat.SetShift(init, ch, automat.Position[q] - automat.Position[init] - 1);
                    automat.SetSuffixLink(q, init);
                }
                else
                {
                    if (automat.Length[p] + 1 == automat.Length[automat.GetTarget(p, ch)])
                    {
                        automat.SetSuffixLink(q, automat.GetTarget(p, ch));
                    }
                    else
                    {
                        var r = automat.NextVertex;
                        automat.CopyVertex(r, automat.GetTarget(p, ch));
                        automat.Length[r] = automat.Length[p] + 1;
                        automat.SetSuffixLink(automat.GetTarget(p, ch), r);
                        automat.SetSuffixLink(q, r);
                        while (p != vertex && automat.Length[automat.GetTarget(p, ch)] >= automat.Length[r])
                        {
                            automat.SetShift(
                                p,
                                ch,
                                automat.Position[automat.GetTarget(p, ch)] - automat.Position[p] - 1);
                            automat.SetTarget(p, ch, r);
                            p = automat.GetSuffixLink(p);
                        }
                    }
                }

                last = q;
            }

            automat.Terminal[last] = 1;
            while (last != init)
            {
                last = automat.GetSuffixLink(last);
                automat.Terminal[last] = 1;
            }
        }
    }
}