using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Poco;

namespace EnvCrypt.Core.EncrypedData.Mapper
{
    [ContractClass(typeof(DatToExternalRepresentationMapperContracts<>))]
    interface IDatToExternalRepresentationMapper<out TExtRep>
        where TExtRep : class, IDataExternalRepresentation
    {
        /// <summary>
        /// Maps to an external representation POCO.
        /// Empty Entires and Categories are not included in the output.
        /// </summary>
        /// <param name="fromDatPoco">POCO representing the contents of a EnvCrypt DAT XML</param>
        /// <returns>POCO to serialise</returns>
        TExtRep Map(EnvCryptDat fromDatPoco);
    }


    [ContractClassFor(typeof(IDatToExternalRepresentationMapper<>))]
    internal abstract class DatToExternalRepresentationMapperContracts<TExtRep>
        : IDatToExternalRepresentationMapper<TExtRep>
        where TExtRep : class, IDataExternalRepresentation
    {
        public TExtRep Map(EnvCryptDat fromDatPoco)
        {
            Contract.Requires<ArgumentNullException>(fromDatPoco != null, "fromDatPoco");
            Contract.Requires<EnvCryptException>(fromDatPoco.Categories != null);
            Contract.Requires<EnvCryptException>(Contract.ForAll(fromDatPoco.Categories,
                c => !string.IsNullOrWhiteSpace(c.Name)));
            /*
             * There must be list of Entires (if 0 Entries then it is not emitted in the TExtRep object).
             * Each entry must have a Name but may not have other things that the 
             * plaintext key doesn't need to have, such as the key name and hash.
             */
            Contract.Requires<EnvCryptException>(Contract.ForAll(fromDatPoco.Categories,
                c => c.Entries != null));
            Contract.Requires<EnvCryptException>(Contract.ForAll(fromDatPoco.Categories,
                c => Contract.ForAll(
                    c.Entries, e => e.EncryptedValue != null)));
            Contract.Requires<EnvCryptException>(Contract.ForAll(fromDatPoco.Categories, c => Contract.ForAll(
                c.Entries, e => e.Name != null)));
            Contract.Requires<EnvCryptException>(Contract.ForAll(fromDatPoco.Categories,
                c => Contract.ForAll(
                    c.Entries, e => Contract.ForAll(e.EncryptedValue, bytes => bytes != null))));

            Contract.Ensures(Contract.Result<TExtRep>() != null);

            return default(TExtRep);
        }
    }
}
