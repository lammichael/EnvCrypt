using System.Diagnostics.Contracts;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Utils.IO.StringWriter;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister.Symmetric
{
    public static class SymmetricKeyPersisterFactory
    {
        public static SymmetricKeyFilePersister<AesKey, EnvCryptKey, StringToFileWriterOptions> GetAesKeyPersister()
        {
            Contract.Ensures(Contract.Result<SymmetricKeyFilePersister<AesKey, EnvCryptKey, StringToFileWriterOptions>>() != null);
            //
            return GetAesKeyPersister(new StringToFileWriter(new MyDirectory(), new MyFile()));
        }


        public static SymmetricKeyFilePersister<AesKey, EnvCryptKey, StringToFileWriterOptions> GetAesKeyPersister(IStringWriter<StringToFileWriterOptions> writer)
        {
            Contract.Ensures(Contract.Result<SymmetricKeyFilePersister<AesKey, EnvCryptKey, StringToFileWriterOptions>>() != null);
            //
            return new SymmetricKeyFilePersister<AesKey, EnvCryptKey, StringToFileWriterOptions>(
                new AesKeyToXmlMapper(new Base64PersistConverter()),
                new XmlSerializationUtils<EnvCryptKey>(), writer
                );
        }
    }
}
