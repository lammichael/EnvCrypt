using System.Collections.Generic;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
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
            

            var workflowOptions = new DecryptEntryWorkflowOptions()
            {
                DatFilePath = options.DatFile,
                KeyFilePaths = options.GetKeyFiles(),
                CategoryEntryPair = categoryEntryPairs
            };


            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                var decryptionResults = new DecryptRsaEntryWorkflowBuilder().Build()
                    .Run(workflowOptions);
                OutputToConsole(decryptionResults);
            }
            else if (encryptionType == EnvCryptAlgoEnum.Aes)
            {
                var decryptionResults = new DecryptAesEntryWorkflowBuilder().Build()
                    .Run(workflowOptions);
                OutputToConsole(decryptionResults);
            }
            else if (encryptionType == EnvCryptAlgoEnum.PlainText)
            {
                var decryptionResults = new DecryptPlainTextEntryWorkflowBuilder().Build()
                    .Run(workflowOptions);
                OutputToConsole(decryptionResults);
            }
            else
            {
                System.Console.Error.WriteLine("Unsupported encryption type: {0}", encryptionType);
            }
        }


        private static void OutputToConsole<TKey>(IList<EntriesDecrypterResult<TKey>> decryptionResults)
            where TKey : KeyBase
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
