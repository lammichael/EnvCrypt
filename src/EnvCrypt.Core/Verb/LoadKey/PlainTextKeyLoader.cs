using EnvCrypt.Core.Key.PlainText;

namespace EnvCrypt.Core.Verb.LoadKey
{
    class PlainTextKeyLoader : IKeyLoader<PlainTextKey>
    {
        public PlainTextKey Load(string ecKeyFilePath)
        {
            return new PlainTextKey();
        }
    }
}
