using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo;

namespace EnvCrypt.Core.Key.Rsa
{
    public class RsaKeyGenerator : IKeyGenerator<RsaKey, RsaKeyGenerationOptions>
    {
        /// <summary>
        /// Gets a new key, given the options requested, including the private key.
        /// The public key can be derived from the public key using helper methods.
        /// </summary>
        /// <param name="options">generation options</param>
        /// <returns>a new RSA key</returns>
        public RsaKey GetNewKey(RsaKeyGenerationOptions options)
        {
            Contract.Ensures(Contract.Result<RsaKey>().Key.Exponent != null,
                "private key does not contain exponent");
            Contract.Ensures(Contract.Result<RsaKey>().Key.Modulus != null,
                "private key does not contain exponent");
            Contract.Ensures(Contract.Result<RsaKey>().Key.D != null,
                "private key does not contain D");
            Contract.Ensures(Contract.Result<RsaKey>().Key.DP != null,
                "private key does not contain DP");
            Contract.Ensures(Contract.Result<RsaKey>().Key.DQ != null,
                "private key does not contain DQ");
            Contract.Ensures(Contract.Result<RsaKey>().Key.InverseQ != null,
                "private key does not contain InverseQ");
            Contract.Ensures(Contract.Result<RsaKey>().Key.P != null,
                "private key does not contain P");
            Contract.Ensures(Contract.Result<RsaKey>().Key.Q != null,
                "private key does not contain Q");
            Contract.Ensures(Contract.Result<RsaKey>().UseOaepPadding == options.UseOaepPadding,
                "OAEP Padding option not replicated correctly");
            //
            RSAParameters privateKey;
            using (var myRsa = new RSACryptoServiceProvider(options.KeySize))
            {
                privateKey = myRsa.ExportParameters(true);
            }
            return new RsaKey(privateKey, options.UseOaepPadding)
            {
                Name = options.NewKeyName
            };
        }
    }
}
