using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EnvCrypt.Console.AddEntry;
using EnvCrypt.Console.DecryptEntry;
using EnvCrypt.Console.GenerateKey;
using EnvCrypt.Console.UnitTest.Helper;
using EnvCrypt.Core.EncryptionAlgo;
using EnvCrypt.Core.UnitTest;
using EnvCrypt.Core.Verb.DecryptEntry;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Console.UnitTest
{
    [TestFixture]
    public class EndToEndTest
    {
        private string _tempDir;

        private const string RsaKeyName = "My test RSA key";
        private string _rsaPrivateKeyFile;
        private string _rsaPublicKeyFile;

        private const string AesKeyName = "My test AES key";
        private string _aesKeyFile;

        private string _datFile;

        private readonly IList<Entry> _rsaEntries = new []
        {
            new Entry()
            {
                CategoryEntryPair = new CategoryEntryPair("EMEA", "DB password (RSA)"),
                DecryptedValue = "Pa55w0rd"
            },
            new Entry()
            {
                CategoryEntryPair = new CategoryEntryPair("APAC", "DB password (RSA)"),
                DecryptedValue = "g1tRoCk5"
            }
        };
        private readonly IList<Entry> _aesEntries = new[]
        {
            new Entry()
            {
                CategoryEntryPair = new CategoryEntryPair("EMEA", "root password (AES)"),
                DecryptedValue = "L1NuXR0cks"
            },
            new Entry()
            {
                CategoryEntryPair = new CategoryEntryPair("APAC", "root password (AES)"),
                DecryptedValue = "AE5R0ck5"
            }
        };
        private readonly IList<Entry> _plainTextEntries = new[]
        {
            new Entry()
            {
                CategoryEntryPair = new CategoryEntryPair("EMEA", "DB username 1"),
                DecryptedValue = "lammichaelEUR"
            },
            new Entry()
            {
                CategoryEntryPair = new CategoryEntryPair("APAC", "DB username 2"),
                DecryptedValue = "lammichaelAPAC"
            }
        };


        [Test]
        public void Given_RSAAndAESKeys_When_AddEntries_Then_DecryptionPossible()
        {
            // Arrange
            using (var tempFolder = new TempDir())
            {
                _tempDir = tempFolder.TempDirectory;
                _datFile = Path.Combine(_tempDir, "My Test Dat.xml");

                /*
                 * Generate RSA and AES keys
                 */
                GenerateRsaKey(tempFolder);
                GenerateAesKey(tempFolder);

                /*
                 * Add RSA, AES and plaintext entries
                 */
                AddRsaEntries();
                AddAesEntries();
                AddPlainTextEntries();



                /*
                 * Decrypt entry
                 */
                DecryptRsaEntries();
                DecryptAesEntries();
                GetPlainTextEntries();
            }
        }


        private void GenerateRsaKey(TempDir tempFolder)
        {
            // Arrange
            var generateRsaKeyArgs = OptionsToStringArgsHelper.GetArgs(new GenerateKeyVerbOptions()
            {
                AlgorithmToUse = EnvCryptAlgoEnum.Rsa.ToString(),
                KeyName = RsaKeyName,
                OutputDirectory = tempFolder.TempDirectory,
                OutputKeyToConsole = false,
                Verbose = false
            });

            // Act
            Program.Main(generateRsaKeyArgs);

            // Assert
            _rsaPrivateKeyFile = Path.Combine(tempFolder.TempDirectory, RsaKeyName + GenerateKeyWorkflow.PrivateKeyPostfix);
            _rsaPublicKeyFile = Path.Combine(tempFolder.TempDirectory, RsaKeyName + GenerateKeyWorkflow.PublicKeyPostfix);
            File.Exists(_rsaPrivateKeyFile).Should().BeTrue();
            File.Exists(_rsaPublicKeyFile).Should().BeTrue();
            
            var privateKeyXml = File.ReadAllText(_rsaPrivateKeyFile);
            privateKeyXml = privateKeyXml.Replace(Environment.NewLine, string.Empty);
            var publicKeyXml = File.ReadAllText(_rsaPublicKeyFile);
            publicKeyXml = publicKeyXml.Replace(Environment.NewLine, string.Empty);

            //      Public key just has the exponent & modulus
            publicKeyXml.Should().MatchRegex(".+<Exponent>.+</Exponent>");
            publicKeyXml.Should().MatchRegex(".+<Modulus>.+</Modulus>");
            publicKeyXml.Should().NotMatchRegex(".+<D>.+</D>");

            //      Private key has everything the public key has
            privateKeyXml.Should().MatchRegex(".+<Exponent>.+</Exponent>");
            privateKeyXml.Should().MatchRegex(".+<Modulus>.+</Modulus>");
            privateKeyXml.Should().MatchRegex(".+<D>.+</D>");
        }


        private void GenerateAesKey(TempDir tempFolder)
        {
            // Arrange
            var generateKeyArgs = OptionsToStringArgsHelper.GetArgs(new GenerateKeyVerbOptions()
            {
                AlgorithmToUse = EnvCryptAlgoEnum.Aes.ToString(),
                KeyName = AesKeyName,
                OutputDirectory = tempFolder.TempDirectory,
                OutputKeyToConsole = false,
                Verbose = false
            });

            // Act
            Program.Main(generateKeyArgs);

            // Assert
            _aesKeyFile = Path.Combine(tempFolder.TempDirectory, AesKeyName + GenerateKeyWorkflow.CommonPostFix);
            File.Exists(_aesKeyFile).Should().BeTrue();

            var keyXml = File.ReadAllText(_aesKeyFile);
            keyXml = keyXml.Replace(Environment.NewLine, string.Empty);

            keyXml.Should().MatchRegex(".+<Key>.+</Key>");
            keyXml.Should().MatchRegex(".+<Iv>.+</Iv>");

        }


        private void AddRsaEntries()
        {
            foreach (var rsaEntry in _rsaEntries)
            {
                // Arrange
                var addEntryArgObj = new AddEntryVerbOptions()
                {
                    AlgorithmToUse = EnvCryptAlgoEnum.Rsa.ToString(),
                    Category = rsaEntry.CategoryEntryPair.Category,
                    NewEntryName = rsaEntry.CategoryEntryPair.Entry,
                    StringToEncrypt = rsaEntry.DecryptedValue,
                    DatFile = _datFile,
                    KeyFile = _rsaPublicKeyFile,
                };
                var addEntryArgs = OptionsToStringArgsHelper.GetArgs(addEntryArgObj);

                // Act
                Program.Main(addEntryArgs);

                // Assert
                var datFileXml = File.ReadAllText(addEntryArgObj.DatFile);
                datFileXml = datFileXml.Replace(Environment.NewLine, string.Empty);
                datFileXml.Should().MatchRegex(string.Format(@".+<Category Name=""{0}"">.+<Entry Name=""{1}"">.+<Decryption KeyName=""{2}"".+", 
                    Regex.Escape(rsaEntry.CategoryEntryPair.Category),
                    Regex.Escape(rsaEntry.CategoryEntryPair.Entry),
                    Regex.Escape(RsaKeyName)));
            }
        }


        private void AddAesEntries()
        {
            foreach (var aesEntry in _aesEntries)
            {
                // Arrange
                var addEntryArgObj = new AddEntryVerbOptions()
                {
                    AlgorithmToUse = EnvCryptAlgoEnum.Aes.ToString(),
                    Category = aesEntry.CategoryEntryPair.Category,
                    NewEntryName = aesEntry.CategoryEntryPair.Entry,
                    StringToEncrypt = aesEntry.DecryptedValue,
                    DatFile = _datFile,
                    KeyFile = _aesKeyFile,
                };
                var addEntryArgs = OptionsToStringArgsHelper.GetArgs(addEntryArgObj);

                // Act
                Program.Main(addEntryArgs);

                // Assert
                var datFileXml = File.ReadAllText(addEntryArgObj.DatFile);
                datFileXml = datFileXml.Replace(Environment.NewLine, string.Empty);
                datFileXml.Should().MatchRegex(string.Format(@".+<Category Name=""{0}"">.+<Entry Name=""{1}"">.+<Decryption KeyName=""{2}"".+", 
                    Regex.Escape(aesEntry.CategoryEntryPair.Category),
                    Regex.Escape(aesEntry.CategoryEntryPair.Entry),
                    Regex.Escape(AesKeyName)));
            }
        }


        private void AddPlainTextEntries()
        {
            foreach (var plainTextEntries in _plainTextEntries)
            {
                // Arrange
                var addEntryArgObj = new AddEntryVerbOptions()
                {
                    AlgorithmToUse = EnvCryptAlgoEnum.PlainText.ToString(),
                    Category = plainTextEntries.CategoryEntryPair.Category,
                    NewEntryName = plainTextEntries.CategoryEntryPair.Entry,
                    StringToEncrypt = plainTextEntries.DecryptedValue,
                    DatFile = _datFile,
                };
                var addEntryArgs = OptionsToStringArgsHelper.GetArgs(addEntryArgObj);

                // Act
                Program.Main(addEntryArgs);

                // Assert
                var datFileXml = File.ReadAllText(addEntryArgObj.DatFile);
                datFileXml = datFileXml.Replace(Environment.NewLine, string.Empty);
                datFileXml.Should().MatchRegex(string.Format(@".+<Category Name=""{0}"">.+<Entry Name=""{1}"">.*<EncryptedValue>{2}</EncryptedValue>.+", 
                    Regex.Escape(plainTextEntries.CategoryEntryPair.Category),
                    Regex.Escape(plainTextEntries.CategoryEntryPair.Entry),
                    Regex.Escape(plainTextEntries.DecryptedValue)));
            }
        }


        private void DecryptRsaEntries()
        {
            var categoriesInString = string.Join("|", _rsaEntries.Select(e => e.CategoryEntryPair.Category));
            var entriesInString = string.Join("|", _rsaEntries.Select(e => e.CategoryEntryPair.Entry));

            var decryptRsaEntryArgObj = new DecryptEntryVerbOptions()
            {
                AlgorithmToUse = EnvCryptAlgoEnum.Rsa.ToString(),
                DatFile = _datFile,
                KeyFiles = _rsaPrivateKeyFile,
                Categories = categoriesInString,
                Entries = entriesInString
            };
            var decryptRsaEntryArgs = OptionsToStringArgsHelper.GetArgs(decryptRsaEntryArgObj);

            var consoleOutput = new ConsoleScraper(() => Program.Main(decryptRsaEntryArgs)).Run();

            // Assert
            foreach (var entry in _rsaEntries)
            {
                consoleOutput.Should().MatchRegex(string.Format(@".*{0}\s+{1}\s+{2}.*",
                    Regex.Escape(entry.CategoryEntryPair.Category),
                    Regex.Escape(entry.CategoryEntryPair.Entry),
                    Regex.Escape(entry.DecryptedValue)));
            }
        }


        private void DecryptAesEntries()
        {
            var categoriesInString = string.Join("|", _aesEntries.Select(e => e.CategoryEntryPair.Category));
            var entriesInString = string.Join("|", _aesEntries.Select(e => e.CategoryEntryPair.Entry));

            var decryptEntryArgObj = new DecryptEntryVerbOptions()
            {
                AlgorithmToUse = EnvCryptAlgoEnum.Aes.ToString(),
                DatFile = _datFile,
                KeyFiles = _aesKeyFile,
                Categories = categoriesInString,
                Entries = entriesInString
            };
            var decryptEntryArgs = OptionsToStringArgsHelper.GetArgs(decryptEntryArgObj);

            var consoleOutput = new ConsoleScraper(() => Program.Main(decryptEntryArgs)).Run();

            // Assert
            foreach (var entry in _aesEntries)
            {
                consoleOutput.Should().MatchRegex(string.Format(@".*{0}\s+{1}\s+{2}.*",
                    Regex.Escape(entry.CategoryEntryPair.Category),
                    Regex.Escape(entry.CategoryEntryPair.Entry),
                    Regex.Escape(entry.DecryptedValue)));
            }
        }


        private void GetPlainTextEntries()
        {
            var categoriesInString = string.Join("|", _plainTextEntries.Select(e => e.CategoryEntryPair.Category));
            var entriesInString = string.Join("|", _plainTextEntries.Select(e => e.CategoryEntryPair.Entry));

            var decryptEntryArgObj = new DecryptEntryVerbOptions()
            {
                AlgorithmToUse = EnvCryptAlgoEnum.PlainText.ToString(),
                DatFile = _datFile,
                Categories = categoriesInString,
                Entries = entriesInString
            };
            var decryptEntryArgs = OptionsToStringArgsHelper.GetArgs(decryptEntryArgObj);

            var consoleOutput = new ConsoleScraper(() => Program.Main(decryptEntryArgs)).Run();

            // Assert
            foreach (var entry in _plainTextEntries)
            {
                consoleOutput.Should().MatchRegex(string.Format(@".*{0}\s+{1}\s+{2}.*",
                    Regex.Escape(entry.CategoryEntryPair.Category),
                    Regex.Escape(entry.CategoryEntryPair.Entry),
                    Regex.Escape(entry.DecryptedValue)));
            }
        }
    }
}