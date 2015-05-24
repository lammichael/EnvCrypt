using System.IO;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.GenerateKey;
using EnvCrypt.Core.Verb.GenerateKey.Persister;

namespace EnvCrypt.Console.GenerateKey
{
    class GenerateKeyWorkflow
    {
        public const string PrivateKeyPostfix = ".private.eckey";
        public const string PublicKeyPostfix = ".public.eckey";

        public void Run(GenerateKeyVerbOptions options)
        {
            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                var keyPersisterOpts = new AsymmetricKeyFilePersisterOptions()
                {
                    NewPrivateKeyFullFilePath = Path.Combine(
                        options.OutputDirectory,
                        string.Concat(options.KeyName, PrivateKeyPostfix)),
                    NewPublicKeyFullFilePath = Path.Combine(
                        options.OutputDirectory,
                        string.Concat(options.KeyName, PublicKeyPostfix) ),
                    OverwriteFileIfExists = false
                };
                if (options.OutputKeyToConsole)
                {
                    new GenerateRsaKeyBuilder(keyPersisterOpts)
                        .WithCustomTextWriter(new ToConsoleTextWriter()).Build().Run();
                }
                else
                {
                    new GenerateRsaKeyBuilder(keyPersisterOpts).Build().Run();
                }
            }
            else
            {
                System.Console.Error.WriteLine("Cannot generate key for encryption type: {0}", encryptionType);
            }
        }
    }
}
