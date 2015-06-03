using System;
using System.Diagnostics.Contracts;
using System.Text;

namespace EnvCrypt.Core.Utils
{
    [ContractClass(typeof(XmlSerializationUtilsContracts<>))]
    public interface IXmlSerializationUtils<T> where T : class
    {
        [Pure]
        string Serialize(T poco);
        [Pure]
        T Deserialize(string xml);
        [Pure]
        Encoding GetUsedEncoding();
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

        public Encoding GetUsedEncoding()
        {
            Contract.Ensures(Contract.Result<Encoding>() != null);

            return default(Encoding);
        }
    }
}