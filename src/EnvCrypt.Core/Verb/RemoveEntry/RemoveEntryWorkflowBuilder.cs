using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Verb.DecryptEntry;
using EnvCrypt.Core.Verb.LoadDat;
using EnvCrypt.Core.Verb.SaveDat;

namespace EnvCrypt.Core.Verb.RemoveEntry
{
    /// <summary>
    /// Builds workflow to remove entries with DAT to and from a file.
    /// </summary>
    public class RemoveEntryWorkflowBuilder : GenericBuilder
    {
        private IDatLoader<DatFromFileLoaderOptions> _datLoader;
        private IDatSaver<DatToFileSaverOptions> _datSaver;

        private RemoveEntryWorkflow<DatFromFileLoaderOptions, DatToFileSaverOptions> _workflow;

        public RemoveEntryWorkflowBuilder()
        {
            _datLoader = DatFromXmlFileFactory.GetDatLoader();
            _datSaver = DatXmlFileSaverFactory.GetDatSaver();
        }


        public RemoveEntryWorkflowBuilder WithDatLoader(IDatLoader<DatFromFileLoaderOptions> datLoader)
        {
            Contract.Requires<ArgumentNullException>(datLoader != null, "datLoader");
            //
            _datLoader = datLoader;
            MarkAsNotBuilt();
            return this;
        }


        public RemoveEntryWorkflowBuilder WithDatSaver(IDatSaver<DatToFileSaverOptions> datSaver)
        {
            Contract.Requires<ArgumentNullException>(datSaver != null, "datSaver");
            //
            _datSaver = datSaver;
            MarkAsNotBuilt();
            return this;
        }


        public RemoveEntryWorkflowBuilder Build()
        {
            _workflow = new RemoveEntryWorkflow<DatFromFileLoaderOptions, DatToFileSaverOptions>(_datLoader, _datSaver);

            IsBuilt = true;
            return this;
        }


        public void Run(DatFromFileLoaderOptions datLoaderOptions, IList<CategoryEntryPair> entriesToRemove, DatToFileSaverOptions datSaverOptions)
        {
            _workflow.Run(datLoaderOptions, entriesToRemove, datSaverOptions);
        }


        protected override void SetWorkflowToNull()
        {
            _workflow = null;
        }

        protected override bool IsWorkflowNull()
        {
            return _workflow == null;
        }

        [ContractInvariantMethod]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(IsBuilt == (_workflow != null));
        }
    }
}
