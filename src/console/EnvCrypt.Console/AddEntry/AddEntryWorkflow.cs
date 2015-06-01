using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.AddEntry;

namespace EnvCrypt.Console.AddEntry
{
    class AddEntryWorkflow
    {
        public void Run(AddEntryVerbOptions options)
        {
            var addEntryOpts = new AddEntryWorkflowOptions()
            {
                CategoryName = options.Category,
                EntryName = options.NewEntryName,
                DatFilePath = options.DatFile,
                KeyFilePath = options.KeyFile,
                StringToEncrypt = options.StringToEncrypt
            };

            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                new AddRsaEntryBuilder(addEntryOpts).Build().Run();
            }
            else if (encryptionType == EnvCryptAlgoEnum.PlainText)
            {
                addEntryOpts.KeyFilePath = null;
                new AddPlainTextEntryBuilder(addEntryOpts).Build().Run();
            }
            else
            {
                System.Console.Error.WriteLine("Cannot add entry for encryption type: {0}", encryptionType);
            }
        }
    }
}
