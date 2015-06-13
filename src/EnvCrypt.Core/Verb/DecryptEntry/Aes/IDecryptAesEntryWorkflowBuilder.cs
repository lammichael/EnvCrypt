using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.DecryptEntry.Aes
{
    public interface IDecryptAesEntryWorkflowBuilder
    {
        DecryptAesEntryWorkflowBuilder WithKeyLoader(IKeyLoader<AesKey, KeyFromFileDetails> keyLoader);
        DecryptAesEntryWorkflowBuilder WithDatLoader(IDatLoader datLoader);
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
}