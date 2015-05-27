namespace EnvCrypt.Core.Utils.IO
{
    public class TextReader : ITextReader
    {
        private readonly IMyFile _myFile;

        public TextReader(IMyFile myFile)
        {
            _myFile = myFile;
        }

        public string ReadAllText(string path)
        {
            return _myFile.ReadAllText(path);
        } 
    }
}
