using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.RemoveEntry
{
    /// <summary>
    /// Removes entries from the DAT file. If entry does not exist, no exception is thrown.
    /// </summary>
    public class RemoveEntryWorkflow<TDatLoaderOptions, TDatSaverOptions>
        where TDatLoaderOptions : class, IDatLoaderOptions
        where TDatSaverOptions : class, IDatSaverOptions
    {
        private readonly IDatLoader<TDatLoaderOptions> _datLoader;
        private readonly IDatSaver<TDatSaverOptions> _datSaver;

        public RemoveEntryWorkflow(IDatLoader<TDatLoaderOptions> datLoader, IDatSaver<TDatSaverOptions> datSaver)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            Contract.Requires<ArgumentNullException>(datSaver != null, "datSaver");
            //
            _datLoader = datLoader;
            _datSaver = datSaver;
        }


        public void Run(TDatLoaderOptions datLoaderOptions, IList<CategoryEntryPair> entriesToRemove, TDatSaverOptions datSaverOptions)
        {
            var dat = _datLoader.Load(datLoaderOptions);

            for (uint entryI = 0; entryI < entriesToRemove.Count; entryI++)
            {
                var currentEntryToRemove = entriesToRemove[(int)entryI];
                dat.RemoveEntry(currentEntryToRemove.Category, currentEntryToRemove.Entry);
            }

            _datSaver.Save(dat, datSaverOptions);
        }
    }
}
