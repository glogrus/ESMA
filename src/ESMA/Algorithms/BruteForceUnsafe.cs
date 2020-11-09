namespace ESMA.Algorithms
{
    using System.Collections.Generic;

    /// <summary>
    ///     The brute force.
    /// </summary>
    [Algorithm("Brute Force Unsafe")]
    public class BruteForceUnsafe : BaseMatch
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
            var i = 0;
            var limit = data.Length - this.Pattern.Length;

            fixed (byte* ptrData = data, ptrPattern = this.Pattern)
            {
                while (i <= limit)
                {
                    {
                        int j;
                        var ptrPatternSeek = ptrPattern;
                        var ptrDataSeek = ptrData + i;
                        for (j = 0;
                             j < patternLength && *ptrPatternSeek == *ptrDataSeek;
                             j++, ptrPatternSeek++, ptrDataSeek++)
                        {
                        }

                        if (j == patternLength)
                        {
                            indexes.Add(i + offset);
                        }

                        i++;
                    }
                }
            }

            return i;
        }
    }
}