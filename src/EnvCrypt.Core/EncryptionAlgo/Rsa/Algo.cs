using System.Security.Cryptography;
using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo.Rsa
{
    public class RsaAlgo : IEncryptionAlgo<RsaKey>
    {
        public byte[] Encrypt(byte[] stringToEncrypt, RsaKey usingKey)
        {
            return RsaEncrypt(stringToEncrypt, usingKey.PrivateKey, true);
        }


        public byte[] Decrypt(byte[] toDecrypt, RsaKey usingKey)
        {
            return RsaDecrypt(toDecrypt, usingKey.PublicKey, true);
        }


        private static byte[] RsaEncrypt(byte[] dataToEncrypt, RSAParameters rsaKeyInfo, bool doOaepPadding)
        {
            //TODO: Catch and display a CryptographicException

            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider. 
            using (var myRsa = new RSACryptoServiceProvider())
            {

                //Import the RSA Key information. This only needs 
                //toinclude the public key information.
                myRsa.ImportParameters(rsaKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.   
                //OAEP padding is only available on Microsoft Windows XP or 
                //later.  
                encryptedData = myRsa.Encrypt(dataToEncrypt, doOaepPadding);
            }
            return encryptedData;
        }


        private static byte[] RsaDecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] decryptedData;
            //Create a new instance of RSACryptoServiceProvider. 
            using (var myRsa = new RSACryptoServiceProvider())
            {
                //Import the RSA Key information. This needs 
                //to include the private key information.
                myRsa.ImportParameters(RSAKeyInfo);

                //Decrypt the passed byte array and specify OAEP padding.   
                //OAEP padding is only available on Microsoft Windows XP or 
                //later.  
                decryptedData = myRsa.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            return decryptedData;
        }
    }
}