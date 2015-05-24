using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Poco;

namespace EnvCrypt.Core.EncrypedData.Mapper
{
    [ContractClass(typeof(ExternalRepresentationToDatMapperContracts<>))]
    interface IExternalRepresentationToDatMapper<in TExtRep>
        where TExtRep : class, IDataExternalRepresentation
    {
        /// <summary>
        /// Maps from external representation to EnvCrypt DAT POCO.
        /// </summary>
        /// <returns>EnvCrypt DAT POCO with data taken from the external representation POCO, or an empty DAT POCO if the external representation was empty</returns>
        EnvCryptDat Map(TExtRep fromExternalRepresentationPoco);
    }


    [ContractClassFor(typeof(IExternalRepresentationToDatMapper<>))]
    internal abstract class ExternalRepresentationToDatMapperContracts<TExtRep>
        : IExternalRepresentationToDatMapper<TExtRep>
        where TExtRep : class, IDataExternalRepresentation
    {
        public EnvCryptDat Map(TExtRep fromExternalRepresentationPoco)
        {
            Contract.Requires<ArgumentNullException>(fromExternalRepresentationPoco != null, "fromExternalRepresentationPoco");
            Contract.Ensures(Contract.Result<EnvCryptDat>() != null);
            //      Categories and Entries in each Category cannot be null, but they can be empty
            Contract.Ensures(Contract.Result<EnvCryptDat>().Categories != null);
            Contract.Ensures(
                Contract.ForAll(Contract.Result<EnvCryptDat>().Categories,
                    c => c.Entries != null));

            return default(EnvCryptDat);
        }
    }
}