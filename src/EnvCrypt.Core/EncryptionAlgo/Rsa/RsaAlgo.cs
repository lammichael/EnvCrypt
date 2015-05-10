using System;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using EnvCrypt.Core.EncryptionAlgo.Rsa.Key;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
{
    public class RsaAlgo : IEncryptionAlgo<RsaKey>
    {
        public byte[] Encrypt(byte[] binaryData, RsaKey usingKey)
        {
            return RsaEncrypt(binaryData, usingKey.Key, true);
        }


        public byte[] Decrypt(byte[] binaryData, RsaKey usingKey)
        {
            return RsaDecrypt(binaryData, usingKey.Key, true);
        }


        private static byte[] RsaEncrypt(byte[] dataToEncrypt, RSAParameters rsaPublicKey, bool doOaepPadding)
        {
            Contract.Requires<EnvCryptAlgoException>(rsaPublicKey.Exponent != null,
                "Exponent not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPublicKey.Modulus != null,
                "Modulus not in private key");

            //

            //TODO: Catch and display a CryptographicException

            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider. 
            using (var myRsa = new RSACryptoServiceProvider())
            {

                //Import the RSA Key information. This only needs 
                //toinclude the public key information.
                myRsa.ImportParameters(rsaPublicKey);

                //Encrypt the passed byte array and specify OAEP padding.   
                //OAEP padding is only available on Microsoft Windows XP or 
                //later.  
                encryptedData = myRsa.Encrypt(dataToEncrypt, doOaepPadding);
            }
            return encryptedData;
        }


        private static byte[] RsaDecrypt(byte[] dataToDecrypt, RSAParameters rsaPrivateKey, bool doOaepPadding)
        {
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.D != null,
                "D not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.DP != null,
                "DP not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.DQ != null,
                "DQ not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.Exponent != null,
                "Exponent not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.InverseQ != null,
                "InverseQ not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.Modulus != null,
                "Modulus not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.P != null,
                "P not in private key");
            Contract.Requires<EnvCryptAlgoException>(rsaPrivateKey.Q != null,
                "Q not in private key");
            //
            byte[] decryptedData;
            //Create a new instance of RSACryptoServiceProvider. 
            using (var myRsa = new RSACryptoServiceProvider())
            {
                //Import the RSA Key information. This needs 
                //to include the private key information.
                myRsa.ImportParameters(rsaPrivateKey);

                //Decrypt the passed byte array and specify OAEP padding.   
                //OAEP padding is only available on Microsoft Windows XP or 
                //later.  
                decryptedData = myRsa.Decrypt(dataToDecrypt, doOaepPadding);
            }
            return decryptedData;
        }
    }
}