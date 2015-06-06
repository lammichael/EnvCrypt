using System.IO;
using System.Text;
using EnvCrypt.Core.Utils.IO;
using EnvCrypt.Core.Utils.IO.StringWriter;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace EnvCrypt.Core.UnitTest.Utils.IO
{
    [TestFixture]
    public class StringToFileWriterTest
    {
        [Test]
        public void Given_FileAlreadyExists_When_DoNotOverwrite_Then_Exception()
        {
            // Arrange
            var myDirMock = new Mock<IMyDirectory>(MockBehavior.Strict);
            var myFileMock = new Mock<IMyFile>(MockBehavior.Strict);

            const string fileToWrite = @"X:\some\made\up\dir\EnvCrypt.key";
            myFileMock.Setup(f => f.Exists(fileToWrite)).Returns(true);

            // Act
            var writer = new StringToFileWriter(myDirMock.Object, myFileMock.Object);
            Action act = () => writer.Write(new StringToFileWriterOptions()
            {
                Contents = "nothing signficant in this string for this test",
                Path = fileToWrite,
                Encoding = Encoding.Unicode,
                OverwriteIfFileExists = false
            });

            // Assert
            act.ShouldThrowExactly<EnvCryptException>();
        }


        [Test]
        public void Given_FileAlreadyExists_When_Overwrite_Then_FileIsWrittenThere()
        {
            // Arrange
            var myDirMock = new Mock<IMyDirectory>();
            var myFileMock = new Mock<IMyFile>();

            const string fileToWrite = @"X:\some\made\up\dir\EnvCrypt.key";
            myFileMock.Setup(f => f.Exists(fileToWrite)).Returns(true);

            const string contents = "nothing signficant in this string for this test";

            // Act
            var writer = new StringToFileWriter(myDirMock.Object, myFileMock.Object);
            writer.Write(new StringToFileWriterOptions()
            {
                Contents = contents,
                Path = fileToWrite,
                Encoding = Encoding.Unicode,
                OverwriteIfFileExists = true
            });

            // Assert
            myDirMock.Verify(d => d.CreateDirectory(Path.GetDirectoryName(fileToWrite)),
                Times.Once);
        }
    }
}