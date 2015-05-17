using System;
using System.IO;
using System.Text;

namespace EnvCrypt.Core.Utils.IO
{
    interface IMyFile
    {
        void WriteAllText(String path, String contents);
        void WriteAllText(String path, String contents, Encoding encoding);
        bool Exists(String path);
    }


    class MyFile : IMyFile
    {
        public void WriteAllText(String path, String contents)
        {
            File.WriteAllText(path, contents);
        }

        public void WriteAllText(String path, String contents, Encoding encoding)
        {
            File.WriteAllText(path, contents);
        }

        public bool Exists(String path)
        {
            return File.Exists(path);
        }
    }
}
