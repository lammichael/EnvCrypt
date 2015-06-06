using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Utils.IO.StringWriter;

namespace EnvCrypt.Core.Verb.SaveDat
{
    public static class DatXmlFileSaverFactory
    {
        public static IDatSaver<DatToFileSaverDetails> GetDatSaver()
        {
            Contract.Ensures(Contract.Result<IDatSaver<DatToFileSaverDetails>>() != null);
            //
            var myFile = new MyFile();
            return new DatToXmlFileSaver(
                new DatToXmlMapper(new Base64PersistConverter()),
                new XmlSerializationUtils<EnvCryptEncryptedData>(),
                new StringToFileWriter(new MyDirectory(), myFile));
        }
    }
}
