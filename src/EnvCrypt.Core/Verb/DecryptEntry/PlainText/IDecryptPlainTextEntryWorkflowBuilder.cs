using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;

namespace EnvCrypt.Core.Verb.DecryptEntry.PlainText
{
    [ContractClass(typeof(DecryptPlainTextEntryWorkflowBuilderContracts))]
    public interface IDecryptPlainTextEntryWorkflowBuilder
    {
        IDecryptPlainTextEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader);
        IDecryptPlainTextEntryWorkflowBuilder WithAuditLogger(IAuditLogger<PlainTextKey, DecryptPlainTextEntryWorkflowOptions> auditLogger);

        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="DecryptPlainTextEntryWorkflowBuilder.Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        IDecryptPlainTextEntryWorkflowBuilder Build();

        IList<EntriesDecrypterResult<PlainTextKey>> Run(DecryptPlainTextEntryWorkflowOptions options);

        [Pure]
        bool IsBuilt { get; }
    }


    [ContractClassFor(typeof(IDecryptPlainTextEntryWorkflowBuilder))]
    internal abstract class DecryptPlainTextEntryWorkflowBuilderContracts : IDecryptPlainTextEntryWorkflowBuilder
    {
        public IDecryptPlainTextEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Ensures(Contract.Result<IDecryptPlainTextEntryWorkflowBuilder>() != null);

            return default(IDecryptPlainTextEntryWorkflowBuilder);
        }

        public IDecryptPlainTextEntryWorkflowBuilder WithAuditLogger(IAuditLogger<PlainTextKey, DecryptPlainTextEntryWorkflowOptions> auditLogger)
        {
            Contract.Requires<ArgumentNullException>(auditLogger != null, "auditLogger");
            Contract.Ensures(Contract.Result<IDecryptPlainTextEntryWorkflowBuilder>() != null);

            return default(IDecryptPlainTextEntryWorkflowBuilder);
        }

        public IDecryptPlainTextEntryWorkflowBuilder Build()
        {
            Contract.Ensures(Contract.Result<IDecryptPlainTextEntryWorkflowBuilder>() != null);

            return default(IDecryptPlainTextEntryWorkflowBuilder);
        }

        public IList<EntriesDecrypterResult<PlainTextKey>> Run(DecryptPlainTextEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Category)),
                "none of the category names can be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Entry)),
                "none of the entry names can be null or whitespace");

            Contract.Ensures(Contract.Result<IList<EntriesDecrypterResult<PlainTextKey>>>() != null);

            return default(IList<EntriesDecrypterResult<PlainTextKey>>);
        }

        public bool IsBuilt { get; private set; }
    }
}