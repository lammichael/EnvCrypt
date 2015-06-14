using System.Collections.Generic;
using EnvCrypt.Console.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Generic;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Rsa;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.RemoveEntry;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Console.RemoveEntry
{
    class RemoveEntryWorkflow
    {
        public void Run(RemoveEntryVerbOptions options)
        {
            var categories = options.GetCategories();
            var entries = options.GetEntries();
            if (categories.Count != entries.Count)
            {
                throw new EnvCryptConsoleException("count of categories and entries do not match");
            }

            var categoryEntryPairs = new List<CategoryEntryPair>();
            for (uint catI = 0; catI < categories.Count; catI++)
            {
                var toAdd = new CategoryEntryPair(categories[(int) catI], entries[(int) catI]);
                categoryEntryPairs.Add(toAdd);
            }


            var builder = new RemoveEntryWorkflowBuilder();
            builder.Build().Run(
                new DatFromFileLoaderOptions()
                {
                    DatFilePath = options.DatFile,
                },
                categoryEntryPairs,
                new DatToFileSaverOptions()
                {
                    DestinationFilePath = options.DatFile
                });
        }
    }
}
