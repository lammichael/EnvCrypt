using System;
using System.IO;

namespace EnvCrypt.Core.Utils.IO
{
    interface IMyDirectory
    {
        DirectoryInfo CreateDirectory(String path);
    }


    class MyDirectory : IMyDirectory
    {
        public DirectoryInfo CreateDirectory(String path)
        {
            return Directory.CreateDirectory(path);
        }
    }
}
