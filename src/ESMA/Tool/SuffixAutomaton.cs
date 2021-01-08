// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuffixAutomaton.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    /// <summary>
    ///     The suffix automaton.
    /// </summary>
    public class SuffixAutomaton
    {
        /// <summary>
        ///     The initial vertex.
        /// </summary>
        public const int InitialVertex = 0;

        /// <summary>
        ///     The undefined.
        /// </summary>
        public const int Undefined = -1;

        /// <summary>
        ///     The size.
        /// </summary>
        private readonly int size;

        /// <summary>
        ///     The vertex.
        /// </summary>
        private int currentVertex = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuffixAutomaton"/> class.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <param name="reverse">
        /// The reverse.
        /// </param>
        public SuffixAutomaton(byte[] pattern, bool reverse)
        {
            this.size = 2 * (pattern.Length + 2);
            this.Target = new int[this.size, 256];
            this.Shift = new int[this.size, 256];
            this.Terminal = new bool[this.size];
            for (var i = 0; i < this.size; i++)
            {
                this.Terminal[i] = false;
                for (var j = 0; j < 256; j++)
                {
                    this.Target[i, j] = Undefined;
                }
            }

            this.BuildSuffixAutomaton(reverse ? Reverse(pattern) : pattern);
        }

        /// <summary>
        ///     Gets the shift.
        /// </summary>
        public int[,] Shift { get; }

        /// <summary>
        ///     Gets the target.
        /// </summary>
        public int[,] Target { get; }

        /// <summary>
        ///     Gets the terminal.
        /// </summary>
        public bool[] Terminal { get; }

        /// <summary>
        ///     The next vertex.
        /// </summary>
        private int NextVertex => this.currentVertex++;

        /// <summary>
        /// The reverse.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <returns>
        /// The reversed byte array.
        /// </returns>
        private static byte[] Reverse(byte[] array)
        {
            var reverse = new byte[array.Length];
            var upper = array.Length - 1;
            for (var i = 0; i < array.Length; i++)
            {
                reverse[i] = array[upper - i];
            }

            return reverse;
        }

        /// <summary>
        /// The build suffix automaton.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        private void BuildSuffixAutomaton(byte[] pattern)
        {
            var suffixLink = new int[this.size];
            var length = new int[this.size];
            var position = new int[this.size];

            var init = InitialVertex;
            var vertex = this.NextVertex;
            suffixLink[init] = vertex;
            var last = init;
            foreach (var ch in pattern)
            {
                var p = last;
                var q = this.NextVertex;
                length[q] = length[p] + 1;
                position[q] = position[p] + 1;
                while (p != init && this.Target[p, ch] == Undefined)
                {
                    this.Target[p, ch] = q;
                    this.Shift[p, ch] = position[q] - position[p] - 1;
                    p = suffixLink[p];
                }

                if (this.Target[p, ch] == Undefined)
                {
                    this.Target[init, ch] = q;
                    this.Shift[init, ch] = position[q] - position[init] - 1;
                    suffixLink[q] = init;
                }
                else
                {
                    if (length[p] + 1 == length[this.Target[p, ch]])
                    {
                        suffixLink[q] = this.Target[p, ch];
                    }
                    else
                    {
                        var r = this.NextVertex;
                        var source = this.Target[p, ch];
                        this.Terminal[r] = this.Terminal[source];
                        suffixLink[r] = suffixLink[source];
                        length[r] = length[source];
                        position[r] = position[source];
                        for (var i = 0; i < 256; i++)
                        {
                            this.Target[r, i] = this.Target[source, i];
                            this.Shift[r, i] = this.Shift[source, i];
                        }

                        length[r] = length[p] + 1;
                        suffixLink[this.Target[p, ch]] = r;
                        suffixLink[q] = r;
                        while (p != vertex && length[this.Target[p, ch]] >= length[r])
                        {
                            this.Shift[p, ch] = position[this.Target[p, ch]] - position[p] - 1;
                            this.Target[p, ch] = r;
                            p = suffixLink[p];
                        }
                    }
                }

                last = q;
            }

            this.Terminal[last] = true;
            while (last != init)
            {
                last = suffixLink[last];
                this.Terminal[last] = true;
            }
        }
    }
}