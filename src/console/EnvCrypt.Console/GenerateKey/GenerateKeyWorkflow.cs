using System.IO;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.GenerateKey;
using EnvCrypt.Core.Verb.GenerateKey.Persister;
using EnvCrypt.Core.Verb.GenerateKey.Persister.Asymetric;
using EnvCrypt.Core.Verb.GenerateKey.Rsa;

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
                    new GenerateRsaKeyBuilder()
                    .WithKeyPersister(AsymmetricKeyFilePersisterFactory.GetRsaKeyPersister(new ToConsoleTextWriter()))
                        .Build().Run(keyPersisterOpts);
                }
                else
                {
                    new GenerateRsaKeyBuilder().Build().Run(keyPersisterOpts);
                }
            }
            else
            {
                System.Console.Error.WriteLine("Unsupported encryption type: {0}", encryptionType);
            }
        }
    }
}
