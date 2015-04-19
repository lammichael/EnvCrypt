using System.Security.Cryptography;

namespace EnvCrypt.Core.Key
{
    public class RsaKey : KeyBase
    {
        public RSAParameters PrivateKey { get; set; }
        public RSAParameters PublicKey { get; set; }
    }
}
