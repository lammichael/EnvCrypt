namespace EnvCrypt.Core.Key
{
    public interface IKeyHasher<T> where T : KeyBase
    {
        byte[] GetHash();
    }
}
