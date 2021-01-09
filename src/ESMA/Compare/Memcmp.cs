// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Memcmp.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// <summary>
//   The brute force.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Compare
{
    /// <summary>
    ///     The brute force.
    /// </summary>
    [ArrayCompare("Memcmp")]
    public class Memcmp : BaseArrayCompare
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
            fixed (byte* ptrArray1 = array1, ptrArray2 = array2)
            {
                return Native.memcmp(ptrArray1, ptrArray2, array1.Length) == 0;
            }
        }
    }
}