using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.SaveDat
{
    class DatToXmlFileSaver : IDatSaver<EnvCryptEncryptedData>
    {
        private readonly IDatToExternalRepresentationMapper<EnvCryptEncryptedData> _pocoToXmlMapper;
        private readonly IXmlSerializationUtils<EnvCryptEncryptedData> _serializationUtils;
        private readonly IStringToFileWriter _fileWriter;

        public DatToXmlFileSaver(IDatToExternalRepresentationMapper<EnvCryptEncryptedData> pocoToXmlMapper, IXmlSerializationUtils<EnvCryptEncryptedData> serializationUtils, IStringToFileWriter fileWriter)
        {
            Contract.Requires<ArgumentNullException>(pocoToXmlMapper != null, "pocoToXmlMapper");
            Contract.Requires<ArgumentNullException>(serializationUtils != null, "serializationUtils");
            Contract.Requires<ArgumentNullException>(fileWriter != null, "fileWriter");
            //
            _pocoToXmlMapper = pocoToXmlMapper;
            _serializationUtils = serializationUtils;
            _fileWriter = fileWriter;
        }


        public void Save(EnvCryptDat data, string toFile)
        {
            var xmlPoco = _pocoToXmlMapper.Map(data);
            var xmlStr = _serializationUtils.Serialize(xmlPoco);
            _fileWriter.Write(toFile, xmlStr, true, _serializationUtils.GetUsedEncoding());
        }
    }
}
