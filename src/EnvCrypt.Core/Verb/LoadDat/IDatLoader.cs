using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Poco;

namespace EnvCrypt.Core.Verb.LoadDat
{
    [ContractClass(typeof(DatLoaderContracts))]
    public interface IDatLoader
    {
        EnvCryptDat Load(string ecDatFilePath);
    }


    [ContractClassFor(typeof(IDatLoader))]
    internal abstract class DatLoaderContracts : IDatLoader
    {
        public EnvCryptDat Load(string ecDatFilePath)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(ecDatFilePath), "ecDatFilePath");
            Contract.Ensures(Contract.Result<EnvCryptDat>() != null);
            Contract.Ensures(Contract.Result<EnvCryptDat>().Categories != null);

            return default(EnvCryptDat);
        }
    }
}