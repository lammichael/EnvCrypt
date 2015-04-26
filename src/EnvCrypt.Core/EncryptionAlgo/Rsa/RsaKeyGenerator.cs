using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
{
    public class RsaKeyGenerator : IKeyGenerator<RsaKey>
    {
        public RsaKey GetNewKey()
        {
            Contract.Ensures(Contract.Result<RsaKey>() != null,
                "no key returned");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.Exponent != null,
                "public key does not contain exponent");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.Modulus != null,
                "public key does not contain exponent");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.D == null,
                "public key exposes D");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.DP == null,
                "public key exposes DP");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.DQ == null,
                "public key exposes DQ");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.InverseQ == null,
                "public key exposes InverseQ");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.P == null,
                "public key exposes P");
            Contract.Ensures(Contract.Result<RsaKey>().PublicKey.Q == null,
                "public key exposes Q");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.Exponent != null,
                "public key does not contain exponent");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.Modulus != null,
                "public key does not contain exponent");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.D != null,
                "public key exposes D");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.DP != null,
                "public key exposes DP");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.DQ != null,
                "public key exposes DQ");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.InverseQ != null,
                "public key exposes InverseQ");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.P != null,
                "public key exposes P");
            Contract.Ensures(Contract.Result<RsaKey>().PrivateKey.Q != null,
                "public key exposes Q");
            //
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
