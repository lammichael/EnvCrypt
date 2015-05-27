using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey
{
    class LoadRsaKeyWorkflow : ILoadKeyWorkflow<RsaKey>
    {
        private readonly IMyFile _myFile;
        private readonly ITextReader _xmlReader;
        private readonly IXmlSerializationUtils<EnvCryptKey> _xmlSerializationUtils;
        private readonly IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> _mapper;

        public LoadRsaKeyWorkflow(IMyFile myFile,
            ITextReader xmlReader, 
            IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils,
            IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> mapper)
        {
            _xmlReader = xmlReader;
            _mapper = mapper;
            _myFile = myFile;
            _xmlSerializationUtils = xmlSerializationUtils;
        }


        public RsaKey Run(string ecKeyFilePath)
        {
            if (!_myFile.Exists(ecKeyFilePath))
            {
                throw new EnvCryptException("key file does not exist: {0}", ecKeyFilePath);
            }

            var xmlFromFile = _xmlReader.ReadAllText(ecKeyFilePath);
            if (string.IsNullOrWhiteSpace(xmlFromFile))
            {
                throw new EnvCryptException("key file is empty: {0}", ecKeyFilePath);
            }
            var xmlPoco = _xmlSerializationUtils.Deserialize(xmlFromFile);
            if (xmlPoco == null)
            {
                throw new EnvCryptException("deserialisation of file failed: {0}", ecKeyFilePath);
            }

            //TODO: Validation

            var poco = _mapper.Map(xmlPoco);
            return poco;
        }
    }
}