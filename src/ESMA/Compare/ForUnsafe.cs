// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForUnsafe.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Compare
{
    /// <summary>
    ///     The brute force.
    /// </summary>
    [ArrayCompare("ForUnsafe")]
    public class ForUnsafe : BaseArrayCompare
    {
        /// <summary>
        ///     The array equals.
        /// </summary>
        /// <param name="array1">
        ///     The array 1.
        /// </param>
        /// <param name="array2">
        ///     The array 2.
        /// </param>
        /// <returns>
        ///     The <see cref="bool" />.
        /// </returns>
        public override unsafe bool ArrayEquals(byte[] array1, byte[] array2)
        {
            fixed (byte* a1 = array1, a2 = array2)
            {
                var stop = a1 + array1.Length;
                var ptr1 = a1;
                var ptr2 = a2;
                for (; ptr1 < stop && *ptr1 == *ptr2; ptr1++, ptr2++)
                {
                }

                return ptr1 == stop;
            }
        }
    }
}