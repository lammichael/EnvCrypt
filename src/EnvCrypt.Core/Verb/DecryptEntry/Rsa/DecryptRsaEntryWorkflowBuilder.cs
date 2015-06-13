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
    public class DecryptRsaEntryWorkflowBuilder : GenericBuilder, IDecryptRsaEntryWorkflowBuilder
    {
        private IKeyLoader<RsaKey, KeyFromFileDetails> _keyLoader;
        private IDatLoader<DatFromFileLoaderOptions> _datLoader;
        private IAuditLogger<RsaKey, DecryptEntryWorkflowOptions> _auditLogger;

        private DecryptEntryWorkflow<RsaKey, DecryptEntryWorkflowOptions, DatFromFileLoaderOptions> _workflow;

        public DecryptRsaEntryWorkflowBuilder()
        {
            _keyLoader = LoadKeyFromXmlFileFactory.GetRsaKeyLoader();
            _datLoader = DatFromXmlFileFactory.GetDatLoader();
            _auditLogger = new NullAuditLogger<RsaKey, DecryptEntryWorkflowOptions>();
        }


        public IDecryptRsaEntryWorkflowBuilder WithKeyLoader(IKeyLoader<RsaKey, KeyFromFileDetails> keyLoader)
        {
            _keyLoader = keyLoader;
            MarkAsNotBuilt();
            return this;
        }


        public IDecryptRsaEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader)
        {
            _datLoader = datLoader;
            MarkAsNotBuilt();
            return this;
        }


        public IDecryptRsaEntryWorkflowBuilder WithAuditLogger(IAuditLogger<RsaKey, DecryptEntryWorkflowOptions> auditLogger)
        {
            _auditLogger = auditLogger;
            MarkAsNotBuilt();
            return this;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public IDecryptRsaEntryWorkflowBuilder Build()
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
