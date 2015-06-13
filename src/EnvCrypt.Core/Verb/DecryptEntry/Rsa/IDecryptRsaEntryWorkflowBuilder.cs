using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.DecryptEntry.Rsa
{
    public interface IDecryptRsaEntryWorkflowBuilder
    {
        DecryptRsaEntryWorkflowBuilder WithKeyLoader(IKeyLoader<RsaKey, KeyFromFileDetails> keyLoader);
        DecryptRsaEntryWorkflowBuilder WithDatLoader(IDatLoader datLoader);
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
}