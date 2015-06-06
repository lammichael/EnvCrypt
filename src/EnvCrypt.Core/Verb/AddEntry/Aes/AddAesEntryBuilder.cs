using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo.Aes;
using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Verb.AddEntry.Rsa;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry.Aes
{
    public class AddAesEntryBuilder : GenericBuilder
    {
        private IKeyLoader<AesKey, KeyFromFileDetails> _keyLoader;
        private IDatLoader _datLoader;
        private IDatSaver<DatToFileSaverDetails> _datSaver;

        private AddEntryUsingKeyFileWorkflow<AesKey, AddEntryUsingKeyFileWorkflowOptions> _workflow;

        public AddAesEntryBuilder()
        {
            _keyLoader = LoadKeyFromXmlFileFactory.GetAesKeyLoader();
            _datLoader = DatFromXmlFileFactory.GetDatLoader();
            _datSaver = DatXmlFileSaverFactory.GetDatSaver();
        }


        public AddAesEntryBuilder WithKeyLoader(IKeyLoader<AesKey, KeyFromFileDetails> keyLoader)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            //
            _keyLoader = keyLoader;
            MarkAsNotBuilt();
            return this;
        }


        public AddAesEntryBuilder WithDatLoader(IDatLoader datLoader)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            //
            _datLoader = datLoader;
            MarkAsNotBuilt();
            return this;
        }


        public AddAesEntryBuilder WithDatSaver(IDatSaver<DatToFileSaverDetails> datSaver)
        {
            Contract.Requires<ArgumentNullException>(datSaver != null, "datSaver");
            //
            _datSaver = datSaver;
            MarkAsNotBuilt();
            return this;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public AddAesEntryBuilder Build()
        {
            Contract.Ensures(Contract.Result<AddAesEntryBuilder>() != null);
            //
            var encryptWorkflow = new EncryptWorkflow<AesKey, KeyFromFileDetails>(
                _keyLoader,
                new AesKeySuitabilityChecker(),
                new Utf16LittleEndianUserStringConverter(),
                new AesSegmentEncryptionAlgo(new AesAlgo()));
            _workflow = new AddEntryUsingKeyFileWorkflow<AesKey, AddEntryUsingKeyFileWorkflowOptions>(encryptWorkflow, _datLoader, _datSaver);
            IsBuilt = true;
            return this;
        }


        public void Run(AddEntryUsingKeyFileWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.CategoryName), "category name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.EntryName), "entry name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.KeyFilePath), "key file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(options.StringToEncrypt), "string to encrypt cannot be null or empty");
            //
            ThrowIfNotBuilt();
            _workflow.Run(options);
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
