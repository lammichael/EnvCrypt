using System.IO;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.GenerateKey.Aes;
using EnvCrypt.Core.Verb.GenerateKey.Persister.Asymetric;
using EnvCrypt.Core.Verb.GenerateKey.Persister.Symmetric;
using EnvCrypt.Core.Verb.GenerateKey.Rsa;

namespace EnvCrypt.Console.GenerateKey
{
    class GenerateKeyWorkflow
    {
        public const string CommonPostFix = ".eckey";
        public const string PrivateKeyPostfix = ".private.eckey";
        public const string PublicKeyPostfix = ".public.eckey";

        public void Run(GenerateKeyVerbOptions options)
        {
            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                var rsaKeyPersisterOpts = new AsymmetricKeyFilePersisterOptions()
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

                if (options.OutputKeyToConsole)
                {
                    new GenerateRsaKeyBuilder()
                    .WithKeyPersister(AsymmetricKeyFilePersisterFactory.GetRsaKeyPersister(new ToConsoleTextWriter()))
                        .Build().Run(rsaKeyPersisterOpts);
                }
                else
                {
                    new GenerateRsaKeyBuilder().Build().Run(rsaKeyPersisterOpts);
                }



            }
            else if (encryptionType == EnvCryptAlgoEnum.Aes)
            {
                var aesKeyPersisterOpts = new SymmetricKeyFilePersisterOptions()
                {
                    NewKeyName = options.KeyName,
                    NewKeyFileFullPath = Path.Combine(
                        options.OutputDirectory,
                        string.Concat(options.KeyName, CommonPostFix)),
                    OverwriteFileIfExists = false
                };

                if (options.OutputKeyToConsole)
                {
                    new GenerateAesKeyBuilder()
                    .WithKeyPersister(SymmetricKeyFilePersisterFactory.GetAesKeyPersister(new ToConsoleTextWriter()))
                        .Build().Run(aesKeyPersisterOpts);
                }
                else
                {
                    new GenerateAesKeyBuilder().Build().Run(aesKeyPersisterOpts);
                }
            }
            else
            {
                System.Console.Error.WriteLine("Unsupported encryption type: {0}", encryptionType);
            }
        }
    }
}
