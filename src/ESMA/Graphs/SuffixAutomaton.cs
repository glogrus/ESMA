// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuffixAutomaton.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Graphs
{
    using System;

    /// <summary>
    ///     The graph.
    /// </summary>
    public sealed class SuffixAutomaton : Automaton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuffixAutomaton"/> class.
        /// </summary>
        /// <param name="vertexCount">
        /// The vertex count.
        /// </param>
        /// <param name="edgeCount">
        /// The edge count.
        /// </param>
        public SuffixAutomaton(int vertexCount, int edgeCount)
            : base(vertexCount, edgeCount)
        {
            for (var i = 0; i < this.EdgeCount; i++)
            {
                this.Target[i] = Undefined;
            }

            this.SuffixLink = new int[this.VertexCount];
            this.Length = new int[this.VertexCount];
            this.Position = new int[this.VertexCount];
            this.Shift = new int[this.EdgeCount];
        }

        /// <summary>
        ///     Gets the length.
        /// </summary>
        public int[] Length { get; }

        /// <summary>
        ///     Gets the position.
        /// </summary>
        public int[] Position { get; }

        /// <summary>
        ///     Gets the suffix link.
        /// </summary>
        private int[] SuffixLink { get; }

        /// <summary>
        ///     Gets the shift.
        /// </summary>
        private int[] Shift { get; }

        /// <summary>
        /// The copy vertex.
        /// </summary>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        public void CopyVertex(int target, int source)
        {
            var size = this.EdgeCount / this.VertexCount;
            var targetStart = target * size;
            var sourceStart = source * size;
            for (var i = 0; i < size; i++)
            {
                this.Target[targetStart + i] = this.Target[sourceStart + i];
                this.Shift[targetStart + i] = this.Shift[sourceStart + i];
            }

            this.Terminal[target] = this.Terminal[source];
            this.SuffixLink[target] = this.SuffixLink[source];
            this.Length[target] = this.Length[source];
            this.Position[target] = this.Position[source];
        }

        /// <summary>
        /// The get shift.
        /// </summary>
        /// <param name="vertex">
        /// The vertex.
        /// </param>
        /// <param name="ch">
        /// The char.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Index of out.
        /// </exception>
        public int GetShift(int vertex, byte ch)
        {
            if (vertex < this.VertexCount && vertex * ch < this.EdgeCount)
            {
                return this.Shift[(vertex * (this.EdgeCount / this.VertexCount)) + ch];
            }

            throw new IndexOutOfRangeException("getShift");
        }

        /// <summary>
        /// The get suffix link.
        /// </summary>
        /// <param name="vertex">
        /// The vertex.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Index of out.
        /// </exception>
        public int GetSuffixLink(int vertex)
        {
            if (vertex < this.VertexCount)
            {
                return this.SuffixLink[vertex];
            }

            throw new IndexOutOfRangeException("getSuffixLink");
        }

        /// <summary>
        /// The set shift.
        /// </summary>
        /// <param name="vertex">
        /// The vertex.
        /// </param>
        /// <param name="ch">
        /// The char.
        /// </param>
        /// <param name="shift">
        /// The shift.
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        /// Index of out.
        /// </exception>
        public void SetShift(int vertex, byte ch, int shift)
        {
            if (vertex < this.VertexCount && vertex * ch <= this.EdgeCount)
            {
                this.Shift[(vertex * (this.EdgeCount / this.VertexCount)) + ch] = shift;
            }
            else
            {
                throw new IndexOutOfRangeException("setShift");
            }
        }

        /// <summary>
        /// The set suffix link.
        /// </summary>
        /// <param name="vertex">
        /// The vertex.
        /// </param>
        /// <param name="suffix">
        /// The suffix.
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        /// Index of out.
        /// </exception>
        public void SetSuffixLink(int vertex, int suffix)
        {
            if (vertex < this.VertexCount && suffix < this.VertexCount)
            {
                this.SuffixLink[vertex] = suffix;
            }
            else
            {
                throw new IndexOutOfRangeException("SetSuffixLink");
            }
        }
    }
}