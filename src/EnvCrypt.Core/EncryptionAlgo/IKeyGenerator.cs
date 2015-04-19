using EnvCrypt.Core.Key;

namespace EnvCrypt.Core.EncryptionAlgo
{
    public interface IKeyGenerator<out T> where T : KeyBase
    {
        T GetNewKey();
    }
}