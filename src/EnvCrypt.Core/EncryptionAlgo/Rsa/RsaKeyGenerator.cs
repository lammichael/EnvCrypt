using System.Security.Cryptography;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
{
    public class RsaKeyGenerator : IKeyGenerator<RsaKey>
    {
        public RsaKey GetNewKey()
        {
            var generated = new RsaKey();
            using (var myRsa = new RSACryptoServiceProvider())
            {
                generated.PublicKey = myRsa.ExportParameters(false);
                generated.PrivateKey = myRsa.ExportParameters(true);
            }
            return generated;
        }
    }
}
