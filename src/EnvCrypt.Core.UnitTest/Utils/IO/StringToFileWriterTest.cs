using System.IO;
using System.Text;
using EnvCrypt.Core.Utils.IO;
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
            var textWriterMock = new Mock<ITextWriter>(MockBehavior.Strict);

            const string fileToWrite = @"X:\some\made\up\dir\EnvCrypt.key";
            myFileMock.Setup(f => f.Exists(fileToWrite)).Returns(true);

            // Act
            var writer = new StringToFileWriter(myDirMock.Object, myFileMock.Object, textWriterMock.Object);
            Action act = () => writer.Write(fileToWrite, "nothing signficant in this string for this test",
                false, Encoding.Unicode);

            // Assert
            act.ShouldThrowExactly<EnvCryptException>();
        }


        [Test]
        public void Given_FileAlreadyExists_When_Overwrite_Then_FileIsWrittenThere()
        {
            // Arrange
            var myDirMock = new Mock<IMyDirectory>();
            var myFileMock = new Mock<IMyFile>();
            var textWriterMock = new Mock<ITextWriter>();

            const string fileToWrite = @"X:\some\made\up\dir\EnvCrypt.key";
            myFileMock.Setup(f => f.Exists(fileToWrite)).Returns(true);

            const string contents = "nothing signficant in this string for this test";

            // Act
            var writer = new StringToFileWriter(myDirMock.Object, myFileMock.Object, textWriterMock.Object);
            writer.Write(fileToWrite, contents,
                true, Encoding.Unicode);

            // Assert
            myDirMock.Verify(d => d.CreateDirectory(Path.GetDirectoryName(fileToWrite)),
                Times.Once);
            textWriterMock.Verify(f => f.WriteAllText(fileToWrite, contents, Encoding.Unicode),
                Times.Once);
        }
    }
}