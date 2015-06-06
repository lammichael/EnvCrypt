using System.IO;
using EnvCrypt.Console.AddEntry;
using EnvCrypt.Console.DecryptEntry;
using EnvCrypt.Console.GenerateKey;
using EnvCrypt.Console.UnitTest.Helper;
using EnvCrypt.Core.EncryptionAlgo;
using FluentAssertions;
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
            using (var tempFolder = new TempDir())
            {
                var generateRsaKeyArgs = OptionsToStringArgsHelper.GetArgs(new GenerateKeyVerbOptions()
                {
                    AlgorithmToUse = EnvCryptAlgoEnum.Rsa.ToString(),
                    KeyName = "my new key",
                    OutputDirectory = tempFolder.TempDirectory,
                    OutputKeyToConsole = false,
                    Verbose = false
                });

                // Act
                Program.Main(generateRsaKeyArgs);

                // Assert
                var privateKeyFilePath = Path.Combine(tempFolder.TempDirectory, "my new key.private.eckey");
                var publicKeyFilePath = Path.Combine(tempFolder.TempDirectory, "my new key.public.eckey");
                File.Exists(privateKeyFilePath).Should().BeTrue();
                File.Exists(publicKeyFilePath).Should().BeTrue();



                // Add RSA entry

                // Arrange
                var addRsaEntryArgObj = new AddEntryVerbOptions()
                {
                    AlgorithmToUse = EnvCryptAlgoEnum.Rsa.ToString(),
                    Category = "PROD",
                    NewEntryName = "some password",
                    DatFile = Path.Combine(tempFolder.TempDirectory, "mydat.xml"),
                    KeyFile = publicKeyFilePath,
                    StringToEncrypt = "passw0rd"
                };
                var addRsaEntryArgs = OptionsToStringArgsHelper.GetArgs(addRsaEntryArgObj);

                // Act
                Program.Main(addRsaEntryArgs);

                // Assert
                var datFileXml = File.ReadAllText(addRsaEntryArgObj.DatFile);
                datFileXml.Should().Contain(@"<Entry Name=""some password"">
      <Decryption KeyName=""my new key""");



                /*
                 * Decrypt entry
                 */
                var decryptRsaEntryArgObj = new DecryptEntryVerbOptions()
                {
                    AlgorithmToUse = EnvCryptAlgoEnum.Rsa.ToString(),
                    DatFile = Path.Combine(tempFolder.TempDirectory, "mydat.xml"),
                    KeyFiles = privateKeyFilePath,
                    Categories = "PROD",
                    Entries = "some password"
                };
                var decryptRsaEntryArgs = OptionsToStringArgsHelper.GetArgs(decryptRsaEntryArgObj);

                var consoleOutput = string.Empty;
                var originalConsoleOut = System.Console.Out; // preserve the original stream
                using (var writer = new StringWriter())
                {
                    System.Console.SetOut(writer);
                    Program.Main(decryptRsaEntryArgs);
                    writer.Flush(); // when you're done, make sure everything is written out

                    consoleOutput = writer.GetStringBuilder().ToString();
                }
                System.Console.SetOut(originalConsoleOut); // restore Console.Out

                // Assert
                consoleOutput.Should().Contain("passw0rd");
            }
        }
    }
}