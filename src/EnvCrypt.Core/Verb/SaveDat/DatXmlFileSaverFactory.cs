using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper.Xml.ToXmlPoco;
using EnvCrypt.Core.EncrypedData.UserStringConverter;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Utils.IO.StringWriter;

namespace EnvCrypt.Core.Verb.SaveDat
{
    public static class DatXmlFileSaverFactory
    {
        public static IDatSaver<DatToFileSaverOptions> GetDatSaver()
        {
            Contract.Ensures(Contract.Result<IDatSaver<DatToFileSaverOptions>>() != null);
            //
            var myFile = new MyFile();
            return new DatToXmlFileSaver(
                new DatToXmlMapper(
                    new EncryptedDetailsPersistConverter(new Utf16LittleEndianUserStringConverter())),
                new XmlSerializationUtils<EnvCryptEncryptedData>(),
                new StringToFileWriter(new MyDirectory(), myFile));
        }
    }
}
