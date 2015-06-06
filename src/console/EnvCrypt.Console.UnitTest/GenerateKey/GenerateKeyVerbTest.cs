using System.IO;
using EnvCrypt.Console.GenerateKey;
using EnvCrypt.Console.UnitTest.Helper;
using EnvCrypt.Core.EncryptionAlgo;
using NUnit.Framework;

namespace EnvCrypt.Console.UnitTest.GenerateKey
{
    [TestFixture]
    public class GenerateKeyVerbTest
    {
        [Test]
        public void Given__When__Then_()
        {
            // Arrange
            var tempFolder = Path.GetTempFileName();
            File.Delete(tempFolder);
            Directory.CreateDirectory(tempFolder);
            var strArgs = OptionsToStringArgsHelper.GetArgs(new GenerateKeyVerbOptions()
            {
                AlgorithmToUse = EnvCryptAlgoEnum.Rsa.ToString(),
                KeyName = "my new key",
                OutputDirectory = tempFolder,
                OutputKeyToConsole = false,
                Verbose = false
            });

            // Act


            // Assert

        }
    }
}