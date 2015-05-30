using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.AddEntry;

namespace EnvCrypt.Console.AddEntry
{
    class AddEntryWorkflow
    {
        public void Run(AddEntryVerbOptions options)
        {
            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                var addEntryOpts = new AddEntryWorkflowOptions()
                {
                    CategoryName = options.Category,
                    EntryName = options.EntryName,
                    DatFilePath = options.DatFile,
                    KeyFilePath = options.KeyFile,
                    StringToEncrypt = options.StringToEncrypt
                };

                new AddRsaEntryBuilder(addEntryOpts).Build().Run();
            }
            else
            {
                System.Console.Error.WriteLine("Cannot generate key for encryption type: {0}", encryptionType);
            }
        }
    }
}
