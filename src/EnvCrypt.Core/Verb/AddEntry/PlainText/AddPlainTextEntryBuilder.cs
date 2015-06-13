using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo.PlainText;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.LoadKey.PlainText;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry.PlainText
{
    public class AddPlainTextEntryBuilder : GenericBuilder
    {
        private IDatLoader<DatFromFileLoaderOptions> _datLoader;
        private IDatSaver<DatToFileSaverOptions> _datSaver;

        private AddPlainTextEntryWorkflow<PlainTextKey, AddPlainTextEntryWorkflowOptions> _workflow;

        public AddPlainTextEntryBuilder()
        {
            _datLoader = DatFromXmlFileFactory.GetDatLoader();
            _datSaver = DatXmlFileSaverFactory.GetDatSaver();
        }


        public AddPlainTextEntryBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            //
            _datLoader = datLoader;
            IsBuilt = false;
            _workflow = null;
            return this;
        }


        public AddPlainTextEntryBuilder WithDatSaver(IDatSaver<DatToFileSaverOptions> datSaver)
        {
            Contract.Requires<ArgumentNullException>(datSaver != null, "datSaver");
            //
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
            Contract.Ensures(Contract.Result<AddPlainTextEntryBuilder>() != null);
            //
            var encryptWorkflow = new EncryptWorkflow<PlainTextKey, NullKeyLoaderDetails>(
                new PlainTextKeyLoader(),
                new PlainTextKeySuitabilityChecker(),
                new Utf16LittleEndianUserStringConverter(),
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
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(options.StringToEncrypt), "string to add as plaintext cannot be null or empty");
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

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(IsBuilt == (_workflow != null));
        }
    }
}
