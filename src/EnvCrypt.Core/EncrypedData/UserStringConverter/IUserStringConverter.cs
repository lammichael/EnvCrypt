using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace EnvCrypt.Core.EncrypedData.UserStringConverter
{
    /// <summary>
    /// To convert from user inputted string to a binary 
    /// because the encryption algorithms use require a byte array
    /// as input.
    /// </summary>
    [ContractClass(typeof(UserStringConverterContracts))]
    public interface IUserStringConverter
    {
        byte[] Encode(string userStr);
        string Decode(byte[] decrypedData);
    }


    [ContractClassFor(typeof(IUserStringConverter))]
    internal abstract class UserStringConverterContracts : IUserStringConverter
    {
        public byte[] Encode(string userStr)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(userStr), "userStr");
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Ensures(Contract.Result<byte[]>().Any());

            return default(byte[]);
        }

        public string Decode(byte[] decrypedData)
        {
            Contract.Requires<ArgumentNullException>(decrypedData != null, "decrypedData");
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            return default(string);
        }
    }
}
