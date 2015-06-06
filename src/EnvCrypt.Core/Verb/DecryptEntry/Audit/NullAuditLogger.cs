using System.Collections.Generic;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;

namespace EnvCrypt.Core.Verb.DecryptEntry.Audit
{
    /// <summary>
    /// Logs nothing.
    /// </summary>
    public class NullAuditLogger<TKey, TWorkflowOptions> : IAuditLogger<TKey, TWorkflowOptions> 
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        public void LogDecryption(TWorkflowOptions withWorkflowOptions, EnvCryptDat ecDat, IList<TKey> usingLoadedKeys, IList<EntriesDecrypterResult<TKey>> results)
        {}
    }
}