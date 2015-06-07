using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils.IO.StringWriter;
using EnvCrypt.Core.Verb.GenerateKey.Persister.Symmetric;

namespace EnvCrypt.Core.Verb.GenerateKey.Aes
{
    public class GenerateAesKeyBuilder : GenericBuilder
    {
        public const int DefaultAesKeySize = 256;

        private SymmetricKeyFilePersister<AesKey, EnvCryptKey, StringToFileWriterOptions> _persister;
        private GenerateKeyWorkflow<AesKey, AesKeyGenerationOptions, EnvCryptKey, SymmetricKeyFilePersisterOptions> _workflow;


        public GenerateAesKeyBuilder()
        {
            Contract.Ensures(!IsBuilt);
            //
            IsBuilt = false;
            _persister = SymmetricKeyFilePersisterFactory.GetAesKeyPersister();
        }


        public GenerateAesKeyBuilder WithKeyPersister(SymmetricKeyFilePersister<AesKey, EnvCryptKey, StringToFileWriterOptions> persister)
        {
            _persister = persister;
            SetWorkflowToNull();
            return this;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public GenerateAesKeyBuilder Build()
        {
            Contract.Ensures(Contract.Result<GenerateAesKeyBuilder>() != null);
            Contract.Ensures(IsBuilt);
            //
            _workflow = new GenerateKeyWorkflow<AesKey, AesKeyGenerationOptions, EnvCryptKey, SymmetricKeyFilePersisterOptions>(new AesKeyGenerator(), _persister);
            IsBuilt = true;
            return this;
        }


        public void Run(SymmetricKeyFilePersisterOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "fileOptions");
            //
            ThrowIfNotBuilt();

            var keyGenerationOptions = new AesKeyGenerationOptions()
            {
                NewKeyName = options.NewKeyName,
                KeySize = DefaultAesKeySize
            };
            _workflow.Run(keyGenerationOptions, options);
        }


        protected override void SetWorkflowToNull()
        {
            _workflow = null;
        }

        protected override bool IsWorkflowNull()
        {
            return _workflow == null;
        }
    }
}
