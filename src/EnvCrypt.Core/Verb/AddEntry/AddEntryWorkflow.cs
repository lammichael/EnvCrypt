using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.AddEntry
{
    public class AddEntryWorkflow<TKey>
        where TKey : KeyBase
    {
        private readonly IKeyLoader<TKey> _keyLoader;
        private readonly IUserStringConverter _userStringConverter;
        private readonly ISegmentEncryptionAlgo<TKey> _segmentEncrypter;

        private readonly IDatLoader _datLoader;
        private readonly IDatSaver<EnvCryptEncryptedData> _datSaver;

        public AddEntryWorkflow(IKeyLoader<TKey> keyLoader,
            IUserStringConverter userStringConverter,
            ISegmentEncryptionAlgo<TKey> segmentEncrypter,
            IDatLoader datLoader, IDatSaver<EnvCryptEncryptedData> datSaver)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            Contract.Requires<ArgumentNullException>(userStringConverter != null, "userStringConverter");
            Contract.Requires<ArgumentNullException>(segmentEncrypter != null, "segmentEncrypter");
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Requires<ArgumentNullException>(datSaver != null, "datSaver");
            //
            _keyLoader = keyLoader;
            _userStringConverter = userStringConverter;
            _segmentEncrypter = segmentEncrypter;
            _datLoader = datLoader;
            _datSaver = datSaver;
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

            var key = _keyLoader.Load(options.KeyFilePath);

            var binaryToEncrypt = _userStringConverter.Encode(options.StringToEncrypt);
            var encryptedSegments = _segmentEncrypter.Encrypt(binaryToEncrypt, key);

            var datPoco = _datLoader.Load(options.DatFilePath);
            datPoco.AddEntry(options.CategoryName, options.EntryName, key, encryptedSegments, false);
            _datSaver.Save(datPoco, options.DatFilePath);
        }
    }
}
