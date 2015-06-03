using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Verb.GenerateKey.Persister;

namespace EnvCrypt.Core.Verb.GenerateKey
{
    public class GenerateRsaKeyBuilder : GenericBuilder
    {
        public const int DefaultRsaKeySize = 2048;
        public const bool DefaultUseOaepPadding = true;

        private IKeyPersister<RsaKey, EnvCryptKey, AsymmetricKeyToFilePersisterOptions> _persister;
        private GenerateKeyWorkflow<RsaKey, RsaKeyGenerationOptions, EnvCryptKey, AsymmetricKeyToFilePersisterOptions> _workflow;


        public GenerateRsaKeyBuilder()
        {
            Contract.Ensures(!IsBuilt);
            //
            IsBuilt = false;
            _persister = AsymmetricKeyPersisterFactory.GetRsaKeyPersister();
        }


        public GenerateRsaKeyBuilder WithKeyPersister(AsymmetricKeyPersister<RsaKey, EnvCryptKey> persister)
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
        public GenerateRsaKeyBuilder Build()
        {
            Contract.Ensures(Contract.Result<GenerateRsaKeyBuilder>() != null);
            Contract.Ensures(IsBuilt);
            //
            _workflow = new GenerateKeyWorkflow<RsaKey, RsaKeyGenerationOptions, EnvCryptKey, AsymmetricKeyToFilePersisterOptions>(
                new RsaKeyGenerator(),
                _persister);
            IsBuilt = true;
            return this;
        }


        public void Run(AsymmetricKeyToFilePersisterOptions toFileOptions)
        {
            Contract.Requires<ArgumentNullException>(toFileOptions != null, "fileOptions");
            //
            ThrowIfNotBuilt();

            var keyGenerationOptions = new RsaKeyGenerationOptions()
            {
                KeySize = DefaultRsaKeySize,
                UseOaepPadding = DefaultUseOaepPadding,
                NewKeyName = toFileOptions.NewKeyName
            };

            _workflow.Run(keyGenerationOptions, toFileOptions);
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
