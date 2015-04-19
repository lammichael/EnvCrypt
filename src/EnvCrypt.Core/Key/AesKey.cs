namespace EnvCrypt.Core.Key
{
    public class AesKey : KeyBase
    {
        public byte[] Key { get; set; }
        public byte[] Iv { get; set; }
    }
}
