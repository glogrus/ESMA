// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Automaton.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Graphs
{
    using System;

    /// <summary>
    ///     The graph.
    /// </summary>
    public class Automaton : Graph
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Automaton"/> class.
        /// </summary>
        /// <param name="vertexCount">
        /// The vertex count.
        /// </param>
        /// <param name="edgeCount">
        /// The edge count.
        /// </param>
        protected Automaton(int vertexCount, int edgeCount)
            : base(vertexCount, edgeCount)
        {
            this.Target = new int[this.EdgeCount];
            this.Terminal = new int[this.VertexCount];
        }

        /// <summary>
        ///     Gets the terminal.
        /// </summary>
        public int[] Terminal { get; }

        /// <summary>
        ///     Gets the target.
        /// </summary>
        protected int[] Target { get; }

        /// <summary>
        /// The get target.
        /// </summary>
        /// <param name="vertex">
        /// The vertex.
        /// </param>
        /// <param name="ch">
        /// The byte.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public int GetTarget(int vertex, byte ch)
        {
            return this.Target[(vertex * (this.EdgeCount / this.VertexCount)) + ch];
        }

        /// <summary>
        /// The is terminal.
        /// </summary>
        /// <param name="vertex">
        /// The vertex.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsTerminal(int vertex)
        {
            return this.Terminal[vertex] != 0;
        }

        /// <summary>
        /// The set target.
        /// </summary>
        /// <param name="vertex">
        /// The vertex.
        /// </param>
        /// <param name="ch">
        /// The byte.
        /// </param>
        /// <param name="target">
        /// The target.
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        /// Index of out.
        /// </exception>
        public void SetTarget(int vertex, byte ch, int target)
        {
            this.Target[(vertex * (this.EdgeCount / this.VertexCount)) + ch] = target;
        }
    }
}