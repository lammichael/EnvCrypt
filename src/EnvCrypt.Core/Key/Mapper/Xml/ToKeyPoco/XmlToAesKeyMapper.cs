using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key.Aes;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.Key.Mapper.Xml.ToKeyPoco
{
    class XmlToAesKeyMapper : IExternalRepresentationToKeyMapper<XmlPoco.EnvCryptKey, AesKey>
    {
        public const EnvCryptAlgoEnum AlgorithmType = EnvCryptAlgoEnum.Rsa;

        private readonly IStringPersistConverter _strConverter;

        public XmlToAesKeyMapper(IStringPersistConverter strConverter)
        {
            _strConverter = strConverter;
        }


        public AesKey Map(XmlPoco.EnvCryptKey fromExternalRepresentationPoco)
        {
            //      Map works ensures that at least the public key will be returned
            Contract.Ensures(Contract.Result<AesKey>().Iv != null);
            Contract.Ensures(Contract.Result<AesKey>().Iv.Length > 0);
            Contract.Ensures(Contract.Result<AesKey>().Key != null);
            Contract.Ensures(Contract.Result<AesKey>().Key.Length > 0);
            //
            if (fromExternalRepresentationPoco.Aes == null)
            {
                throw new EnvCryptException("AES element must be present in key");
            }
            // There will be only 1 key per XML POCO
            if (string.IsNullOrWhiteSpace(fromExternalRepresentationPoco.Name))
            {
                throw new EnvCryptException("ECKey name in XML must not be empty");
            }
            if (fromExternalRepresentationPoco.Aes.Length != 1)
            {
                throw new EnvCryptException("ECKey XML must contain only one AES (1) key");
            }

            if (string.IsNullOrWhiteSpace(fromExternalRepresentationPoco.Aes[0].Iv))
            {
                throw new EnvCryptException("ECKey XML must contain at the IV for AES");
            }
            if (string.IsNullOrWhiteSpace(fromExternalRepresentationPoco.Aes[0].Key))
            {
                throw new EnvCryptException("ECKey XML must contain the Key for AES");
            }

            var xmlData = fromExternalRepresentationPoco.Aes[0];
            var ret = new AesKey()
            {
                Name = fromExternalRepresentationPoco.Name
            };
            ret.Iv = _strConverter.Decode(xmlData.Iv);
            ret.Key = _strConverter.Decode(xmlData.Key);
            return ret;
        }
    }
}
