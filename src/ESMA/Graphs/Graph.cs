// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Graph.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Graphs
{
    /// <summary>
    ///     The graph.
    /// </summary>
    public class Graph
    {
        /// <summary>
        ///     The undefined.
        /// </summary>
        public const int Undefined = -1;

        /// <summary>
        ///     The vertex.
        /// </summary>
        private int vertex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="vertexCount">
        /// The vertex count.
        /// </param>
        /// <param name="edgeCount">
        /// The edge count.
        /// </param>
        protected Graph(int vertexCount, int edgeCount)
        {
            this.VertexCount = vertexCount;
            this.EdgeCount = edgeCount;
            this.InitialVertex = 0;
            this.vertex = 1;
        }

        /// <summary>
        ///     Gets the initial.
        /// </summary>
        public int InitialVertex { get; }

        /// <summary>
        ///     Gets the vertex number.
        /// </summary>
        public int NextVertex => this.vertex++;

        /// <summary>
        ///     Gets the edge number.
        /// </summary>
        protected int EdgeCount { get; }

        /// <summary>
        ///     Gets the vertex counter.
        /// </summary>
        protected int VertexCount { get; }
    }
}