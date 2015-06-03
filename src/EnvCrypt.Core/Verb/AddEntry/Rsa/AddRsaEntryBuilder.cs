using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToDatPoco;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using EnvCrypt.Core.Key.Mapper.Xml.ToKeyPoco;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.LoadKey.Rsa;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry.Rsa
{
    public class AddRsaEntryBuilder : GenericBuilder
    {
        private IKeyLoader<RsaKey, KeyFromFileDetails> _keyLoader;
        private IDatLoader _datLoader;
        private IDatSaver<DatToFileSaverDetails> _datSaver;


        private AddEntryUsingKeyFileWorkflow<RsaKey, AddEntryUsingKeyFileWorkflowOptions> _workflow;

        public AddRsaEntryBuilder()
        {
            _keyLoader = LoadKeyFromXmlFileFactory.GetRsaKeyLoader();
            _datLoader = DatFromXmlFileFactory.GetDatLoader();
            _datSaver = DatXmlFileSaverFactory.GetDatSaver();
        }


        public AddRsaEntryBuilder WithKeyLoader(IKeyLoader<RsaKey, KeyFromFileDetails> keyLoader)
        {
            _keyLoader = keyLoader;
            IsBuilt = false;
            _workflow = null;
            return this;
        }


        public AddRsaEntryBuilder WithDatLoader(IDatLoader datLoader)
        {
            _datLoader = datLoader;
            IsBuilt = false;
            _workflow = null;
            return this;
        }


        public AddRsaEntryBuilder WithDatSaver(IDatSaver<DatToFileSaverDetails> datSaver)
        {
            _datSaver = datSaver;
            IsBuilt = false;
            _workflow = null;
            return this;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public AddRsaEntryBuilder Build()
        {
            var encryptWorkflow = new EncryptWorkflow<RsaKey, KeyFromFileDetails>(
                _keyLoader,
                new RsaKeySuitabilityChecker(),
                new Utf16LittleEndianUserStringConverter(),
                new RsaSegmentEncryptionAlgo(new RsaAlgo(), new RsaMaxEncryptionCalc()));
            _workflow = new AddEntryUsingKeyFileWorkflow<RsaKey, AddEntryUsingKeyFileWorkflowOptions>(encryptWorkflow, _datLoader, _datSaver);
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
