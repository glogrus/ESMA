namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Brute Force Memcmp")]
    public class BruteForceMemcmp : BaseMatch
    {
        /// <summary>
        ///     The internal match.
        /// </summary>
        /// <param name="data">
        ///     The data.
        /// </param>
        /// <param name="indexes">
        ///     The indexes.
        /// </param>
        /// <param name="offset">
        ///     The offset.
        /// </param>
        /// <returns>
        ///     The <see cref="int" />.
        /// </returns>
        protected override unsafe long InternalMatch(byte[] data, List<long> indexes, long offset = 0)
        {
            var patternLength = this.Pattern.Length;

            fixed (byte* ptrData = data, ptrPattern = this.Pattern)
            {
                var ptrDataSeek = ptrData;
                var ptrDataStop = ptrData + (data.Length - patternLength);
                while (ptrDataSeek <= ptrDataStop)
                {
                    if (Native.memcmp(ptrDataSeek, ptrPattern, patternLength) == 0)
                    {
                        indexes.Add((ptrDataSeek - ptrData) + offset);
                    }

                    ptrDataSeek++;
                }

                return ptrDataSeek - ptrData;
            }
        }
    }
}