using System;
using CommandLine;
using CommandLine.Text;

namespace EnvCrypt.Console
{
    internal abstract class VerbCommandLineProcessor<TOptions> : IVerbCommandLineProcessor<TOptions>
        where TOptions : class
    {
        public bool Run(ParserResult<object> parserResult)
        {
            var options = parserResult.Value as TOptions;
            if (options == null)
            {
                return false;
            }

            if (ReportErrors(options))
            {
                System.Console.Error.WriteLine(HelpText.AutoBuild(parserResult));
                throw new EnvCryptConsoleException("Command line argument validation errors found");
            }

            RunWorflow(options);
            return true;
        }

        /// <summary>
        /// Writes the validation errors of the command line options to standard error,
        /// and returns if there were errors found or not.
        /// </summary>
        /// <param name="options">to validate</param>
        /// <returns>true if errors were found</returns>
        protected abstract bool ReportErrors(TOptions options);

        protected abstract void RunWorflow(TOptions options);
    }
}