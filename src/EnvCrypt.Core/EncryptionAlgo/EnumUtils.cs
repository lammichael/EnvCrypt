namespace EnvCrypt.Core.EncryptionAlgo
{
    static class EnumUtils
    {
        public static AlgorithmTypeEnum GetType(this EnvCryptAlgoEnum algo)
        {
            if (algo == EnvCryptAlgoEnum.Aes)
            {
                return AlgorithmTypeEnum.Symmetric;
            }
            if (algo == EnvCryptAlgoEnum.Rsa)
            {
                return AlgorithmTypeEnum.Asymmetric;
            }
            throw new EnvCryptAlgoException("unrecognised algorithm: " + algo);
        }
    }
}
