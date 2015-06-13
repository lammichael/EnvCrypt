using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Verb.AddEntry.PlainText;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry
{
    /// <summary>
    /// Gets key and EC DAT from file, adds the encrypted entry, and saves the DAT file
    /// back to the same file location.
    /// </summary>
    public abstract class AddEntryWorkflow<TKey, TWorkflowOptions, TKeyLoaderOptions, TDatLoaderOptions, TDatSaverOptions>
        where TKey : KeyBase
        where TWorkflowOptions : AddPlainTextEntryWorkflowOptions
        where TDatLoaderOptions : IDatLoaderOptions
        where TDatSaverOptions : IDatSaverOptions
    {
        private readonly EncryptWorkflow<TKey, TKeyLoaderOptions> _encryptWorkflow;
        private readonly IDatLoader<TDatLoaderOptions> _datLoader;
        private readonly IDatSaver<TDatSaverOptions> _datSaver;

        protected AddEntryWorkflow(EncryptWorkflow<TKey, TKeyLoaderOptions> encryptWorkflow, IDatLoader<TDatLoaderOptions> datLoader, IDatSaver<TDatSaverOptions> datSaver)
        {
            Contract.Requires<ArgumentNullException>(encryptWorkflow != null, "encryptWorkflow");
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Requires<ArgumentNullException>(datSaver != null, "datSaver");
            //
            _datLoader = datLoader;
            _datSaver = datSaver;
            _encryptWorkflow = encryptWorkflow;
        }


        public void Run(TWorkflowOptions workflowDetails)
        {
            Contract.Requires<ArgumentNullException>(workflowDetails != null, "options");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(workflowDetails.CategoryName), "category name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(workflowDetails.DatFilePath), "DAT file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(workflowDetails.EntryName), "entry name cannot be null or whitespace");
            //      PlainText decryption doesn't require a key
            //Contract.Requires<ArgumentException>(typeof(TKey) == typeof(PlainTextKey) || !string.IsNullOrWhiteSpace(options.KeyFilePath), "key file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(workflowDetails.StringToEncrypt), "string to encrypt cannot be null or empty");
            //
            /* 
             * Get key from file
             * Get Entries from file or create a new file if not already there
             * 
             * Map keys to entries
             * Encrypt value using key (or leave in plain text)
             * Add encrypted value to POCO
             * Write POCO to file
             */

            TKey key;
            var encryptedSegments = _encryptWorkflow.GetEncryptedSegments(GetKeyLoaderDetails(workflowDetails),
                workflowDetails.StringToEncrypt, out key);


            var datPoco = _datLoader.Load(GetDatLoaderOptions(workflowDetails));
            datPoco.AddEntry(workflowDetails.CategoryName, workflowDetails.EntryName, key, encryptedSegments, false);
            _datSaver.Save(datPoco, GetDatSaverOptions(workflowDetails));
        }


        protected abstract TKeyLoaderOptions GetKeyLoaderDetails(TWorkflowOptions workflowDetails);

        protected abstract TDatLoaderOptions GetDatLoaderOptions(TWorkflowOptions workflowDetails);

        protected abstract TDatSaverOptions GetDatSaverOptions(TWorkflowOptions workflowDetails);
    }
}
