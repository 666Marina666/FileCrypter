
namespace RSACryptor
{
    public interface IFileCrypter
    {
        byte[] EncryptBytes(byte[] bytesToEncrypt);
        byte[] DecryptBytes(byte[] bytesToDecrypt);
        //public static object ProtectedData { get; private set; }
    }
}
