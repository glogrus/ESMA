// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForSafe.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Compare
{
    /// <summary>
    ///     The brute force.
    /// </summary>
    [ArrayCompare("ForSafe")]
    public class ForSafe : BaseArrayCompare
    {
        /// <summary>
        /// The array equals.
        /// </summary>
        /// <param name="array1">
        /// The array 1.
        /// </param>
        /// <param name="array2">
        /// The array 2.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool ArrayEquals(byte[] array1, byte[] array2)
        {
            int j;
            for (j = 0; j < array1.Length && array1[j] == array2[j]; j++)
            {
            }

            return j >= array1.Length;
        }
    }
}