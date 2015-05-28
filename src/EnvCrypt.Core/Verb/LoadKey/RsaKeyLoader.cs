using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key.Mapper;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Core.Verb.LoadKey
{
    /// <summary>
    /// Loads the RSA key. Could be a private or public key.
    /// </summary>
    class RsaKeyLoader : IKeyLoader<RsaKey>
    {
        private readonly IMyFile _myFile;
        private readonly ITextReader _xmlReader;
        private readonly IXmlSerializationUtils<EnvCryptKey> _xmlSerializationUtils;
        private readonly IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> _mapper;

        public RsaKeyLoader(IMyFile myFile,
            ITextReader xmlReader, 
            IXmlSerializationUtils<EnvCryptKey> xmlSerializationUtils,
            IExternalRepresentationToKeyMapper<EnvCryptKey, RsaKey> mapper)
        {
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            Contract.Requires<ArgumentNullException>(xmlReader != null, "xmlReader");
            Contract.Requires<ArgumentNullException>(xmlSerializationUtils != null, "xmlSerializationUtils");
            Contract.Requires<ArgumentNullException>(mapper != null, "mapper");
            //
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

            // Mapper will throw exception if any part is not there
            var poco = _mapper.Map(xmlPoco);
            return poco;
        }
    }
}