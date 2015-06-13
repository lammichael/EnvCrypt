using System;
using System.Collections.Generic;
using System.IO;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Generic;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Rsa;
using EnvCrypt.Core.Verb.LoadDat;
using NLog.Fluent;

namespace EnvCrypt.InteractiveDecrypt
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Logger.Fatal()
                    .Message("2 arguments required - DAT file path & decrypting key file path")
                    .Write();
                Environment.Exit(1);
            }

#if (!DEBUG)
            try
            {
#endif
                Run(args[0], args[1]);
#if (!DEBUG)
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Uncaught exception occurred");
                throw;
            }
#endif
        }


        private static void Run(string datFilePath, string keyFilePath)
        {
            if (!File.Exists(datFilePath))
            {
                Logger.Fatal()
                    .Message("DAT file does not exist: {0}", datFilePath)
                    .Write();
                Environment.Exit(1);
            }


            if (!File.Exists(keyFilePath))
            {
                Logger.Fatal()
                    .Message("Private key file does not exist: {0}", keyFilePath)
                    .Write();
                Environment.Exit(1);
            }


            var builder = new DecryptGenericWorkflowBuilder(new DecryptPlainTextEntryWorkflowBuilder(),
                new DecryptRsaEntryWorkflowBuilder(), new DecryptAesEntryWorkflowBuilder());
            var result = builder.Build().Run(new DecryptGenericWorkflowOptions()
            {
                CategoryEntryPair = GetPairsFromConfig(),
                DatFilePath = datFilePath,
                KeyFilePath = keyFilePath,
                ThrowExceptionIfEntryNotFound = true,
            });


            foreach (var r in result)
            {
                Logger.Info()
                    .Message("Category: {0}\tEntry: {1}\t\tValue: {2}", r.CategoryEntryPair.Category, r.CategoryEntryPair.Entry,
                        r.DecryptedValue)
                    .Write();
            }
        }


        static IList<CategoryEntryPair> GetPairsFromConfig()
        {
            var config = EntriesToDecrypt.GetConfig();

            var ret = new List<CategoryEntryPair>();

            foreach (var category in config.Items)
            {
                foreach (var entry in category.Entry)
                {
                    ret.Add(new CategoryEntryPair(category.Name, entry.Value));
                }
            }
            return ret;
        }
    }
}
