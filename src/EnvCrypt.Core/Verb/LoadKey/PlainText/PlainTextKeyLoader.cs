using EnvCrypt.Core.Key.PlainText;

namespace EnvCrypt.Core.Verb.LoadKey.PlainText
{
    public class PlainTextKeyLoader : IKeyLoader<PlainTextKey, NullKeyLoaderDetails>
    {
        public PlainTextKey Load(NullKeyLoaderDetails notUsed)
        {
            return new PlainTextKey();
        }
    }
}
