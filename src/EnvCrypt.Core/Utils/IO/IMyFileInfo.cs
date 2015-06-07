using System;

namespace EnvCrypt.Core.Utils.IO
{
    public interface IMyFileInfo
    {
        DateTime CreationTime { get; }
        DateTime CreationTimeUtc { get; }
        void Delete();
    }
}
