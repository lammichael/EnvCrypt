using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    [ContractClass(typeof (KeyFilePersisterContracts<,,>))]
    internal interface IKeyFilePersister<in TKeyPoco, TKeyXmlPoco, in TOptions>
        where TKeyPoco : KeyBase
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
        where TOptions : KeyFilePersisterOptions
    {
        void WriteToFile(TKeyPoco thisKey, TOptions withOptions);
    }


    [ContractClassFor(typeof(IKeyFilePersister<,,>))]
    internal abstract class KeyFilePersisterContracts<TKeyPoco, TKeyXmlPoco, TOptions> : IKeyFilePersister<TKeyPoco, TKeyXmlPoco, TOptions>
        where TKeyPoco : KeyBase
        where TKeyXmlPoco : IKeyExternalRepresentation<TKeyPoco>
        where TOptions : KeyFilePersisterOptions
    {
        public void WriteToFile(TKeyPoco thisKey, TOptions toDesiredFullKeyFilePath)
        {
            Contract.Requires<ArgumentNullException>(thisKey != null, "thisKey");
            Contract.Requires<ArgumentNullException>(toDesiredFullKeyFilePath != null, "toDesiredFullKeyFilePath");
        }
    }
}