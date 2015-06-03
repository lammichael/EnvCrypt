namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    public class AsymmetricKeyToFilePersisterOptions : KeyPersisterOptions
    {
        public string NewKeyName { get; set; }
        public string NewPrivateKeyFullFilePath { get; set; }
        public string NewPublicKeyFullFilePath { get; set; }
    }
}
