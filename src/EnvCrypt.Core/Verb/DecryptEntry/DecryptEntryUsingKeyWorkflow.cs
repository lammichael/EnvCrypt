using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    class DecryptEntryUsingKeyWorkflow<TKey, TWorkflowOptions> : DecryptEntryUsingDatFileWorkflow<TKey, TWorkflowOptions>
        where TKey : KeyBase
        where TWorkflowOptions : DecryptEntryWorkflowOptions
    {
        private readonly IKeyLoader<TKey, KeyFromFileDetails> _keyLoader;

        public DecryptEntryUsingKeyWorkflow(IDatLoader<DatFromFileLoaderOptions> datLoader, EntriesDecrypter<TKey> entriesDecrypter, IAuditLogger<TKey, TWorkflowOptions> auditLogger, IKeyLoader<TKey, KeyFromFileDetails> keyLoader)
            : base(datLoader, entriesDecrypter, auditLogger)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            //
            _keyLoader = keyLoader;
        }

        protected override List<TKey> LoadKeys(TWorkflowOptions workflowOptions)
        {
            var keys = new List<TKey>(workflowOptions.KeyFilePaths.Count);
            for (uint keyPathI = 0; keyPathI < workflowOptions.KeyFilePaths.Count; keyPathI++)
            {
                var loadedKey = _keyLoader.Load(
                    new KeyFromFileDetails() { FilePath = workflowOptions.KeyFilePaths[(int)keyPathI] });
                keys.Add(loadedKey);
            }

            return keys;
        }
    }
}