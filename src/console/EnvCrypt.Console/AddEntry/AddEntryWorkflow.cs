using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.AddEntry;
using EnvCrypt.Core.Verb.AddEntry.PlainText;
using EnvCrypt.Core.Verb.AddEntry.Rsa;

namespace EnvCrypt.Console.AddEntry
{
    class AddEntryWorkflow
    {
        public void Run(AddEntryVerbOptions options)
        {
            var addEntryOpts = new AddEntryUsingKeyFileWorkflowOptions()
            {
                CategoryName = options.Category,
                EntryName = options.NewEntryName,
                DatFilePath = options.DatFile,
                StringToEncrypt = options.StringToEncrypt
            };

            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                addEntryOpts.KeyFilePath = options.KeyFile;
                new AddRsaEntryBuilder().Build().Run(addEntryOpts);
            }
            else if (encryptionType == EnvCryptAlgoEnum.PlainText)
            {
                new AddPlainTextEntryBuilder().Build().Run(addEntryOpts);
            }
            else
            {
                System.Console.Error.WriteLine("Cannot add entry for encryption type: {0}", encryptionType);
            }
        }
    }
}
