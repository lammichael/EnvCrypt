using System.Collections.Generic;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Verb.GetEntry;

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
            

            var workflowOptions = new DecryptEntryWorkflowOptions()
            {
                DatFilePath = options.DatFile,
                KeyFilePaths = options.GetKeyFiles(),
                CategoryEntryPair = categoryEntryPairs
            };

            IList<EntriesDecrypterResult> decryptionResults = null;

            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                decryptionResults = new DecryptRsaEntryWorkflowBuilder(workflowOptions).Build().Run();
            }
            else
            {
                System.Console.Error.WriteLine("Unsupported encryption type: {0}", encryptionType);
            }


            OutputToConsole(decryptionResults);
        }


        private static void OutputToConsole(IList<EntriesDecrypterResult> decryptionResults)
        {
            foreach (var result in decryptionResults)
            {
                System.Console.Write(result.CategoryEntryPair.Category);
                System.Console.Write("\t");
                System.Console.Write(result.CategoryEntryPair.Entry);
                System.Console.Write("\t");
                System.Console.Write(result.DecryptedValue);
            }
        }
    }
}
