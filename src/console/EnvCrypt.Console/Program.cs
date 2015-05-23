using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using EnvCrypt.Console.GenerateKey;

namespace EnvCrypt.Console
{
    class Program
    {
        private static void Main(string[] args)
        {
            var parser = new Parser(settings => settings.CaseSensitive = false);
            var parserResult = parser.ParseArguments(args, typeof (GenerateKeyVerbOptions));
            /*catch (System.ArgumentException argEx)
            {
                // Occurs when an exact enum value is not passed
            }*/

            if (parserResult.Errors.Any())
            {
                System.Console.Error.WriteLine(HelpText.AutoBuild(parserResult));
                Environment.Exit(1);
            }

            if (parserResult.Value is NullInstance)
            {
                System.Console.Error.WriteLine(HelpText.AutoBuild(parserResult));
                Environment.Exit(1);
            }
            var generateKeyVerbOptions = parserResult.Value as GenerateKeyVerbOptions;
            if (generateKeyVerbOptions != null)
            {
                var validator = new GenerateKeyOptionsValidator();
                if (validator.HasErrorsAndReport(generateKeyVerbOptions))
                {
                    System.Console.Error.WriteLine(HelpText.AutoBuild(parserResult));
                    Environment.Exit(1);
                }

                try
                {
                    new GenerateKeyWorkflow().Run(generateKeyVerbOptions);
                }
                catch (Exception ex)
                {
                    System.Console.Error.WriteLine("Exception occurred:{0}{1}", Environment.NewLine, ex);
#if (DEBUG)
                    throw;
#endif
                    Environment.Exit(2);
                }
            }
        }
    }
}
