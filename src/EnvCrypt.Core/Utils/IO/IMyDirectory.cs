using System;
using System.IO;

namespace EnvCrypt.Core.Utils.IO
{
    public interface IMyDirectory
    {
        DirectoryInfo CreateDirectory(String path);
        String[] GetFiles(String path);
    }


    class MyDirectory : IMyDirectory
    {
        public DirectoryInfo CreateDirectory(String path)
        {
            return Directory.CreateDirectory(path);
        }

        public string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }
    }
}
