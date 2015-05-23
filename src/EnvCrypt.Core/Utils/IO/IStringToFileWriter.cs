using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvCrypt.Core.Utils.IO
{
    [ContractClass(typeof(StringToFileWriterContracts))]
    interface IStringToFileWriter
    {
        void Write(string path, string contents, bool overwriteExisting,Encoding encoding);
    }


    [ContractClassFor(typeof(IStringToFileWriter))]
    abstract class StringToFileWriterContracts : IStringToFileWriter
    {
        public void Write(string path, string contents, bool overwriteExisting, Encoding encoding)
        {
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(path), "path");
            Contract.Requires<ArgumentException>(!String.IsNullOrEmpty(contents), "contents");
            Contract.Requires<ArgumentNullException>(encoding != null, "encoding");
        }
    }


    class StringToFileWriter : IStringToFileWriter
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
            _myFile.WriteAllText(path, contents, encoding);
        }
    }
}
