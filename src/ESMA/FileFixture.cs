// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileFixture.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    /// <summary>
    ///     The file fixture.
    /// </summary>
    public class FileFixture : IDisposable
    {
        /// <summary>
        ///     The lazy.
        /// </summary>
        private static readonly Lazy<List<FileFixture>> LazyFileFixtures = new Lazy<List<FileFixture>>(GetFileFixtures);

        /// <summary>
        ///     The pattern file path.
        /// </summary>
        private readonly string patternFilePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFixture"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="dataPath">
        /// The data path.
        /// </param>
        /// <param name="patternPath">
        /// The pattern path.
        /// </param>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <param name="bufferSize">
        /// The buffer size.
        /// </param>
        public FileFixture(string name, string dataPath, string patternPath, long[] expected, int bufferSize = -1)
        {
            this.Name = name;
            this.DataFilePath = dataPath;
            this.patternFilePath = patternPath;
            this.Expected = expected;
            this.BufferSize = bufferSize;
        }

        /// <summary>
        ///     Gets or sets the config file.
        /// </summary>
        public static string ConfigFile { private get; set; }

        /// <summary>
        ///     The get instance.
        /// </summary>
        /// <returns>
        ///     The IEnumerable of FileFixture.
        /// </returns>
        public static IEnumerable<FileFixture> FileFixtures => LazyFileFixtures.Value;

        /// <summary>
        ///     Gets or sets the buffer size.
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public byte[] Data => File.ReadAllBytes(this.DataFilePath);

        /// <summary>
        ///     Gets the data file path.
        /// </summary>
        public string DataFilePath { get; }

        /// <summary>
        ///     Gets the expected.
        /// </summary>
        public long[] Expected { get; }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the pattern.
        /// </summary>
        public byte[] Pattern => File.ReadAllBytes(this.patternFilePath);

        /// <summary>
        ///     The dispose.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        ///     The get file fixtures.
        /// </summary>
        /// <returns>
        ///     The List of FileFixture.
        /// </returns>
        private static List<FileFixture> GetFileFixtures()
        {
            var list = new List<FileFixture>();

            var document = XDocument.Load(ConfigFile);
            var files = document.XPathSelectElement("/files");
            if (files == null)
            {
                return list;
            }

            var directory = files.Attribute("directory")?.Value ?? "./";
            foreach (var file in files.XPathSelectElements("file"))
            {
                var name = file.Attribute("name")?.Value;
                var pattern = file.Attribute("pattern")?.Value;
                var data = file.Attribute("data")?.Value;
                var bufferSize = int.Parse(file.Attribute("buffersize")?.Value ?? "-1");
                var expected = file.XPathSelectElements("index").Select(index => long.Parse(index.Attribute("value")?.Value ?? "0")).ToArray();
                var fixture = new FileFixture(
                    name,
                    Path.Combine(directory, data ?? string.Empty),
                    Path.Combine(directory, pattern ?? string.Empty),
                    expected,
                    bufferSize);
                list.Add(fixture);
            }

            return list;
        }
    }
}