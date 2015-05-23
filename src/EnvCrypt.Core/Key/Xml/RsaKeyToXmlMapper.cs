using System;
using System.Diagnostics.Contracts;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.Key.Xml
{
    class RsaKeyToXmlMapper : IKeyToExternalRepresentationMapper<RsaKey, Key.Xml.EnvCryptKey>
    {
        public const EnvCryptAlgoEnum AlgorithmType = EnvCryptAlgoEnum.Rsa;

        private readonly IStringPersistConverter _strConverter;

        public RsaKeyToXmlMapper(IStringPersistConverter strConverter)
        {
            Contract.Requires<ArgumentNullException>(strConverter != null, "strConverter");
            //
            _strConverter = strConverter;
        }


        public void Map(RsaKey fromPoco, Key.Xml.EnvCryptKey toExternalRepresentationPoco)
        {
            Contract.Ensures(toExternalRepresentationPoco.Rsa != null);
            //      There will be only 1 key per XML POCO
            Contract.Ensures(toExternalRepresentationPoco.Rsa.Length == 1);
            Contract.Ensures(!string.IsNullOrWhiteSpace(toExternalRepresentationPoco.Rsa[0].Exponent));
            Contract.Ensures(!string.IsNullOrWhiteSpace(toExternalRepresentationPoco.Rsa[0].Modulus));
            //
            if (fromPoco.Key.Exponent == null)
            {
                throw new EnvCryptException("RSA Exponent must at least be in the key");
            }
            if (fromPoco.Key.Modulus == null)
            {
                throw new EnvCryptException("RSA Modulus must at least be in the key");
            }

            toExternalRepresentationPoco.Name = fromPoco.Name;
            toExternalRepresentationPoco.Encryption = AlgorithmType.ToString();
            toExternalRepresentationPoco.Type = fromPoco.GetKeyType().ToString();
            var xmlRsaRoot = new EnvCryptKeyRsa();
            xmlRsaRoot.OaepPadding = fromPoco.UseOaepPadding;
            xmlRsaRoot.D = fromPoco.Key.D == null ? null : 
                _strConverter.Encode(fromPoco.Key.D);
            xmlRsaRoot.Dp = fromPoco.Key.DP == null ? null : 
                _strConverter.Encode(fromPoco.Key.DP);
            xmlRsaRoot.Dq = fromPoco.Key.DQ == null ? null : 
                _strConverter.Encode(fromPoco.Key.DQ);
            xmlRsaRoot.Exponent = _strConverter.Encode(fromPoco.Key.Exponent);
            xmlRsaRoot.InverseQ = fromPoco.Key.InverseQ == null ? null : 
                _strConverter.Encode(fromPoco.Key.InverseQ);
            xmlRsaRoot.Modulus = _strConverter.Encode(fromPoco.Key.Modulus);
            xmlRsaRoot.P = fromPoco.Key.P == null ? null : 
                _strConverter.Encode(fromPoco.Key.P);
            xmlRsaRoot.Q = fromPoco.Key.Q == null ? null : 
                _strConverter.Encode(fromPoco.Key.Q);
            toExternalRepresentationPoco.Rsa = new[]
            {
                xmlRsaRoot
            };
        }
    }
}
