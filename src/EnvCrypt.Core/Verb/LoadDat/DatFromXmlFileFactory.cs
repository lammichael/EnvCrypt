using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToDatPoco;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadDat
{
    public static class DatFromXmlFileFactory
    {
        public static IDatLoader GetDatLoader()
        {
            Contract.Ensures(Contract.Result<IDatLoader>() != null);
            //
            var myFile = new MyFile();
            return new DatFromXmlFileLoader(
                myFile,
                new TextReader(myFile),
                new XmlSerializationUtils<EnvCryptEncryptedData>(),
                new XmlToDatMapper(new EncryptedDetailsPersistConverter(new Utf16LittleEndianUserStringConverter())));
        }
    }
}
