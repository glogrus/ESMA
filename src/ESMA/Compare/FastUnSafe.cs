// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FastUnSafe.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Compare
{
    /// <summary>
    ///     The brute force.
    /// </summary>
    [ArrayCompare("FastUnsafe")]
    public class FastUnSafe : BaseArrayCompare
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
            fixed (byte* bytes1 = array1, bytes2 = array2)
            {
                var len = array1.Length;
                var rem = len % (sizeof(long) * 16);
                var b1 = (long*)bytes1;
                var b2 = (long*)bytes2;
                var e1 = (long*)(bytes1 + len - rem);

                while (b1 < e1)
                {
                    if (*b1 != *b2 || *(b1 + 1) != *(b2 + 1) || *(b1 + 2) != *(b2 + 2) || *(b1 + 3) != *(b2 + 3)
                        || *(b1 + 4) != *(b2 + 4) || *(b1 + 5) != *(b2 + 5) || *(b1 + 6) != *(b2 + 6)
                        || *(b1 + 7) != *(b2 + 7) || *(b1 + 8) != *(b2 + 8) || *(b1 + 9) != *(b2 + 9)
                        || *(b1 + 10) != *(b2 + 10) || *(b1 + 11) != *(b2 + 11) || *(b1 + 12) != *(b2 + 12)
                        || *(b1 + 13) != *(b2 + 13) || *(b1 + 14) != *(b2 + 14) || *(b1 + 15) != *(b2 + 15))
                    {
                        return false;
                    }

                    b1 += 16;
                    b2 += 16;
                }

                for (var i = 0; i < rem; i++)
                {
                    if (array1[len - 1 - i] != array2[len - 1 - i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}