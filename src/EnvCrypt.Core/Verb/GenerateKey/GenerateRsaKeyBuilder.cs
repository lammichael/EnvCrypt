using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key.Xml;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.GenerateKey.Persister;

namespace EnvCrypt.Core.Verb.GenerateKey
{
    public class GenerateRsaKeyBuilder
    {
        public const int DefaultRsaKeySize = 2048;
        public const bool DefaultUseOaepPadding = true;

        public bool IsBuilt { get; private set; }
        private GenerateKeyWorkflow<RsaKey, RsaKeyGenerationOptions, EnvCryptKey, AsymmetricKeyFilePersisterOptions> _workflow;

        private readonly IKeyGenerator<RsaKey, RsaKeyGenerationOptions> _rsaKeyGenerator;
        private readonly RsaKeyGenerationOptions _keyGenerationOptions;

        private readonly IKeyFilePersister<RsaKey, EnvCryptKey, AsymmetricKeyFilePersisterOptions> _persister;
        private readonly AsymmetricKeyFilePersisterOptions _fileOptions;


        public GenerateRsaKeyBuilder(AsymmetricKeyFilePersisterOptions fileOptions)
        {
            IsBuilt = false;
            _fileOptions = fileOptions;

            _rsaKeyGenerator = new RsaKeyGenerator();
            _keyGenerationOptions = new RsaKeyGenerationOptions(DefaultRsaKeySize, DefaultUseOaepPadding);
            
            _persister = new RsaKeyFilePersister(new RsaKeyToXmlMapper(new Base64PersistConverter()), new XmlSerializationUtils<EnvCryptKey>(), new StringToFileWriter(new MyDirectory(), new MyFile()));
        }

        /*private GenerateRsaKeyBuilder(AsymmetricKeyFilePersisterOptions fileOptions, IKeyFilePersister<RsaKey, EnvCryptKey, AsymmetricKeyFilePersisterOptions> persister) : this(fileOptions)
        {
            _persister = persister;
        }


        /// <summary>
        /// Returns a new Builder instance with a custom file persister.
        /// </summary>
        public GenerateRsaKeyBuilder WithCustomKeyFilePersister(
            IKeyFilePersister<RsaKey, EnvCryptKey, AsymmetricKeyFilePersisterOptions> persister)
        {
            return new GenerateRsaKeyBuilder(_fileOptions, persister);
        }*/


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to 
        /// <see cref="Run"/> method.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public GenerateRsaKeyBuilder Build()
        {
            _workflow = new GenerateKeyWorkflow<RsaKey, RsaKeyGenerationOptions, EnvCryptKey, AsymmetricKeyFilePersisterOptions>(_rsaKeyGenerator, _keyGenerationOptions, _persister, _fileOptions);
            IsBuilt = true;
            return this;
        }


        public void Run()
        {
            if (_workflow == null)
            {
                throw new EnvCryptException("workflow cannot be run because it has not been built");
            }
            _workflow.Run();
        }


        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            // If workflow is null then not built
            Contract.Invariant(IsBuilt == (_workflow != null));
        }
    }
}
