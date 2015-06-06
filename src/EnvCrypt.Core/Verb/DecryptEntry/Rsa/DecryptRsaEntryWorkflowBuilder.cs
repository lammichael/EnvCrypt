using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncryptionAlgo.Rsa;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.DecryptEntry.Rsa
{
    public class DecryptRsaEntryWorkflowBuilder : GenericBuilder
    {
        private IKeyLoader<RsaKey, KeyFromFileDetails> _keyLoader;
        private IDatLoader _datLoader;
        private IAuditLogger<RsaKey, DecryptEntryWorkflowOptions> _auditLogger;

        private DecryptEntryWorkflow<RsaKey, DecryptEntryWorkflowOptions> _workflow;

        public DecryptRsaEntryWorkflowBuilder()
        {
            _keyLoader = LoadKeyFromXmlFileFactory.GetRsaKeyLoader();
            _datLoader = DatFromXmlFileFactory.GetDatLoader();
            _auditLogger = new NullAuditLogger<RsaKey, DecryptEntryWorkflowOptions>();
        }


        public DecryptRsaEntryWorkflowBuilder WithKeyLoader(IKeyLoader<RsaKey, KeyFromFileDetails> keyLoader)
        {
            _keyLoader = keyLoader;
            MarkAsNotBuilt();
            return this;
        }


        public DecryptRsaEntryWorkflowBuilder WithDatLoader(IDatLoader datLoader)
        {
            _datLoader = datLoader;
            MarkAsNotBuilt();
            return this;
        }


        public DecryptRsaEntryWorkflowBuilder WithAuditLogger(IAuditLogger<RsaKey, DecryptEntryWorkflowOptions> auditLogger)
        {
            Contract.Requires<ArgumentNullException>(auditLogger != null, "auditLogger");
            //
            _auditLogger = auditLogger;
            MarkAsNotBuilt();
            return this;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public DecryptRsaEntryWorkflowBuilder Build()
        {
            var entriesDecrypter = new EntriesDecrypter<RsaKey>(
                new RsaKeySuitabilityChecker(),
                new Utf16LittleEndianUserStringConverter(),
                new RsaSegmentEncryptionAlgo(new RsaAlgo(), new RsaMaxEncryptionCalc()));

            _workflow = new DecryptEntryUsingKeyWorkflow<RsaKey, DecryptEntryWorkflowOptions>(_datLoader, entriesDecrypter, _auditLogger, _keyLoader);
            IsBuilt = true;
            return this;
        }


        public IList<EntriesDecrypterResult<RsaKey>> Run(DecryptEntryWorkflowOptions options)
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
            if (!IsBuilt)
            {
                throw new EnvCryptException("workflow cannot be run because it has not been built");
            }
            return _workflow.Run(options);
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
