using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Console.DecryptEntry
{
    class DecryptEntryCommandLineProcessor : VerbCommandLineProcessor<DecryptEntryVerbOptions>
    {
        protected override bool ReportErrors(DecryptEntryVerbOptions options)
        {
            var hasErrors = false;
            
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
