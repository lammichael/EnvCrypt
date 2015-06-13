using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.AddEntry.PlainText;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry
{
    /// <summary>
    /// Loads the DAT from file and saves it to file.
    /// </summary>
    abstract class AddEntryAndLoadSaveDatToFileWorkflow<TKey, TWorkflowOptions, TKeyLoaderOptions> : AddEntryWorkflow<TKey, TWorkflowOptions, TKeyLoaderOptions, DatFromFileLoaderOptions, DatToFileSaverOptions> 
        where TKey : KeyBase 
        where TWorkflowOptions : AddPlainTextEntryWorkflowOptions
    {
        protected AddEntryAndLoadSaveDatToFileWorkflow(EncryptWorkflow<TKey, TKeyLoaderOptions> encryptWorkflow, IDatLoader<DatFromFileLoaderOptions> datLoader, IDatSaver<DatToFileSaverOptions> datSaver)
            : base(encryptWorkflow, datLoader, datSaver)
        {}

        protected override DatFromFileLoaderOptions GetDatLoaderOptions(TWorkflowOptions workflowDetails)
        {
            return new DatFromFileLoaderOptions()
            {
                DatFilePath = workflowDetails.DatFilePath
            };
        }


        protected override DatToFileSaverOptions GetDatSaverOptions(TWorkflowOptions workflowDetails)
        {
            return new DatToFileSaverOptions()
            {
                DestinationFilePath = workflowDetails.DatFilePath
            };
        }
    }
}