using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncrypedData.Mapper;
using EnvCrypt.Core.EncrypedData.Poco;
using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO.StringWriter;

namespace EnvCrypt.Core.Verb.SaveDat
{
    class DatToXmlFileSaver : IDatSaver<DatToFileSaverDetails>
    {
        private readonly IDatToExternalRepresentationMapper<EnvCryptEncryptedData> _pocoToXmlMapper;
        private readonly IXmlSerializationUtils<EnvCryptEncryptedData> _serializationUtils;
        private readonly IStringWriter<StringToFileWriterOptions> _fileWriter;

        public DatToXmlFileSaver(IDatToExternalRepresentationMapper<EnvCryptEncryptedData> pocoToXmlMapper,
            IXmlSerializationUtils<EnvCryptEncryptedData> serializationUtils,
            IStringWriter<StringToFileWriterOptions> fileWriter)
        {
            Contract.Requires<ArgumentNullException>(pocoToXmlMapper != null, "pocoToXmlMapper");
            Contract.Requires<ArgumentNullException>(serializationUtils != null, "serializationUtils");
            Contract.Requires<ArgumentNullException>(fileWriter != null, "fileWriter");
            //
            _pocoToXmlMapper = pocoToXmlMapper;
            _serializationUtils = serializationUtils;
            _fileWriter = fileWriter;
        }


        public void Save(EnvCryptDat data, DatToFileSaverDetails fileSaverDetails)
        {
            if (fileSaverDetails == null ||
                string.IsNullOrWhiteSpace(fileSaverDetails.DestinationFilePath))
            {
                throw new ArgumentException("destination file path cannot be empty");
            }

            var xmlPoco = _pocoToXmlMapper.Map(data);
            var xmlStr = _serializationUtils.Serialize(xmlPoco);

            var fileWriterOptions = new StringToFileWriterOptions()
            {
                Contents = xmlStr,
                Encoding = _serializationUtils.GetUsedEncoding(),
                Path = fileSaverDetails.DestinationFilePath,
                OverwriteIfFileExists = true
            };
            _fileWriter.Write(fileWriterOptions);
        }
    }
}
