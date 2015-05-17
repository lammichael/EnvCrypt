namespace EnvCrypt.Core.EncryptionAlgo.Aes.Key
{
    class AesKeyGenerationOptions : KeyGenerationOptions<AesKey>
    {
        public int KeySize { get; set; }
    }
}
