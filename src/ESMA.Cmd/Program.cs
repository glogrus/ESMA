// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="GLogrus">
//   Copyright (c) GLogrus. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ESMA.Cmd
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CommandLine;
    using CommandLine.Text;

    using ESMA.Cmd.Options;
    using ESMA.Cmd.Runners;

    /// <summary>
    ///     The program.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The display help.
        /// </summary>
        /// <param name="result">
        /// The result.
        /// </param>
        /// <param name="errs">
        /// The errs.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int DisplayHelp(ParserResult<object> result, IEnumerable<Error> errs)
        {
            if (errs.Any(error => error.Tag == ErrorType.NoVerbSelectedError))
            {
                var parser = new Parser(
                    with =>
                    {
                        with.HelpWriter = null;
                        with.AutoVersion = true;
                        with.AutoHelp = true;
                    });
                result = parser.ParseArguments<VerbFixture, VerbFind, VerbList>(new[] { "help" });
            }

            var helpText = HelpText.AutoBuild(
                result,
                help =>
                {
                    help.AdditionalNewLineAfterOption = false;
                    help.Copyright = string.Empty;
                    help.AutoHelp = true;
                    help.AutoVersion = true;
                    return HelpText.DefaultParsingErrorsHandler(result, help);
                });

            Console.WriteLine(helpText);
            return 0;
        }

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        private static int Main(string[] args)
        {
            var parser = new Parser(
                with =>
                {
                    with.HelpWriter = null;
                    with.AutoVersion = true;
                    with.AutoHelp = true;
                });

            var parserResult = parser.ParseArguments<VerbFixture, VerbFind, VerbList>(args);
            return parserResult.MapResult(
                (VerbFixture opts) => Fixture.Run(opts),
                (VerbFind opts) => Find.Run(opts),
                (VerbList opts) => List.Run(opts),
                errors => DisplayHelp(parserResult, errors));
        }
    }
}