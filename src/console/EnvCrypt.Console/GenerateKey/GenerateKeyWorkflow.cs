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
            var keyPersisterOpts = new AsymmetricKeyFilePersisterOptions()
            {
                NewKeyName = options.KeyName,
                NewPrivateKeyFullFilePath = Path.Combine(
                    options.OutputDirectory,
                    string.Concat(options.KeyName, PrivateKeyPostfix)),
                NewPublicKeyFullFilePath = Path.Combine(
                    options.OutputDirectory,
                    string.Concat(options.KeyName, PublicKeyPostfix)),
                OverwriteFileIfExists = false
            };

            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
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
                System.Console.Error.WriteLine("Unsupported encryption type: {0}", encryptionType);
            }
        }
    }
}
