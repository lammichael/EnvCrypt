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
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry
{
    public class AddPlainTextEntryBuilder
    {
        public bool IsBuilt { get; private set; }

        private AddEntryWorkflow<PlainTextKey> _workflow;
        private readonly AddEntryWorkflowOptions _options;

        public AddPlainTextEntryBuilder(AddEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.CategoryName), "category name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.EntryName), "entry name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(string.IsNullOrWhiteSpace(options.KeyFilePath), "key file path is not required for plaintext encryption");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(options.StringToEncrypt), "string to encrypt cannot be null or empty");
            //
            _options = options;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public AddPlainTextEntryBuilder Build()
        {
            var myFile = new MyFile();

            var userStringConverter = new Utf16LittleEndianUserStringConverter();
            var persistConverter = new PlainTextPersistConverter(userStringConverter);

            var encryptWorkflow = new EncryptWorkflow<PlainTextKey>(
                new PlainTextKeyLoader(),
                new CanEncryptUsingPlainTextKeyChecker(),
                userStringConverter,
                new PlainTextSegmentEncryptionAlgo());
            var datLoader = new DatLoader(
                myFile,
                new TextReader(myFile),
                new XmlSerializationUtils<EnvCryptEncryptedData>(),
                new XmlToDatMapper(persistConverter));
            var datSaver = new DatToXmlFileSaver(
                new DatToXmlMapper(persistConverter),
                new XmlSerializationUtils<EnvCryptEncryptedData>(),
                new StringToFileWriter(new MyDirectory(), myFile, new TextWriter(myFile)));
            _workflow = new AddEntryWorkflow<PlainTextKey>(encryptWorkflow, datLoader, datSaver);
            IsBuilt = true;
            return this;
        }


        public void Run()
        {
            if (!IsBuilt)
            {
                throw new EnvCryptException("workflow cannot be run because it has not been built");
            }
            _workflow.Run(_options);
        }


        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(IsBuilt == (_workflow != null));
        }
    }
}
