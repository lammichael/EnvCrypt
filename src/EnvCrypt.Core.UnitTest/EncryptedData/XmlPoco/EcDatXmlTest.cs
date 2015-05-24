using EnvCrypt.Core.EncrypedData.XmlPoco;
using EnvCrypt.Core.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.EncryptedData.XmlPoco
{
    [TestFixture]
    public class EcDatXmlTest
    {
        [Test]
        public void Given_ValidDatXML_When_Parsed_Then_CountsCorrect()
        {
            // Arrange
            var serializationUtil = new XmlSerializationUtils<EnvCryptEncryptedData>();

            // Act
            var res = serializationUtil.Deserialize(@"<EnvCryptEncryptedData>
	<Category Name=""Prod ReadOnly"">
		<!-- EnvCrypt always stores Strings -->
		
		<Entry Name=""ComLib"">
			<!-- Hash of only part of the key because KeyName is not unique -->
			<Decryption KeyName=""first key"" Hash=""123456"" Algo=""RSA"" />

<!-- 2nd decryption should be ignored. This should not be allowed. -->
			<Decryption KeyName=""asdasdsad RO Key"" Hash=""0"" Algo=""RSA"" />
			
			<EncryptedValue>...</EncryptedValue>
			<!-- Value to be encrypted may spill over max that can be encrypted in one go (RSA) -->
			<EncryptedValue>...</EncryptedValue>
		</Entry>
		<Entry Name=""ComLibConnectionString"">
			<!-- No encryption -->
			<EncryptedValue>...</EncryptedValue>
		</Entry>
	</Category>
	<Category Name=""Prod ReadWrite"">
		<Entry Name=""ComLib"">
			<!-- Hash of only part of the key because KeyName is not unique -->
			<Decryption KeyName=""Prod RW Key"" Hash=""1"" Algo=""AES""/>
			<EncryptedValue>...</EncryptedValue>
		</Entry>
		<Entry Name=""ComLibConnectionString"">
			<!-- No encryption -->
			<EncryptedValue>...</EncryptedValue>
		</Entry>
	</Category>
</EnvCryptEncryptedData>");

            // Assert
            res.Items.Should().HaveCount(2);    // 2 categories
            res.Items[0].Entry.Should().HaveCount(2);    // 2 entries in 1st category
            res.Items[0].Entry[0].Decryption.KeyName.Should().Be("first key");
            res.Items[0].Entry[0].Decryption.Hash.Should().Be(123456);
        }
    }
}