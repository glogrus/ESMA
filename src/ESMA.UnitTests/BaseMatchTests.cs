// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseMatchTests.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.UnitTests
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using FluentAssertions;

    using Xunit;

    /// <summary>
    ///     The base match tests.
    /// </summary>
    public class BaseMatchTests
    {
        /// <summary>
        ///     Gets the algorithms.
        /// </summary>
        public static IEnumerable<BaseMatch> Algorithms
        {
            get
            {
                return AlgorithmEnumerator.Algorithms.Select(algorithm => algorithm.Value);
            }
        }

        /// <summary>
        ///     Gets the files.
        /// </summary>
        public static IEnumerable<FileFixture> FileFixtures
        {
            get
            {
                FileFixture.ConfigFile = "FileFixture.xml";
                return FileFixture.FileFixtures;
            }
        }

        /// <summary>
        ///     Gets the matrix data.
        /// </summary>
        public static MatrixTheoryData<BaseMatch, FileFixture> MatrixData { get; } =
            new MatrixTheoryData<BaseMatch, FileFixture>(Algorithms, FileFixtures);

        /// <summary>
        /// The file test.
        /// </summary>
        /// <param name="matcher">
        /// The matcher.
        /// </param>
        /// <param name="file">
        /// The file.
        /// </param>
        [Theory]
        [MemberData(nameof(MatrixData))]
        public void MatchInFileTest(BaseMatch matcher, FileFixture file)
        {
            matcher.Pattern = file.Pattern;
            matcher.BufferSize = file.BufferSize;
            var indexes = matcher.Match(file.DataFilePath);
            indexes.Should().BeEquivalentTo(file.Expected);
        }

        /// <summary>
        /// The match test.
        /// </summary>
        /// <param name="matcher">
        /// The matcher.
        /// </param>
        /// <param name="file">
        /// The file.
        /// </param>
        [Theory]
        [MemberData(nameof(MatrixData))]
        public void MatchInMemoryTest(BaseMatch matcher, FileFixture file)
        {
            var indexes = matcher.Match(file.Data, file.Pattern);
            indexes.Should().BeEquivalentTo(file.Expected);
        }

        /// <summary>
        /// The matrix theory data.
        /// </summary>
        /// <typeparam name="T1">
        /// Class T1
        /// </typeparam>
        /// <typeparam name="T2">
        /// Class T2
        /// </typeparam>
        public class MatrixTheoryData<T1, T2> : TheoryData<T1, T2>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MatrixTheoryData{T1,T2}"/> class.
            /// </summary>
            /// <param name="data1">
            /// The data 1.
            /// </param>
            /// <param name="data2">
            /// The data 2.
            /// </param>
            public MatrixTheoryData(IEnumerable<T1> data1, IEnumerable<T2> data2)
            {
                var enumerable1 = data1 as T1[] ?? data1.ToArray();
                Contract.Assert(data1 != null && enumerable1.Any());
                var enumerable2 = data2 as T2[] ?? data2.ToArray();
                Contract.Assert(data2 != null && enumerable2.Any());

                foreach (var t1 in enumerable1)
                {
                    foreach (var t2 in enumerable2)
                    {
                        this.Add(t1, t2);
                    }
                }
            }
        }
    }
}