namespace EnvCrypt.Core.EncrypedData
{
    public class Entry
    {
        public DecryptionInfo DecryptionInfo { get; set; }
        public byte[] EncryptedValue { get; set; }
    }
}