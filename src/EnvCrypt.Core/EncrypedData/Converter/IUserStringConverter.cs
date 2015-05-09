namespace EnvCrypt.Core.EncrypedData.Converter
{
    interface IUserStringConverter
    {
        byte[] Encode(string userStr);
        string Decode(byte[] decrypedData);
    }
}
