
namespace WebApplication1.Helpers
{
    public interface IFileCrypter
    {
        byte[] EncryptFile(byte[] bytesToEncrypt);
        byte[] DecryptFile(byte[] bytesToDecrypt);
        public static object ProtectedData { get; private set; }
    }
}
