using System;
using System.Text;

namespace EnvCrypt.Core.Utils.IO
{
    class TextWriter : ITextWriter
    {
        private readonly IMyFile _myFile;

        public TextWriter(IMyFile myFile)
        {
            _myFile = myFile;
        }

        public void WriteAllText(String path, String contents, Encoding encoding)
        {
            _myFile.WriteAllText(path, contents, encoding);
        }
    }
}
