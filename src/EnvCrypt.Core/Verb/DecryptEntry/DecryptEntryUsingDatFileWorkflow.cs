using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.DecryptEntry.Audit;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.LoadDat;

namespace EnvCrypt.Core.Verb.DecryptEntry
{
    abstract class DecryptEntryUsingDatFileWorkflow<TKey, TWorkflowOptions> : DecryptEntryWorkflow<TKey, TWorkflowOptions, DatFromFileLoaderOptions>
        where TKey : KeyBase where TWorkflowOptions : DecryptPlainTextEntryWorkflowOptions
    {
        protected DecryptEntryUsingDatFileWorkflow(IDatLoader<DatFromFileLoaderOptions> datLoader, EntriesDecrypter<TKey> entriesDecrypter, IAuditLogger<TKey, TWorkflowOptions> auditLogger) : base(datLoader, entriesDecrypter, auditLogger)
        {}

        protected override DatFromFileLoaderOptions GetDatLoaderOptions(TWorkflowOptions workflowOptions)
        {
            return new DatFromFileLoaderOptions()
            {
                DatFilePath = workflowOptions.DatFilePath
            };
        }
    }
}