// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseArrayCompare.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    using System;

    /// <summary>
    ///     The Searcher interface.
    /// </summary>
    public abstract class BaseArrayCompare
    {
        /// <summary>
        /// The compare.
        /// </summary>
        /// <param name="array1">
        /// The array 1.
        /// </param>
        /// <param name="array2">
        /// The array 2.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public abstract bool ArrayEquals(byte[] array1, byte[] array2);

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            var algorithm = (AlgorithmAttribute)Attribute.GetCustomAttribute(
                this.GetType(),
                typeof(AlgorithmAttribute));

            return algorithm == null ? this.GetType().Name : algorithm.Name;
        }
    }
}