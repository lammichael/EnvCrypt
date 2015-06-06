using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using EnvCrypt.Console.AddEntry;
using EnvCrypt.Console.DecryptEntry;
using EnvCrypt.Console.GenerateKey;

namespace EnvCrypt.Console
{
    class Program
    {
        private static void Main(string[] args)
        {
            ParserResult<object> parserResult = null;
#if (!DEBUG)
            try
            {
#endif
            var parser = new Parser(settings => settings.CaseSensitive = false);
            parserResult = parser.ParseArguments(args,
                typeof(GenerateKeyVerbOptions),
                typeof(AddEntryVerbOptions),
                typeof(DecryptEntryVerbOptions));
            /*catch (System.ArgumentException argEx)
            {
                // Occurs when an exact enum value is not passed
            }*/
#if (!DEBUG)
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("Exception whilst parsing arguments:{0}{1}", Environment.NewLine, ex);
                Environment.Exit(1);
            }
#endif

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


#if (!DEBUG)
            try
            {
#endif
                if (new GenerateKeyCommandLineProcessor().Run(parserResult))
                {
                    return;
                }
                if (new AddEntryCommandLineProcessor().Run(parserResult))
                {
                    return;
                }
                if (new DecryptEntryCommandLineProcessor().Run(parserResult))
                {
                    return;
                }
#if (!DEBUG)
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("Exception occurred:{0}{1}", Environment.NewLine, ex);
                Environment.Exit(2);
            }
#endif
        }
    }
}
