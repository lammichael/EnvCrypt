using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.DecryptEntry.Rsa
{
    [ContractClass(typeof(DecryptRsaEntryWorkflowBuilderContracts))]
    public interface IDecryptRsaEntryWorkflowBuilder
    {
        DecryptRsaEntryWorkflowBuilder WithKeyLoader(IKeyLoader<RsaKey, KeyFromFileDetails> keyLoader);
        DecryptRsaEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader);
        DecryptRsaEntryWorkflowBuilder WithAuditLogger(IAuditLogger<RsaKey, DecryptEntryWorkflowOptions> auditLogger);

        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="DecryptRsaEntryWorkflowBuilder.Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        DecryptRsaEntryWorkflowBuilder Build();

        IList<EntriesDecrypterResult<RsaKey>> Run(DecryptEntryWorkflowOptions options);

        [Pure]
        bool IsBuilt { get; }
    }



    [ContractClassFor(typeof(IDecryptRsaEntryWorkflowBuilder))]
    internal abstract class DecryptRsaEntryWorkflowBuilderContracts : IDecryptRsaEntryWorkflowBuilder
    {
        public DecryptRsaEntryWorkflowBuilder WithKeyLoader(IKeyLoader<RsaKey, KeyFromFileDetails> keyLoader)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            Contract.Ensures(Contract.Result<DecryptRsaEntryWorkflowBuilder>() != null);

            return default(DecryptRsaEntryWorkflowBuilder);            
        }

        public DecryptRsaEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Ensures(Contract.Result<DecryptRsaEntryWorkflowBuilder>() != null);

            return default(DecryptRsaEntryWorkflowBuilder);
        }

        public DecryptRsaEntryWorkflowBuilder WithAuditLogger(IAuditLogger<RsaKey, DecryptEntryWorkflowOptions> auditLogger)
        {
            Contract.Requires<ArgumentNullException>(auditLogger != null, "auditLogger");
            Contract.Ensures(Contract.Result<DecryptRsaEntryWorkflowBuilder>() != null);

            return default(DecryptRsaEntryWorkflowBuilder);
        }

        public DecryptRsaEntryWorkflowBuilder Build()
        {
            Contract.Ensures(Contract.Result<DecryptRsaEntryWorkflowBuilder>() != null);
            return default(DecryptRsaEntryWorkflowBuilder);
        }

        public IList<EntriesDecrypterResult<RsaKey>> Run(DecryptEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            /*Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");*/
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Category)),
                "none of the category names can be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Entry)),
                "none of the entry names can be null or whitespace");

            Contract.Requires<ArgumentException>(Contract.ForAll(options.KeyFilePaths, s => !string.IsNullOrWhiteSpace(s)),
                "key file path cannot be null or whitespace");
            Contract.Ensures(Contract.Result<IList<EntriesDecrypterResult<RsaKey>>>() != null);

            return default(IList<EntriesDecrypterResult<RsaKey>>);
        }

        public bool IsBuilt { get; private set; }
    }
}