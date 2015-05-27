using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.Key.XmlPoco;
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

        private IKeyFilePersister<RsaKey, EnvCryptKey, AsymmetricKeyFilePersisterOptions> _persister;
        private readonly AsymmetricKeyFilePersisterOptions _fileOptions;
        private ITextWriter _textWriter;


        public GenerateRsaKeyBuilder(AsymmetricKeyFilePersisterOptions fileOptions)
        {
            Contract.Requires<ArgumentNullException>(fileOptions != null, "fileOptions");
            Contract.Ensures(!IsBuilt);
            //
            IsBuilt = false;
            _fileOptions = fileOptions;

            _rsaKeyGenerator = new RsaKeyGenerator();
            _keyGenerationOptions = new RsaKeyGenerationOptions(DefaultRsaKeySize, DefaultUseOaepPadding);
            _textWriter = new TextWriter(new MyFile());
        }


        /// <summary>
        /// Returns a new Builder instance with a custom text writer used
        /// to write the XML contents.
        /// </summary>
        public GenerateRsaKeyBuilder WithCustomTextWriter(ITextWriter textWriter)
        {
            Contract.Requires<ArgumentNullException>(textWriter != null, "textWriter");
            Contract.Ensures(Contract.Result<GenerateRsaKeyBuilder>() != null);
            Contract.Ensures(!IsBuilt, "Build() must be called for Builder to be built");
            //
            _textWriter = textWriter;
            IsBuilt = false;
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
            _persister = new RsaKeyFilePersister(new RsaKeyToXmlMapper(new Base64PersistConverter()), new XmlSerializationUtils<EnvCryptKey>(), new StringToFileWriter(new MyDirectory(), new MyFile(), _textWriter));

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
            Contract.Invariant(IsBuilt == (_persister != null));
            Contract.Invariant(_rsaKeyGenerator != null);
            Contract.Invariant(_keyGenerationOptions != null);
            Contract.Invariant(_fileOptions != null);
            Contract.Invariant(_textWriter != null);
        }
    }
}
