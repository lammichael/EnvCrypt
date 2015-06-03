using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry.PlainText
{
    class AddPlainTextEntryWorkflow<TKey, TWorkflowDetails> : AddEntryWorkflow<TKey, TWorkflowDetails, NullKeyLoaderDetails>
        where TKey : KeyBase
        where TWorkflowDetails : AddPlainTextEntryWorkflowOptions
    {
        public AddPlainTextEntryWorkflow(EncryptWorkflow<TKey, NullKeyLoaderDetails> encryptWorkflow, IDatLoader datLoader, IDatSaver<DatToFileSaverDetails> datSaver)
            : base(encryptWorkflow, datLoader, datSaver)
        {}


        protected override NullKeyLoaderDetails GetKeyLoaderDetails(TWorkflowDetails workflowDetails)
        {
            return null;
        }
    }
}