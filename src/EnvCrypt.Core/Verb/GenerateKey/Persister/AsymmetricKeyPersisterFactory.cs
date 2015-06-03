using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    public static class AsymmetricKeyPersisterFactory
    {
        public static AsymmetricKeyPersister<RsaKey, EnvCryptKey> GetRsaKeyPersister()
        {
            Contract.Ensures(Contract.Result<AsymmetricKeyPersister<RsaKey, EnvCryptKey>>() != null);
            //
            return GetRsaKeyPersister(new TextWriter(new MyFile()));
        }


        public static AsymmetricKeyPersister<RsaKey, EnvCryptKey> GetRsaKeyPersister(ITextWriter writer)
        {
            Contract.Ensures(Contract.Result<AsymmetricKeyPersister<RsaKey, EnvCryptKey>>() != null);
            //
            return new RsaKeyPersister(
                new RsaKeyToXmlMapper(new Base64PersistConverter()),
                new XmlSerializationUtils<EnvCryptKey>(),
                new StringToFileWriter(new MyDirectory(), new MyFile(), writer));
        }
    }
}
