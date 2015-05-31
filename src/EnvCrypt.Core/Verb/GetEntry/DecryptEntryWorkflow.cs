using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.LoadKey;

namespace EnvCrypt.Core.Verb.GetEntry
{
    public class DecryptEntryWorkflow<TKey>
        where TKey : KeyBase
    {
        private readonly IKeyLoader<TKey> _keyLoader;
        private readonly IDatLoader _datLoader;
        private readonly EntriesDecrypter<TKey> _entriesDecrypter;

        public DecryptEntryWorkflow(IKeyLoader<TKey> keyLoader, IDatLoader datLoader, EntriesDecrypter<TKey> entriesDecrypter)
        {
            Contract.Requires<ArgumentNullException>(keyLoader != null, "keyLoader");
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Requires<ArgumentNullException>(entriesDecrypter != null, "encryptWorkflow");
            //
            _datLoader = datLoader;
            _keyLoader = keyLoader;
            _entriesDecrypter = entriesDecrypter;
        }


        public IList<EntriesDecrypterResult> Run(DecryptEntryWorkflowOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");

            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(options.DatFilePath), "DAT file path cannot be null or whitespace");

            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryDetails, t => !string.IsNullOrWhiteSpace(t.Category)),
                "none of the category names can be null or whitespace");
            Contract.Requires<ArgumentException>(Contract.ForAll(options.CategoryEntryDetails, t => !string.IsNullOrWhiteSpace(t.Entry)),
                "none of the entry names can be null or whitespace");

            Contract.Requires<ArgumentException>(typeof(TKey) == typeof(PlainTextKey) || 
                Contract.ForAll(options.KeyFilePaths, s => !string.IsNullOrWhiteSpace(s)), 
                "key file path cannot be null or whitespace");
            //

            var datPoco = _datLoader.Load(options.DatFilePath);

            var keys = new List<TKey>(options.KeyFilePaths.Count);
            for (uint keyPathI = 0; keyPathI < options.KeyFilePaths.Count; keyPathI++)
            {
                var loadedKey = _keyLoader.Load(options.KeyFilePaths[(int)keyPathI]);
                keys.Add(loadedKey);
            }


            return _entriesDecrypter.Decrypt(keys, datPoco, options.CategoryEntryDetails);
        }
    }
}
