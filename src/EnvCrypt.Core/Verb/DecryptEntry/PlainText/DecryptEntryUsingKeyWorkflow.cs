using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.DecryptEntry.PlainText
{
    class DecryptPlainTextEntryWorkflow<TKey, TWorkflowOptions> : DecryptEntryWorkflow<TKey, TWorkflowOptions>
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        private readonly IKeyLoader<TKey, NullKeyLoaderDetails> _keyLoader;

        public DecryptPlainTextEntryWorkflow(IDatLoader datLoader, EntriesDecrypter<TKey> entriesDecrypter, IKeyLoader<TKey, NullKeyLoaderDetails> keyLoader)
            : base(datLoader, entriesDecrypter)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            //
            _keyLoader = keyLoader;
        }


        protected override List<TKey> LoadKeys(TWorkflowOptions workflowOptions)
        {
            return new List<TKey>() {_keyLoader.Load(new NullKeyLoaderDetails())};
        }
    }
}