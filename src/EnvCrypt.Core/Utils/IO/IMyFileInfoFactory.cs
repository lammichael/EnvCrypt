using System;

namespace EnvCrypt.Core.Utils.IO
{
    public interface IMyFileInfoFactory
    {
        IMyFileInfo GetNewInstance(String fileName);
    }

    public class MyFileInfoFactory : IMyFileInfoFactory
    {
        public IMyFileInfo GetNewInstance(string fileName)
        {
            return new MyFileInfo(fileName);
        }
    }
}
