using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData;
using EnvCrypt.Core.EncrypedData.Poco;

namespace EnvCrypt.Core.Verb.SaveDat
{
    [ContractClass(typeof(DatSaverContracts<>))]
    public interface IDatSaver<TExtRep>
        where TExtRep : IDataExternalRepresentation
    {
        void Save(EnvCryptDat data, string toFile);
    }


    [ContractClassFor(typeof(IDatSaver<>))]
    internal abstract class DatSaverContracts<TExtRep> : IDatSaver<TExtRep> 
        where TExtRep : IDataExternalRepresentation
    {
        public void Save(EnvCryptDat data, string toFile)
        {
            Contract.Requires<ArgumentNullException>(data != null, "data");
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(toFile), "toFile");
        }
    }
}