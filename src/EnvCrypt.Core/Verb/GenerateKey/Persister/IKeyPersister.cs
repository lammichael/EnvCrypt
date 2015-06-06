using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    [ContractClass(typeof (KeyPersisterContracts<,>))]
    public interface IKeyPersister<in TKeyPoco, in TOptions>
        where TKeyPoco : KeyBase
        where TOptions : KeyPersisterOptions
    {
        void Persist(TKeyPoco thisKey, TOptions withOptions);
    }


    [ContractClassFor(typeof(IKeyPersister<,>))]
    internal abstract class KeyPersisterContracts<TKeyPoco, TOptions> : IKeyPersister<TKeyPoco, TOptions>
        where TKeyPoco : KeyBase where TOptions : KeyPersisterOptions
    {
        public void Persist(TKeyPoco thisKey, TOptions withOptions)
        {
            Contract.Requires<ArgumentNullException>(thisKey != null, "thisKey");
            Contract.Requires<ArgumentNullException>(withOptions != null, "toDesiredFullKeyFilePath");
        }
    }
}