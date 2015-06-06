using EnvCrypt.Core.Utils.IO.StringWriter;

namespace EnvCrypt.Console.GenerateKey
{
    class ToConsoleTextWriter : IStringWriter<StringToFileWriterOptions>
    {
        public void Write(StringToFileWriterOptions options)
        {
            System.Console.WriteLine(options.Contents);
            System.Console.WriteLine();
        }
    }
}
