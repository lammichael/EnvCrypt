using System;
using System.IO;
using System.Text;

namespace EnvCrypt.Core.Utils.IO
{
    public interface IMyFile
    {
        void WriteAllText(string path, string contents);
        void WriteAllText(string path, string contents, Encoding encoding);
        bool Exists(string path);
        string ReadAllText(string path);
    }


    class MyFile : IMyFile
    {
        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
