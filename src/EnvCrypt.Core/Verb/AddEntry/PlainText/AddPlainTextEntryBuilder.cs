using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToDatPoco;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.EncryptionAlgo.PlainText;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.LoadKey.PlainText;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry.PlainText
{
    public class AddPlainTextEntryBuilder : GenericBuilder
    {
        private IDatLoader _datLoader;
        private IDatSaver<DatToFileSaverDetails> _datSaver;

        private AddPlainTextEntryWorkflow<PlainTextKey, AddPlainTextEntryWorkflowOptions> _workflow;

        public AddPlainTextEntryBuilder()
        {
            _datLoader = DatFromXmlFileFactory.GetDatLoader();
            _datSaver = DatXmlFileSaverFactory.GetDatSaver();
        }


        public AddPlainTextEntryBuilder WithDatLoader(IDatLoader datLoader)
        {
            _datLoader = datLoader;
            IsBuilt = false;
            _workflow = null;
            return this;
        }


        public AddPlainTextEntryBuilder WithDatSaver(IDatSaver<DatToFileSaverDetails> datSaver)
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
        public AddPlainTextEntryBuilder Build()
        {
            var userStringConverter = new Utf16LittleEndianUserStringConverter();

            var encryptWorkflow = new EncryptWorkflow<PlainTextKey, NullKeyLoaderDetails>(
                new PlainTextKeyLoader(),
                new PlainTextKeySuitabilityChecker(),
                userStringConverter,
                new PlainTextSegmentEncryptionAlgo());
            
            _workflow = new AddPlainTextEntryWorkflow<PlainTextKey, AddPlainTextEntryWorkflowOptions>(encryptWorkflow, _datLoader, _datSaver);
            IsBuilt = true;
            return this;
        }


        public void Run(AddPlainTextEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.CategoryName), "category name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.EntryName), "entry name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(options.StringToEncrypt), "string to encrypt cannot be null or empty");
            //
            if (!IsBuilt)
            {
                throw new EnvCryptException("workflow cannot be run because it has not been built");
            }
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

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(IsBuilt == (_workflow != null));
        }
    }
}
