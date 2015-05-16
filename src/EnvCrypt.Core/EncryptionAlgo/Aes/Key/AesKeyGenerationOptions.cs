namespace EnvCrypt.Core.EncryptionAlgo.Aes.Key
{
    public class AesKeyGenerationOptions : IKeyGenerationOptions<AesKey>
    {
        public int KeySize { get; set; }
    }
}
