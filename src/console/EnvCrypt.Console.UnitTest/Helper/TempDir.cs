using System;
using System.IO;

namespace EnvCrypt.Console.UnitTest.Helper
{
    class TempDir : IDisposable
    {
        public string TempDirectory { get; private set; }

        public TempDir()
        {
            TempDirectory = Path.GetTempFileName();
            File.Delete(TempDirectory);
            Directory.CreateDirectory(TempDirectory);
        }

        public void Dispose()
        {
            Directory.Delete(TempDirectory, true);
        }


        public override string ToString()
        {
            return TempDirectory;
        }
    }
}
