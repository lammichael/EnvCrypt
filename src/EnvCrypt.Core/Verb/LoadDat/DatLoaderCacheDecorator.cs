using EnvCrypt.Core.EncrypedData.Poco;

namespace EnvCrypt.Core.Verb.LoadDat
{
    class DatLoaderCacheDecorator<TOptions> : IDatLoader<TOptions> where TOptions : IDatLoaderOptions
    {
        private readonly IDatLoader<TOptions> _toDecorate;
        private EnvCryptDat _cachedResult = null;

        public DatLoaderCacheDecorator(IDatLoader<TOptions> toDecorate)
        {
            _toDecorate = toDecorate;
        }


        public EnvCryptDat Load(TOptions options)
        {
            return _cachedResult ?? (_cachedResult = _toDecorate.Load(options));
        }
    }
}