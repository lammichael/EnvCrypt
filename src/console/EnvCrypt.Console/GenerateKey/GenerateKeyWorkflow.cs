using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvCrypt.Console.Options;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Verb.GenerateKey;
using EnvCrypt.Core.Verb.GenerateKey.Persister;

namespace EnvCrypt.Console.GenerateKey
{
    class GenerateKeyWorkflow
    {
        public void Run(GenerateKeyVerbOptions options)
        {
            var encryptionType = options.GetAlgorithm();
            if (encryptionType == EnvCryptAlgoEnum.Rsa)
            {
                var keyPersisterOpts = new AsymmetricKeyFilePersisterOptions()
                {
                    NewPrivateKeyFullFilePath = Path.Combine(
                        options.OutputDirectory, options.KeyName + ".private.eckey"),
                    NewPublicKeyFullFilePath = Path.Combine(
                        options.OutputDirectory, options.KeyName + ".public.eckey"),
                    OverwriteFileIfExists = true
                };
                new GenerateRsaKeyBuilder(keyPersisterOpts).Build().Run();
            }
            else
            {
                System.Console.Error.WriteLine("Cannot generate key for encryption type: {0}", encryptionType);
            }
        }
    }
}
