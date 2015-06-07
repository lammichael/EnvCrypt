using System;
using System.IO;

namespace EnvCrypt.Core.Utils.IO
{
    public class MyFileInfo : IMyFileInfo
    {
        private readonly FileInfo _instance;

        public DateTime CreationTime
        {
            get { return _instance.CreationTime; }
        }

        public DateTime CreationTimeUtc
        {
            get { return _instance.CreationTimeUtc; }
        }


        public MyFileInfo(string fileName)
        {
            _instance = new FileInfo(fileName);
        }

        public void Delete()
        {
            _instance.Delete();
        }
    }
}