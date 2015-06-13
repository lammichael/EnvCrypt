using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry
{
    class AddEntryUsingKeyFileWorkflow<TKey, TWorkflowDetails> : AddEntryAndLoadSaveDatToFileWorkflow<TKey, TWorkflowDetails, KeyFromFileDetails>
        where TKey : KeyBase
        where TWorkflowDetails : AddEntryUsingKeyFileWorkflowOptions

    {
        public AddEntryUsingKeyFileWorkflow(EncryptWorkflow<TKey, KeyFromFileDetails> encryptWorkflow, IDatLoader<DatFromFileLoaderOptions> datLoader, IDatSaver<DatToFileSaverOptions> datSaver)
            : base(encryptWorkflow, datLoader, datSaver)
        {}


        protected override KeyFromFileDetails GetKeyLoaderDetails(TWorkflowDetails workflowDetails)
        {
            return new KeyFromFileDetails()
            {
                FilePath = workflowDetails.KeyFilePath
            };
        }
    }
}