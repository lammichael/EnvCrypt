using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace EnvCrypt.Core.Utils.IO.StringWriter
{
    public class StringToFileWriter : IStringWriter<StringToFileWriterOptions>
    {
        private readonly IMyDirectory _myDirectory;
        private readonly IMyFile _myFile;

        public StringToFileWriter(IMyDirectory myDirectory, IMyFile myFile)
        {
            Contract.Requires<ArgumentNullException>(myDirectory != null, "myDirectory");
            Contract.Requires<ArgumentNullException>(myFile != null, "myFile");
            //
            _myDirectory = myDirectory;
            _myFile = myFile;
        }


        public void Write(StringToFileWriterOptions options)
        {
            if (_myFile.Exists(options.Path))
            {
                if (!options.OverwriteIfFileExists)
                {
                    throw new EnvCryptException(
                        string.Format("file already exists at the path and you have asked it not to be overwritten: {0}", options.Path));
                }
            }

            // Create directory
            var directoryOfPath = Path.GetDirectoryName(options.Path);
            _myDirectory.CreateDirectory(directoryOfPath);

            // Write string to file
            _myFile.WriteAllText(options.Path, options.Contents, options.Encoding);
        }
    }
}