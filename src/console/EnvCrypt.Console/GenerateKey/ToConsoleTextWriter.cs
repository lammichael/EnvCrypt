using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvCrypt.Core.Utils.IO;

namespace EnvCrypt.Console.GenerateKey
{
    class ToConsoleTextWriter : ITextWriter
    {
        public void WriteAllText(string path, string contents, Encoding encoding)
        {
            System.Console.WriteLine(contents);
            System.Console.WriteLine();
        }
    }
}
