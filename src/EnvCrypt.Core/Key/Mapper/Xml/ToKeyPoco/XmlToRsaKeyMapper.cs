using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.Key.Rsa;
using EnvCrypt.Core.Key.XmlPoco;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.Key.Mapper.Xml.ToKeyPoco
{
    class XmlToRsaKeyMapper : IExternalRepresentationToKeyMapper<XmlPoco.EnvCryptKey, RsaKey>
    {
        public const EnvCryptAlgoEnum AlgorithmType = EnvCryptAlgoEnum.Rsa;

        private readonly IStringPersistConverter _strConverter;

        public XmlToRsaKeyMapper(IStringPersistConverter strConverter)
        {
            _strConverter = strConverter;
        }


        public RsaKey Map(XmlPoco.EnvCryptKey fromExternalRepresentationPoco)
        {
            //      Map works ensures that at least the public key will be returned
            Contract.Ensures(Contract.Result<RsaKey>().Key.Exponent != null);
            Contract.Ensures(Contract.Result<RsaKey>().Key.Exponent.Length > 0);
            Contract.Ensures(Contract.Result<RsaKey>().Key.Modulus != null);
            Contract.Ensures(Contract.Result<RsaKey>().Key.Modulus.Length > 0);
            //
            if (fromExternalRepresentationPoco.Rsa == null)
            {
                throw new EnvCryptException("RSA element must be present in key");
            }
            // There will be only 1 key per XML POCO
            if (string.IsNullOrWhiteSpace(fromExternalRepresentationPoco.Name))
            {
                throw new EnvCryptException("ECKey name in XML must not be empty");
            }
            if (fromExternalRepresentationPoco.Rsa.Length != 1)
            {
                throw new EnvCryptException("ECKey XML must contain only one RSA (1) key");
            }

            if (string.IsNullOrWhiteSpace(fromExternalRepresentationPoco.Rsa[0].Exponent))
            {
                throw new EnvCryptException("ECKey XML must contain at least the Exponent for RSA");
            }
            if (string.IsNullOrWhiteSpace(fromExternalRepresentationPoco.Rsa[0].Modulus))
            {
                throw new EnvCryptException("ECKey XML must contain at least the Modulus for RSA");
            }

            var xmlData = fromExternalRepresentationPoco.Rsa[0];
            var rsaParameters = GetRsaParametersFromXml(xmlData);
            var ret = new RsaKey(rsaParameters, xmlData.OaepPadding)
            {
                Name = fromExternalRepresentationPoco.Name
            };
            return ret;
        }


        private RSAParameters GetRsaParametersFromXml(EnvCryptKeyRsa xmlData)
        {
            var rsaParameters = new RSAParameters();
            // Exponent & Modulus not null check performed in pre-conidition
            rsaParameters.Exponent = _strConverter.Decode(xmlData.Exponent);
            rsaParameters.Modulus = _strConverter.Decode(xmlData.Modulus);
            rsaParameters.D = xmlData.D == null ? null : _strConverter.Decode(xmlData.D);
            rsaParameters.DP = xmlData.Dp == null ? null : _strConverter.Decode(xmlData.Dp);
            rsaParameters.DQ = xmlData.Dq == null ? null : _strConverter.Decode(xmlData.Dq);
            rsaParameters.InverseQ = xmlData.InverseQ == null ? null : _strConverter.Decode(xmlData.InverseQ);
            rsaParameters.P = xmlData.P == null ? null : _strConverter.Decode(xmlData.P);
            rsaParameters.Q = xmlData.Q == null ? null : _strConverter.Decode(xmlData.Q);
            return rsaParameters;
        }
    }
}
