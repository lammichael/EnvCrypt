using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.Poco;

namespace EnvCrypt.Core.Verb.SaveDat
{
    [ContractClass(typeof(DatSaverContracts<>))]
    public interface IDatSaver<in TSaverDetails>
    {
        void Save(EnvCryptDat data, TSaverDetails saverDetails);
    }


    [ContractClassFor(typeof(IDatSaver<>))]
    internal abstract class DatSaverContracts<TSaverDetails> : IDatSaver<TSaverDetails>
    {
        public void Save(EnvCryptDat data, TSaverDetails saverDetails)
        {
            Contract.Requires<ArgumentNullException>(data != null, "data");
        }
    }
}