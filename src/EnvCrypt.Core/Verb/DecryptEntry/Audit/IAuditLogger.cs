using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;

namespace EnvCrypt.Core.Verb.DecryptEntry.Audit
{
    /// <summary>
    /// To log decryption attempts for audit purposes. One reason as to why this is required
    /// is when one wants to know what exact processes are accessing which entries 
    /// to calcuate the impact of changes to the encrypted entries in the EC DAT file.
    /// </summary>
    [ContractClass(typeof(AuditLoggerContracts<,>))]
    public interface IAuditLogger<TKey, in TWorkflowOptions>
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        void LogDecryption(
            TWorkflowOptions withWorkflowOptions,
            EnvCryptDat ecDat,
            IList<TKey> usingLoadedKeys,
            IList<EntriesDecrypterResult<TKey>> results);
    }


    [ContractClassFor(typeof(IAuditLogger<,>))]
    internal abstract class AuditLoggerContracts<TKey, TWorkflowOptions> : IAuditLogger<TKey, TWorkflowOptions> 
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        public void LogDecryption(TWorkflowOptions withWorkflowOptions, EnvCryptDat ecDat, IList<TKey> usingLoadedKeys, IList<EntriesDecrypterResult<TKey>> results)
        {
            Contract.Requires<ArgumentNullException>(withWorkflowOptions != null, "withWorkflowOptions");
            Contract.Requires<ArgumentNullException>(ecDat != null, "ecDat");
            Contract.Requires<ArgumentNullException>(usingLoadedKeys != null, "usingLoadedKeys");
            Contract.Requires<EnvCryptException>(Contract.ForAll(usingLoadedKeys, k => k != null), "no loaded keys can be null");
            Contract.Requires<ArgumentNullException>(results != null, "results");
            Contract.Requires<EnvCryptException>(Contract.ForAll(results, r => r != null), "no results can be null");
        }
    }
}
