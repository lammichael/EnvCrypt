using EnvCrypt.Core.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace EnvCrypt.Core.UnitTest.Utils
{
    [TestFixture]
    public class KeyDetailsPersistConverterTest
    {
        [Test]
        public void Given_RandomBinaryData_When_EncodeAndDecode_Then_BinaryDataMustBeUnchanged()
        {
            // Arrange
            var randomData = RandomByteArrayUtils.CreateRandomByteArray(9999);

            // Act
            var converter = new KeyDetailsPersistConverter();
            var result = converter.Decode(converter.Encode(randomData));

            // Assert
            result.Should().BeEquivalentTo(randomData);
        }
    }
}