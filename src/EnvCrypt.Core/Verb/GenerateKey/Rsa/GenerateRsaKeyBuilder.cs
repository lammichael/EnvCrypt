using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils.IO.StringWriter;
using EnvCrypt.Core.Verb.GenerateKey.Persister;
using EnvCrypt.Core.Verb.GenerateKey.Persister.Asymetric;

namespace EnvCrypt.Core.Verb.GenerateKey.Rsa
{
    public class GenerateRsaKeyBuilder : GenericBuilder
    {
        public const int DefaultRsaKeySize = 2048;
        public const bool DefaultUseOaepPadding = true;

        private IKeyPersister<RsaKey, AsymmetricKeyFilePersisterOptions> _persister;
        private GenerateKeyWorkflow<RsaKey, RsaKeyGenerationOptions, AsymmetricKeyFilePersisterOptions> _workflow;


        public GenerateRsaKeyBuilder()
        {
            Contract.Ensures(!IsBuilt);
            //
            IsBuilt = false;
            _persister = AsymmetricKeyFilePersisterFactory.GetRsaKeyPersister();
        }


        public GenerateRsaKeyBuilder WithKeyPersister(AsymmetricKeyFilePersister<RsaKey, EnvCryptKey, StringToFileWriterOptions> persister)
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
            _workflow = new GenerateKeyWorkflow<RsaKey, RsaKeyGenerationOptions, AsymmetricKeyFilePersisterOptions>(
                new RsaKeyGenerator(),
                _persister);
            IsBuilt = true;
            return this;
        }


        public void Run(AsymmetricKeyFilePersisterOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "fileOptions");
            //
            ThrowIfNotBuilt();

            var keyGenerationOptions = new RsaKeyGenerationOptions()
            {
                KeySize = DefaultRsaKeySize,
                UseOaepPadding = DefaultUseOaepPadding,
                NewKeyName = options.NewKeyName
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
