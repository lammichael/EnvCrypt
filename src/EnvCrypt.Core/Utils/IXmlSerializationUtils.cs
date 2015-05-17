using System;
using System.Diagnostics.Contracts;

namespace EnvCrypt.Core.Utils
{
    [ContractClass(typeof(XmlSerializationUtilsContracts<>))]
    internal interface IXmlSerializationUtils<T> where T : class
    {
        string Serialize(T poco);
        T Deserialize(string xml);
    }


    [ContractClassFor(typeof(IXmlSerializationUtils<>))]
    internal abstract class XmlSerializationUtilsContracts<T> : IXmlSerializationUtils<T>
        where T : class
    {
        public string Serialize(T poco)
        {
            Contract.Requires<ArgumentNullException>(poco != null, "poco");
            Contract.Ensures(!String.IsNullOrEmpty(Contract.Result<string>()));

            return default(string);
        }

        public T Deserialize(string xml)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(xml), "xml");
            Contract.Ensures(Contract.Result<T>() != null);

            return default(T);
        }
    }
}