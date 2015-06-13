using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.LoadDat;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    [ContractClass(typeof(DecryptEntryWorkflowContracts<,,>))]
    public abstract class DecryptEntryWorkflow<TKey, TWorkflowOptions, TDatLoaderOptions>
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
        where TDatLoaderOptions : IDatLoaderOptions
    {
        private readonly IDatLoader<TDatLoaderOptions> _datLoader;
        private readonly EntriesDecrypter<TKey> _entriesDecrypter;
        private readonly IAuditLogger<TKey, TWorkflowOptions> _auditLogger;

        protected DecryptEntryWorkflow(IDatLoader<TDatLoaderOptions> datLoader, EntriesDecrypter<TKey> entriesDecrypter, IAuditLogger<TKey, TWorkflowOptions> auditLogger)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Requires<ArgumentNullException>(entriesDecrypter != null, "encryptWorkflow");
            Contract.Requires<ArgumentNullException>(auditLogger != null, "auditLogger");
            //
            _datLoader = datLoader;
            _entriesDecrypter = entriesDecrypter;
            _auditLogger = auditLogger;
        }


        public IList<EntriesDecrypterResult<TKey>> Run(TWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");

            /*Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");*/

            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Category)),
                "none of the category names can be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryPair, t => !string.IsNullOrWhiteSpace(t.Entry)),
                "none of the entry names can be null or whitespace");

            /*Contract.Requires<ArgumentException>(typeof(TKey) == typeof(PlainTextKey) || 
                Contract.ForAll(options.KeyFilePaths, s => !string.IsNullOrWhiteSpace(s)), 
                "key file path cannot be null or whitespace");*/
            Contract.Ensures(Contract.Result<IList<EntriesDecrypterResult<TKey>>>() != null);
            //

            var datPoco = _datLoader.Load(GetDatLoaderOptions(options));
            var loadedKeys = LoadKeys(options);

            var ret = _entriesDecrypter.Decrypt(loadedKeys, datPoco, options.CategoryEntryPair, options.ThrowExceptionIfEntryNotFound, options.ThrowIfDecryptingKeyNotFound, options.ThrowIfKeyCannotDecrypt);

            _auditLogger.LogDecryption(options, datPoco, loadedKeys, ret);

            return ret;
        }


        protected abstract List<TKey> LoadKeys(TWorkflowOptions workflowOptions);

        protected abstract TDatLoaderOptions GetDatLoaderOptions(TWorkflowOptions workflowOptions);
    }


    [ContractClassFor(typeof(DecryptEntryWorkflow<,,>))]
    internal abstract class DecryptEntryWorkflowContracts<TKey, TWorkflowOptions, TDatLoaderOptions> : DecryptEntryWorkflow<TKey, TWorkflowOptions, TDatLoaderOptions>
        where TKey : KeyBase
        where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
        where TDatLoaderOptions : IDatLoaderOptions
    {
        protected DecryptEntryWorkflowContracts(IDatLoader<TDatLoaderOptions> datLoader, EntriesDecrypter<TKey> entriesDecrypter, IAuditLogger<TKey, TWorkflowOptions> auditLogger)
            : base(datLoader, entriesDecrypter, auditLogger)
        {}


        protected override List<TKey> LoadKeys(TWorkflowOptions workflowOptions)
        {
            Contract.Requires<ArgumentNullException>(workflowOptions != null, "workflowOptions");
            Contract.Ensures(Contract.Result<List<TKey>>() != null);
            Contract.Ensures(Contract.Result<List<TKey>>().Any());

            return default(List<TKey>);
        }


        protected override TDatLoaderOptions GetDatLoaderOptions(TWorkflowOptions workflowOptions)
        {
            Contract.Requires<ArgumentNullException>(workflowOptions != null, "workflowOptions");
            Contract.Ensures(Contract.Result<TDatLoaderOptions>() != null);

            return default(TDatLoaderOptions);
        }
    }
}
