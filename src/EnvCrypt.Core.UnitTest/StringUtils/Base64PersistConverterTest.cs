using EnvCrypt.Core.StringUtils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.StringUtils
{
    [TestFixture]
    public class Base64PersistConverterTest
    {
        [Test]
        public void Given_RandomBinaryData_When_EncodeAndDecode_Then_BinaryDataMustBeUnchanged()
        {
            // Arrange
            var randomData = RandomByteArrayUtils.CreateRandomByteArray(9999);

            // Act
            var converter = new Base64PersistConverter();
            var result = converter.Decode(converter.Encode(randomData));

            // Assert
            result.Should().BeEquivalentTo(randomData);
        }
    }
}