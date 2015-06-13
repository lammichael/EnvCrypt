using System.Collections.Generic;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Generic;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Rsa;

namespace EnvCrypt.Console.DecryptEntry
{
    class DecryptEntryWorkflow
    {
        public void Run(DecryptEntryVerbOptions options)
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
                var toAdd = new CategoryEntryPair(categories[(int)catI], entries[(int)catI]);
                categoryEntryPairs.Add(toAdd);
            }


            var builder = new DecryptGenericWorkflowBuilder(
                new DecryptPlainTextEntryWorkflowBuilder(),
                new DecryptRsaEntryWorkflowBuilder(),
                new DecryptAesEntryWorkflowBuilder());
            var result = builder.Build().Run(new DecryptGenericWorkflowOptions()
            {
                CategoryEntryPair = categoryEntryPairs,
                DatFilePath = options.DatFile,
                KeyFilePath = options.KeyFile,
                ThrowExceptionIfEntryNotFound = true,
            });
            OutputToConsole(result);
        }


        private static void OutputToConsole(IList<EntriesDecrypterResult> decryptionResults)
        {
            foreach (var result in decryptionResults)
            {
                System.Console.Write(result.CategoryEntryPair.Category);
                System.Console.Write("\t");
                System.Console.Write(result.CategoryEntryPair.Entry);
                System.Console.Write("\t");
                System.Console.WriteLine(result.DecryptedValue);
            }
        }
    }
}
