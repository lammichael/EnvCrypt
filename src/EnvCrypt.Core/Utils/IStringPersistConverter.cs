using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Utils
{
    /// <summary>
    /// Defines how EnvCrypt persists binary to a String for storage to file.
    /// </summary>
    [ContractClass(typeof(StringPersistConverterContract))]
    interface IStringPersistConverter
    {
        string Encode(byte[] dataToPersist);
        byte[] Decode(string persistedStr);
    }


    [ContractClassFor(typeof(IStringPersistConverter))]
    internal abstract class StringPersistConverterContract : IStringPersistConverter
    {
        public string Encode(byte[] dataToPersist)
        {
            Contract.Requires<ArgumentNullException>(dataToPersist != null, "str");
            Contract.Ensures(Contract.Result<string>() != null);

            return default(string);
        }


        public byte[] Decode(string persistedStr)
        {
            Contract.Requires<ArgumentNullException>(persistedStr != null, "str");
            Contract.Ensures(Contract.Result<byte[]>() != null);

            return default(byte[]);
        }
    }
}
