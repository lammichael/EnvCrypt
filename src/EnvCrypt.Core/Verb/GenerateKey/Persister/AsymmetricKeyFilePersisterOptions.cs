namespace EnvCrypt.Core.Verb.GenerateKey.Persister
{
    public class AsymmetricKeyFilePersisterOptions : KeyFilePersisterOptions
    {
        public string NewPrivateKeyFullFilePath { get; set; }
        public string NewPublicKeyFullFilePath { get; set; }
    }
}
