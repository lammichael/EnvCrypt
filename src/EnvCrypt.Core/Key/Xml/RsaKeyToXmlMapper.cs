using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvCrypt.Core.EncryptionAlgo.Aes.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Utils;
using EnvCrypt.Core.Utils;

namespace EnvCrypt.Core.Key.Xml
{
    class RsaKeyToXmlMapper : IKeyToExternalRepresentationMapper<RsaKey, Key.Xml.EnvCryptKey>
    {
        public const EnvCryptAlgorithmEnum AlgorithmType = EnvCryptAlgorithmEnum.Rsa;

        private readonly IStringPersistConverter _strConverter;

        public RsaKeyToXmlMapper(IStringPersistConverter strConverter)
        {
            Contract.Requires<ArgumentNullException>(strConverter != null, "strConverter");
            //
            _strConverter = strConverter;
        }


        public void Map(RsaKey fromPoco, Key.Xml.EnvCryptKey toExternalRepresentationPoco)
        {
            Contract.Requires<EnvCryptException>(fromPoco.Key.Exponent != null,
                "RSA Exponent must at least be in the key");
            Contract.Requires<EnvCryptException>(fromPoco.Key.Modulus != null,
                "RSA Modulus must at least be in the key");
            Contract.Ensures(toExternalRepresentationPoco.Rsa != null);
            //      There will be only 1 key per XML POCO
            Contract.Ensures(toExternalRepresentationPoco.Rsa.Length == 1);
            Contract.Ensures(!string.IsNullOrWhiteSpace(toExternalRepresentationPoco.Rsa[0].Exponent));
            Contract.Ensures(!string.IsNullOrWhiteSpace(toExternalRepresentationPoco.Rsa[0].Modulus));
            //
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
            xmlRsaRoot.Exponent = fromPoco.Key.Exponent == null ? null : 
                _strConverter.Encode(fromPoco.Key.Exponent);
            xmlRsaRoot.InverseQ = fromPoco.Key.InverseQ == null ? null : 
                _strConverter.Encode(fromPoco.Key.InverseQ);
            xmlRsaRoot.Modulus = fromPoco.Key.Modulus == null ? null : 
                _strConverter.Encode(fromPoco.Key.Modulus);
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
