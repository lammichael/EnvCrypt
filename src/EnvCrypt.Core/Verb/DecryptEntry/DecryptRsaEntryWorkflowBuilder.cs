using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToDatPoco;
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
using EnvCrypt.Core.Verb.LoadKey.Rsa;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    public class DecryptRsaEntryWorkflowBuilder
    {
        public bool IsBuilt { get; private set; }

        private DecryptEntryWorkflow<RsaKey> _workflow;
        private readonly DecryptEntryWorkflowOptions _options;

        public DecryptRsaEntryWorkflowBuilder(DecryptEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");

            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");

            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Category)),
                "none of the category names can be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Entry)),
                "none of the entry names can be null or whitespace");

            Contract.Requires<ArgumentException>(Contract.ForAll(options.KeyFilePaths, s => !string.IsNullOrWhiteSpace(s)),
                "key file path cannot be null or whitespace");
            //
            _options = options;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public DecryptRsaEntryWorkflowBuilder Build()
        {
            var myFile = new MyFile();

            var persistConverter = new Base64PersistConverter();

            var keyLoader = new RsaKeyFromXmlFileLoader(
                myFile,
                new TextReader(myFile),
                new XmlSerializationUtils<EnvCryptKey>(),
                new XmlToRsaKeyMapper(persistConverter));
            var datLoader = new DatLoader(
                myFile, 
                new TextReader(myFile), 
                new XmlSerializationUtils<EnvCryptEncryptedData>(), 
                new XmlToDatMapper(persistConverter));
            var entriesDecrypter = new EntriesDecrypter<RsaKey>(
                new RsaKeySuitabilityChecker(), 
                new Utf16LittleEndianUserStringConverter(), 
                new RsaSegmentEncryptionAlgo(new RsaAlgo(), new RsaMaxEncryptionCalc()));

            _workflow = new DecryptEntryWorkflow<RsaKey>(keyLoader, datLoader, entriesDecrypter);
            IsBuilt = true;
            return this;
        }


        public IList<EntriesDecrypterResult> Run()
        {
            if (!IsBuilt)
            {
                throw new EnvCryptException("workflow cannot be run because it has not been built");
            }
            return _workflow.Run(_options);
        }


        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(IsBuilt == (_workflow != null));
        }
    }
}
