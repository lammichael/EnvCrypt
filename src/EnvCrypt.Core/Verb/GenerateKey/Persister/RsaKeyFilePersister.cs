using System;
using System.Diagnostics.Contracts;
using System.IO;
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using EnvCrypt.Core.Key;
using EnvCrypt.Core.Utils;
using EnvCryptKey = EnvCrypt.Core.Key.Xml.EnvCryptKey;

namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    class RsaKeyFilePersister : IAsymetricKeyFilePersister<RsaKey, Key.Xml.EnvCryptKey>
    {
        private readonly IKeyToExternalRepresentationMapper<RsaKey, Key.Xml.EnvCryptKey> _pocoMapper;
        private readonly IXmlSerializationUtils<Key.Xml.EnvCryptKey> _serializationUtils;

        public RsaKeyFilePersister(IKeyToExternalRepresentationMapper<RsaKey, EnvCryptKey> pocoMapper, IXmlSerializationUtils<EnvCryptKey> serializationUtils)
        {
            Contract.Requires<ArgumentNullException>(pocoMapper != null, "pocoMapper");
            Contract.Requires<ArgumentNullException>(serializationUtils != null, "serializationUtils");
            //
            _pocoMapper = pocoMapper;
            _serializationUtils = serializationUtils;
        }


        public void WriteToFile(RsaKey thisKey, AsymmetricKeyFilePersisterOptions withOptions)
        {
            if (thisKey.GetKeyType() != AsymmetricKeyType.Private)
            {
                throw new EnvCryptException("key to persist must have all data for a private key");
            }

            Directory.CreateDirectory(Path.GetDirectoryName(withOptions.NewPrivateKeyFileFullPath));
            {
                // Write private key
                var privateKeyXmlPoco = new Key.Xml.EnvCryptKey();
                _pocoMapper.Map(thisKey, privateKeyXmlPoco);
                var toWrite = _serializationUtils.Serialize(privateKeyXmlPoco);

                File.WriteAllText(withOptions.NewPrivateKeyFileFullPath, toWrite);
            }

            // Write public key
            var publicKeyXmlPoco = new Key.Xml.EnvCryptKey();
            _pocoMapper.Map(thisKey, publicKeyXmlPoco);
            _serializationUtils.Serialize(publicKeyXmlPoco);

            //TODO: Refactor code to write to file
        }
    }
}