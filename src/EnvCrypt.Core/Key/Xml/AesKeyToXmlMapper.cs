using System;
using System.Diagnostics.Contracts;
using System.Linq;
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.Key.Xml
{
    class AesKeyToXmlMapper : IKeyToExternalRepresentationMapper<AesKey, Key.Xml.EnvCryptKey>
    {
        public const EnvCryptAlgorithmEnum AlgorithmType = EnvCryptAlgorithmEnum.Aes;

        private readonly IStringPersistConverter _strConverter;

        public AesKeyToXmlMapper(IStringPersistConverter strConverter)
        {
            Contract.Requires<ArgumentNullException>(strConverter != null, "strConverter");
            //
            _strConverter = strConverter;
        }


        public void Map(AesKey fromPoco, Key.Xml.EnvCryptKey toExternalRepresentationPoco)
        {
            Contract.Ensures(toExternalRepresentationPoco.Aes != null);
            //      There will be only 1 key per XML POCO
            Contract.Ensures(toExternalRepresentationPoco.Aes.Length == 1);
            Contract.Ensures(!string.IsNullOrWhiteSpace(toExternalRepresentationPoco.Aes[0].Iv));
            Contract.Ensures(!string.IsNullOrWhiteSpace(toExternalRepresentationPoco.Aes[0].Key));
            //
            if (fromPoco.Iv == null || !fromPoco.Iv.Any())
            {
                throw new EnvCryptException("AES IV must be in the key");
            }
            if (fromPoco.Key == null || !fromPoco.Key.Any())
            {
                throw new EnvCryptException("AES Key must be in the key");
            }

            toExternalRepresentationPoco.Name = fromPoco.Name;
            toExternalRepresentationPoco.Encryption = AlgorithmType.ToString();
            var xmlAesRoot = new EnvCryptKeyAes();
            xmlAesRoot.Iv = _strConverter.Encode(fromPoco.Iv);
            xmlAesRoot.Key = _strConverter.Encode(fromPoco.Key);
            toExternalRepresentationPoco.Aes = new[]
            {
                xmlAesRoot
            };
        }
    }
}
