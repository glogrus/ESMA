// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericList.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    /// <summary>
    /// The generic list.
    /// </summary>
    /// <typeparam name="T">
    /// Type of list items.
    /// </typeparam>
    public class GenericList<T>
    {
        /// <summary>
        ///     The head.
        /// </summary>
        private Node head;

        /// <summary>
        /// The add node.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        public void AddNode(T node)
        {
            var newNode = new Node { Next = this.head, Data = node };
            this.head = newNode;
        }

        /// <summary>
        ///     The get first added.
        /// </summary>
        /// <returns>
        ///     The <see cref="T" />.
        /// </returns>
        public T GetFirstAdded()
        {
            // The value of temp is returned as the value of the method. 
            // The following declaration initializes temp to the appropriate 
            // default value for type T. The default value is returned if the 
            // list is empty.
            var temp = default(T);

            var current = this.head;
            while (current != null)
            {
                temp = current.Data;
                current = current.Next;
            }

            return temp;
        }

        /// <summary>
        ///     The node.
        /// </summary>
        internal class Node
        {
            /// <summary>
            ///     Gets or sets the data.
            /// </summary>
            public T Data { get; set; }

            /// <summary>
            ///     Gets or sets the next.
            /// </summary>
            public Node Next { get; set; }
        }
    }
}