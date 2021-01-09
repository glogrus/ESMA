// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayCompareEnumerator.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ESMA.Compare;

    /// <summary>
    ///     The reflective enumerator.
    /// </summary>
    public static class ArrayCompareEnumerator
    {
        /// <summary>
        ///     The lazy algorithms.
        /// </summary>
        private static readonly Lazy<SortedDictionary<string, BaseArrayCompare>> LazyAlgorithms =
            new Lazy<SortedDictionary<string, BaseArrayCompare>>(GetAlgorithms);

        /// <summary>
        ///     Gets the algorithms.
        /// </summary>
        public static SortedDictionary<string, BaseArrayCompare> Algorithms => LazyAlgorithms.Value;

        /// <summary>
        /// The get class.
        /// </summary>
        /// <param name="algorithm">
        /// The algorithm.
        /// </param>
        /// <returns>
        /// The <see cref="BaseMatch"/>.
        /// </returns>
        public static BaseArrayCompare GetClass(string algorithm)
        {
            return Algorithms.TryGetValue(algorithm, out var value) ? value : new ForSafe();
        }

        /// <summary>
        ///     The get algorithms.
        /// </summary>
        /// <returns>
        ///     The SortedDictionary of BaseMatch.
        /// </returns>
        private static SortedDictionary<string, BaseArrayCompare> GetAlgorithms()
        {
            var objects = new SortedDictionary<string, BaseArrayCompare>();
            var list = Assembly.GetAssembly(typeof(BaseArrayCompare));
            if (list == null)
            {
                return objects;
            }

            foreach (var type in list.GetTypes().Where(
                myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BaseArrayCompare))))
            {
                var algorithm =
                    (ArrayCompareAttribute)Attribute.GetCustomAttribute(type, typeof(ArrayCompareAttribute));

                objects.Add(
                    algorithm == null ? type.Name : algorithm.Name,
                    (BaseArrayCompare)Activator.CreateInstance(type));
            }

            return objects;
        }
    }
}