using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Console.DecryptEntry
{
    class DecryptEntryCommandLineProcessor : VerbCommandLineProcessor<DecryptEntryVerbOptions>
    {
        protected override bool ReportErrors(DecryptEntryVerbOptions options)
        {
            var hasErrors = false;
            var algorithm = options.GetAlgorithm();
            if (algorithm == null)
            {
                System.Console.Error.WriteLine("Unrecognised algorithm: {0}", options.AlgorithmToUse);
                hasErrors = true;
            }
            else
            {
                // PlainText encryption doesn't require any key
                if (algorithm.Value != EnvCryptAlgoEnum.PlainText)
                {
                    if (string.IsNullOrWhiteSpace(options.KeyFiles))
                    {
                        System.Console.Error.WriteLine("No key file paths (decryption keys) defined.");
                        hasErrors = true;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(options.DatFile))
            {
                System.Console.Error.WriteLine("DAT file path not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.Categories))
            {
                System.Console.Error.WriteLine("Category not defined.");
                hasErrors = true;
            }
            if (string.IsNullOrWhiteSpace(options.Entries))
            {
                System.Console.Error.WriteLine("Entry name not defined.");
                hasErrors = true;
            }

            var categories = options.GetCategories();
            var entries = options.GetEntries();

            if (categories.Count != entries.Count)
            {
                System.Console.Error.WriteLine("Number of categories ({0}) does not match the number of entries ({1}).", categories.Count, entries.Count);
                hasErrors = true;
            }

            return hasErrors;
        }


        protected override void RunWorflow(DecryptEntryVerbOptions options)
        {
            new DecryptEntryWorkflow().Run(options);
        }
    }
}
