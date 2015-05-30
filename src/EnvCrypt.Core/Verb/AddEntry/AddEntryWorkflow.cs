using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry
{
    public class AddEntryWorkflow<TKey>
        where TKey : KeyBase
    {
        private readonly EncryptWorkflow<TKey> _encryptWorkflow;
        private readonly IDatLoader _datLoader;
        private readonly IDatSaver<EnvCryptEncryptedData> _datSaver;

        public AddEntryWorkflow(EncryptWorkflow<TKey> encryptWorkflow, IDatLoader datLoader, IDatSaver<EnvCryptEncryptedData> datSaver)
        {
            Contract.Requires<ArgumentNullException>(encryptWorkflow != null, "encryptWorkflow");
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Requires<ArgumentNullException>(datSaver != null, "datSaver");
            //
            _datLoader = datLoader;
            _datSaver = datSaver;
            _encryptWorkflow = encryptWorkflow;
        }


        public void Run(AddEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.CategoryName), "category name cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.EntryName), "entry name cannot be null or whitespace");
            //      PlainText decryption doesn't require a key
            Contract.Requires<ArgumentException>(typeof(TKey) == typeof(PlainTextKey) || !string.IsNullOrWhiteSpace(options.KeyFilePath), "key file path cannot be null or whitespace");
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(options.StringToEncrypt), "string to encrypt cannot be null or empty");
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
            var encryptedSegments = _encryptWorkflow.GetEncryptedSegments(options.KeyFilePath, options.StringToEncrypt, out key);

            var datPoco = _datLoader.Load(options.DatFilePath);
            datPoco.AddEntry(options.CategoryName, options.EntryName, key, encryptedSegments, false);
            _datSaver.Save(datPoco, options.DatFilePath);
        }
    }
}
