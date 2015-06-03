using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.Mapper.Xml.ToKeyPoco;
using EnvCrypt.Core.Key.PlainText;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Verb.LoadKey.Aes;
using EnvCrypt.Core.Verb.LoadKey.PlainText;
using EnvCrypt.Core.Verb.LoadKey.Rsa;

namespace EnvCrypt.Core.Verb.LoadKey
{
    public static class LoadKeyFromXmlFileFactory
    {
        public static IKeyLoader<RsaKey, KeyFromFileDetails> GetRsaKeyLoader()
        {
            Contract.Ensures(Contract.Result<IKeyLoader<RsaKey, KeyFromFileDetails>>() != null);
            //
            var myFile = new MyFile();

            var persistConverter = new Base64PersistConverter();

            return new RsaKeyFromXmlFileLoader(
                myFile,
                new TextReader(myFile),
                new XmlSerializationUtils<EnvCryptKey>(),
                new XmlToRsaKeyMapper(persistConverter));
        }


        public static IKeyLoader<AesKey, KeyFromFileDetails> GetAesKeyLoader()
        {
            Contract.Ensures(Contract.Result<IKeyLoader<AesKey, KeyFromFileDetails>>() != null);
            //
            var myFile = new MyFile();

            var persistConverter = new Base64PersistConverter();

            return new AesKeyFromXmlFileLoader(
                myFile,
                new TextReader(myFile),
                new XmlSerializationUtils<EnvCryptKey>(),
                new XmlToAesKeyMapper(persistConverter));
        }


        public static IKeyLoader<PlainTextKey, NullKeyLoaderDetails> GetPlainTextKeyLoader()
        {
            Contract.Ensures(Contract.Result<IKeyLoader<PlainTextKey, NullKeyLoaderDetails>>() != null);
            //
            return new PlainTextKeyLoader();
        }
    }
}
