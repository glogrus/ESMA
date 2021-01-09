// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AlgorithmEnumerator.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using ESMA.Algorithm;

    /// <summary>
    ///     The reflective enumerator.
    /// </summary>
    public static class AlgorithmEnumerator
    {
        /// <summary>
        ///     The lazy algorithms.
        /// </summary>
        private static readonly Lazy<SortedDictionary<string, BaseMatch>> LazyAlgorithms =
            new Lazy<SortedDictionary<string, BaseMatch>>(GetAlgorithms);

        /// <summary>
        ///     Gets the algorithms.
        /// </summary>
        public static SortedDictionary<string, BaseMatch> Algorithms => LazyAlgorithms.Value;

        /// <summary>
        /// The get class.
        /// </summary>
        /// <param name="algorithm">
        /// The algorithm.
        /// </param>
        /// <returns>
        /// The <see cref="BaseMatch"/>.
        /// </returns>
        public static BaseMatch GetClass(string algorithm)
        {
            return Algorithms.TryGetValue(algorithm, out var value) ? value : new BruteForce();
        }

        /// <summary>
        ///     The get algorithms.
        /// </summary>
        /// <returns>
        ///     The SortedDictionary of BaseMatch.
        /// </returns>
        private static SortedDictionary<string, BaseMatch> GetAlgorithms()
        {
            var objects = new SortedDictionary<string, BaseMatch>();
            var list = Assembly.GetAssembly(typeof(BaseMatch));
            if (list == null)
            {
                return objects;
            }

            foreach (var type in list.GetTypes().Where(
                myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(BaseMatch))))
            {
                var algorithm = (AlgorithmAttribute)Attribute.GetCustomAttribute(type, typeof(AlgorithmAttribute));

                objects.Add(algorithm == null ? type.Name : algorithm.Name, (BaseMatch)Activator.CreateInstance(type));
            }

            return objects;
        }
    }
}