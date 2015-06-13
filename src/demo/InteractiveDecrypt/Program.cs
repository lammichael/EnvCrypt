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
            try
            {
                Run();
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Uncaught exception occurred");
                throw;
            }
        }

        private static void Run()
        {
            Console.Write("Enter the DAT file path (your encrypted credentials): ");
            Console.WriteLine();
            var datFileFilePath = Console.ReadLine();
            if (!File.Exists(datFileFilePath))
            {
                Logger.Fatal()
                    .Message("DAT file does not exist: {0}", datFileFilePath)
                    .Write();
                Environment.Exit(1);
            }


            Console.Write("Enter private key file path: ");
            Console.WriteLine();
            var privateKeyFilePath = Console.ReadLine();
            if (!File.Exists(privateKeyFilePath))
            {
                Logger.Fatal()
                    .Message("Private key file does not exist: {0}", privateKeyFilePath)
                    .Write();
                Environment.Exit(1);
            }


            var builder = new DecryptGenericWorkflowBuilder(new DecryptPlainTextEntryWorkflowBuilder(),
                new DecryptRsaEntryWorkflowBuilder(), new DecryptAesEntryWorkflowBuilder());
            var result = builder.Build().Run(new DecryptGenericWorkflowOptions()
            {
                CategoryEntryPair = GetPairsFromConfig(),
                DatFilePath = datFileFilePath,
                KeyFilePath = privateKeyFilePath,
                ThrowExceptionIfEntryNotFound = true,
            });


            foreach (var r in result)
            {
                Logger.Info()
                    .Message("Category: {0}\tEntry{1}\t\tValue: {2}", r.CategoryEntryPair.Category, r.CategoryEntryPair.Entry,
                        r.DecryptedValue)
                    .Write();
            }

            Logger.Info()
                .Message("Press any key to exit")
                .Write();
            Console.ReadKey();
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
