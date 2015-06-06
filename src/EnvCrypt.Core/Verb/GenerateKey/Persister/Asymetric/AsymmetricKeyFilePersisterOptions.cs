namespace EnvCrypt.Core.Verb.GenerateKey.Persister.Asymetric
{
    public class AsymmetricKeyFilePersisterOptions : KeyFilePersisterOptions
    {
        public string NewPrivateKeyFullFilePath { get; set; }
        public string NewPublicKeyFullFilePath { get; set; }
    }
}
