using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToDatPoco;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.DecryptEntry.Aes;
using EnvCrypt.Core.Verb.DecryptEntry.PlainText;
using EnvCrypt.Core.Verb.DecryptEntry.Rsa;
using EnvCrypt.Core.Verb.LoadDat;

namespace EnvCrypt.Core.Verb.DecryptEntry.Generic
{
    public class DecryptGenericWorkflowBuilder : GenericBuilder
    {
        private IDecryptPlainTextEntryWorkflowBuilder _plaintextWorkFlowBuilder;
        private IDecryptRsaEntryWorkflowBuilder _rsaWorkFlowBuilder;
        private IDecryptAesEntryWorkflowBuilder _aesWorkFlowBuilder;

        private DecryptGenericWorkflow _workflow;

        public DecryptGenericWorkflowBuilder(IDecryptPlainTextEntryWorkflowBuilder plaintextWorkFlowBuilder, IDecryptRsaEntryWorkflowBuilder rsaWorkFlowBuilder, IDecryptAesEntryWorkflowBuilder aesWorkFlowBuilder)
        {
            Contract.Requires<ArgumentNullException>(plaintextWorkFlowBuilder != null, "plaintextWorkFlowBuilder");
            Contract.Requires<ArgumentNullException>(rsaWorkFlowBuilder != null, "rsaWorkFlowBuilder");
            Contract.Requires<ArgumentNullException>(aesWorkFlowBuilder != null, "aesWorkFlowBuilder");
            //
            _plaintextWorkFlowBuilder = plaintextWorkFlowBuilder;
            _rsaWorkFlowBuilder = rsaWorkFlowBuilder;
            _aesWorkFlowBuilder = aesWorkFlowBuilder;
        }


        public DecryptGenericWorkflowBuilder WithDecryptPlainTextEntryWorkflowBuilder(IDecryptPlainTextEntryWorkflowBuilder builder)
        {
            Contract.Requires<ArgumentNullException>(builder != null, "builder");
            //
            _plaintextWorkFlowBuilder = builder;
            MarkAsNotBuilt();
            return this;
        }


        public DecryptGenericWorkflowBuilder WithDecryptRsaEntryWorkflowBuilder(IDecryptRsaEntryWorkflowBuilder builder)
        {
            Contract.Requires<ArgumentNullException>(builder != null, "builder");
            //
            _rsaWorkFlowBuilder = builder;
            MarkAsNotBuilt();
            return this;
        }


        public DecryptGenericWorkflowBuilder WithDecryptAesEntryWorkflowBuilder(IDecryptAesEntryWorkflowBuilder builder)
        {
            Contract.Requires<ArgumentNullException>(builder != null, "builder");
            //
            _aesWorkFlowBuilder = builder;
            MarkAsNotBuilt();
            return this;
        }


        /// <summary>
        /// Prepares the Builder ready for use. This must be called before your first call to the <see cref="Run"/> method.
        /// This method is idempotent.
        /// </summary>
        /// <returns>the same Builder instance</returns>
        public DecryptGenericWorkflowBuilder Build()
        {
            _workflow = new DecryptGenericWorkflow(new DatFromXmlFileLoader(new MyFile(), new TextReader(new MyFile()), new XmlSerializationUtils<EnvCryptEncryptedData>(), new XmlToDatMapper(new EncryptedDetailsPersistConverter(new Utf16LittleEndianUserStringConverter()))), _plaintextWorkFlowBuilder, _rsaWorkFlowBuilder, _aesWorkFlowBuilder);
            IsBuilt = true;
            return this;
        }


        public IList<EntriesDecrypterResult> Run(DecryptGenericWorkflowOptions options)
        {
            if (!IsBuilt)
            {
                throw new EnvCryptException("workflow cannot be run because it has not been built");
            }
            return _workflow.Run(options);
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
