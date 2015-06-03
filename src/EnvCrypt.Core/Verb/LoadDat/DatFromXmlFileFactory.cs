using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToDatPoco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadDat
{
    public static class DatFromXmlFileFactory
    {
         public static IDatLoader GetDatLoader()
         {
             var myFile = new MyFile();
             var persistConverter = new Base64PersistConverter();
             return new DatFromXmlFileLoader(
                myFile,
                new TextReader(myFile),
                new XmlSerializationUtils<EnvCryptEncryptedData>(),
                new XmlToDatMapper(persistConverter));
         }
    }
}
