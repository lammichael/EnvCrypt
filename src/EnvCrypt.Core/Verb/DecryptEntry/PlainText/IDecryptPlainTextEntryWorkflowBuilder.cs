using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;

namespace EnvCrypt.Core.Verb.DecryptEntry.PlainText
{
    public interface IDecryptPlainTextEntryWorkflowBuilder
    {
        DecryptPlainTextEntryWorkflowBuilder WithDatLoader(IDatLoader datLoader);
        DecryptPlainTextEntryWorkflowBuilder WithAuditLogger(IAuditLogger<PlainTextKey, DecryptPlainTextEntryWorkflowOptions> auditLogger);

        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="DecryptPlainTextEntryWorkflowBuilder.Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        DecryptPlainTextEntryWorkflowBuilder Build();

        IList<EntriesDecrypterResult<PlainTextKey>> Run(DecryptPlainTextEntryWorkflowOptions options);

        [Pure]
        bool IsBuilt { get; }
    }
}