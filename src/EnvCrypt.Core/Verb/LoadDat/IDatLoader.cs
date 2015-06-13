using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Poco;

namespace EnvCrypt.Core.Verb.LoadDat
{
    [ContractClass(typeof(DatLoaderContracts<>))]
    public interface IDatLoader<in TOptions>
        where TOptions : IDatLoaderOptions
    {
        EnvCryptDat Load(TOptions options);
    }


    [ContractClassFor(typeof(IDatLoader<>))]
    internal abstract class DatLoaderContracts<TOptions> : IDatLoader<TOptions>
        where TOptions : IDatLoaderOptions
    {
        public EnvCryptDat Load(TOptions options)
        {
            Contract.Requires<ArgumentNullException>(options != null, "options");
            Contract.Ensures(Contract.Result<EnvCryptDat>() != null);
            Contract.Ensures(Contract.Result<EnvCryptDat>().Categories != null);

            return default(EnvCryptDat);
        }
    }
}