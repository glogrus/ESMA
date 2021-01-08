// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayCompareAttribute.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    using System;

    /// <summary>
    ///     The match class attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ArrayCompareAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayCompareAttribute"/> class.
        ///     Initializes a new instance of the <see cref="AlgorithmAttribute"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public ArrayCompareAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }
    }
}