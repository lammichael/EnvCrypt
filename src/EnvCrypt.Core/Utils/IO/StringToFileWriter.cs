using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace EnvCrypt.Core.Utils.IO
{
    class StringToFileWriter : IStringToFileWriter
    {
        private readonly IMyDirectory _myDirectory;
        private readonly IMyFile _myFile;
        private readonly ITextWriter _textWriter;

        public StringToFileWriter(IMyDirectory myDirectory, IMyFile myFile, ITextWriter textWriter)
        {
            Contract.Requires<ArgumentNullException>(myDirectory != null, "myDirectory");
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            //
            _myDirectory = myDirectory;
            _myFile = myFile;
            _textWriter = textWriter;
        }


        public void Write(string path, string contents, bool overwriteExisting, Encoding encoding)
        {
            if (_myFile.Exists(path))
            {
                if (!overwriteExisting)
                {
                    throw new EnvCryptException(
                        string.Format("file already exists at the path and you have asked it not to be overwritten: {0}", path));
                }
            }

            // Create directory
            var directoryOfPath = Path.GetDirectoryName(path);
            _myDirectory.CreateDirectory(directoryOfPath);

            // Write string to file
            _textWriter.WriteAllText(path, contents, encoding);
        }
    }
}