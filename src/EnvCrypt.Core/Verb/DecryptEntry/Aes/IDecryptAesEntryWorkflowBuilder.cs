using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.DecryptEntry.Aes
{
    [ContractClass(typeof(DecryptAesEntryWorkflowBuilderContract))]
    public interface IDecryptAesEntryWorkflowBuilder
    {
        DecryptAesEntryWorkflowBuilder WithKeyLoader(IKeyLoader<AesKey, KeyFromFileDetails> keyLoader);
        DecryptAesEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader);
        DecryptAesEntryWorkflowBuilder WithAuditLogger(IAuditLogger<AesKey, DecryptEntryWorkflowOptions> auditLogger);

        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="DecryptAesEntryWorkflowBuilder.Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        DecryptAesEntryWorkflowBuilder Build();

        IList<EntriesDecrypterResult<AesKey>> Run(DecryptEntryWorkflowOptions options);

        [Pure]
        bool IsBuilt { get; }
    }


    [ContractClassFor(typeof(IDecryptAesEntryWorkflowBuilder))]
    internal abstract class DecryptAesEntryWorkflowBuilderContract : IDecryptAesEntryWorkflowBuilder
    {
        public DecryptAesEntryWorkflowBuilder WithKeyLoader(IKeyLoader<AesKey, KeyFromFileDetails> keyLoader)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            
            return default(DecryptAesEntryWorkflowBuilder);
        }

        public DecryptAesEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");

            return default(DecryptAesEntryWorkflowBuilder);
        }

        public DecryptAesEntryWorkflowBuilder WithAuditLogger(IAuditLogger<AesKey, DecryptEntryWorkflowOptions> auditLogger)
        {
            Contract.Requires<ArgumentNullException>(auditLogger != null, "auditLogger");

            return default(DecryptAesEntryWorkflowBuilder);

        }

        public DecryptAesEntryWorkflowBuilder Build()
        {
            Contract.Ensures(Contract.Result<DecryptAesEntryWorkflowBuilder>() != null);

            return default(DecryptAesEntryWorkflowBuilder);
        }

        public IList<EntriesDecrypterResult<AesKey>> Run(DecryptEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Category)),
                "none of the category names can be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Entry)),
                "none of the entry names can be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.KeyFilePaths, s => !string.IsNullOrWhiteSpace(s)),
                "key file path cannot be null or whitespace");

            return default(IList<EntriesDecrypterResult<AesKey>>);
        }

        public bool IsBuilt { get; private set; }
    }
}