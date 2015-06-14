using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using EnvCrypt.ServiceInstaller.CommandLine;

namespace EnvCrypt.ServiceInstaller
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            ParserResult<CommandLineOptions> parserResult = null;
#if (!DEBUG)
            try
            {
#endif
            var parser = new Parser(settings => settings.CaseSensitive = false);
            parserResult = parser.ParseArguments<CommandLineOptions>(args);
#if (!DEBUG)
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Exception whilst parsing arguments: {0}{1}{2}{1}{3}", string.Join(" ", args), Environment.NewLine, ex, HelpText.AutoBuild(parserResult));
                Environment.Exit(1);
            }
#endif
            if (parserResult.Errors.Any())
            {
                Logger.Info(HelpText.AutoBuild(parserResult));
                Environment.Exit(1);
            }
#if (!DEBUG)
            try
            {
#endif
            Validator.ThrowIfInvalid(parserResult.Value);
            new Workflow().Run(parserResult.Value);
#if (!DEBUG)
            }
            catch (Exception ex)
            {
                Logger.Fatal(ex, "Uncaught exception occured{0}{1}", Environment.NewLine, ex);
                Environment.Exit(2);
            }
#endif
        }
    }
}
