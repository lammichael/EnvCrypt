using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Console.AddEntry
{
    class AddEntryCommandLineProcessor : VerbCommandLineProcessor<AddEntryVerbOptions>
    {
        protected override bool ReportErrors(AddEntryVerbOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            //
            var hasErrors = false;
            if (options.GetAlgorithm() == null)
            {
                System.Console.Error.WriteLine("Unrecognised algorithm: {0}", options.AlgorithmToUse);
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.KeyFile))
            {
                System.Console.Error.WriteLine("Key file path (encryption key) not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.DatFile))
            {
                System.Console.Error.WriteLine("DAT file path not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.Category))
            {
                System.Console.Error.WriteLine("Category not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.EntryName))
            {
                System.Console.Error.WriteLine("Entry name not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.StringToEncrypt))
            {
                System.Console.Error.WriteLine("String to encrypt not defined.");
                hasErrors = true;
            }

            return hasErrors;
        }


        protected override void RunWorflow(AddEntryVerbOptions options)
        {
            new AddEntryWorkflow().Run(options);
        }
    }
}
