using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RSACryptor
{
    public class CryptoHelper : IFileCrypter
    {
        private readonly string _rsaKey;

        // Declare CspParmeters and RsaCryptoServiceProvider
        // objects with global scope of your Form class.
        private CspParameters _cspp = new CspParameters();
        private RSACryptoServiceProvider _rsa;

        public CryptoHelper(string rsaKey)
        {
            _rsaKey = rsaKey;
        }

        public static string GenerateKey()
        {
            var Algorithm = new RSACryptoServiceProvider();

            //        Сохранить секретный ключ
            string CompleteKey = Algorithm.ToXmlString(true);
            byte[] KeyBytes = Encoding.UTF8.GetBytes(CompleteKey);

            //        Вернуть открытый ключ
            return Algorithm.ToXmlString(false);
        }


        public byte[] EncryptBytes(byte[] bytesToEncrypt)
        {

            if (_rsa == null)
            {
                throw new Exception("rsa key not set.");
            }

            byte[] result;
            // Create instance of Aes for
            // symmetric encryption of the data.
            var aes = Aes.Create();
            var transform = aes.CreateEncryptor();

            // Use RSACryptoServiceProvider to
            // encrypt the AES key.
            // rsa is previously instantiated:
            //    rsa = new RSACryptoServiceProvider(cspp);
            var keyEncrypted = _rsa.Encrypt(aes.Key, false);

            // Create byte arrays to contain
            // the length values of the key and IV.
            var LenK = new byte[4];
            var LenIV = new byte[4];

            var lKey = keyEncrypted.Length;
            LenK = BitConverter.GetBytes(lKey);
            var lIV = aes.IV.Length;
            LenIV = BitConverter.GetBytes(lIV);

            using (var outFs = new MemoryStream())
            {
                outFs.Write(LenK, 0, 4);
                outFs.Write(LenIV, 0, 4);
                outFs.Write(keyEncrypted, 0, lKey);
                outFs.Write(aes.IV, 0, lIV);

                // Now write the cipher text using
                // a CryptoStream for encrypting.
                using (var outStreamEncrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                {
                    outStreamEncrypted.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                    outStreamEncrypted.FlushFinalBlock();
                    return outFs.ToArray();
                }

                outFs.Close();
            }

            return result;
        }

        public byte[] DecryptBytes(byte[] bytesToDecrypt)
        {
            // Create instance of Aes for
            // symetric decryption of the data.
            Aes aes = Aes.Create();

            // Create byte arrays to get the length of
            // the encrypted key and IV.
            // These values were stored as 4 bytes each
            // at the beginning of the encrypted package.
            var LenK = new byte[4];
            var LenIV = new byte[4];


            // Use FileStream objects to read the encrypted
            // file (inFs) and save the decrypted file (outFs).
            using (var inFs = new MemoryStream(bytesToDecrypt))
            {
                inFs.Seek(0, SeekOrigin.Begin);
                inFs.Seek(0, SeekOrigin.Begin);
                inFs.Read(LenK, 0, 3);
                inFs.Seek(4, SeekOrigin.Begin);
                inFs.Read(LenIV, 0, 3);

                // Convert the lengths to integer values.
                var lenK = BitConverter.ToInt32(LenK, 0);
                var lenIV = BitConverter.ToInt32(LenIV, 0);

                // Determine the start postition of
                // the ciphter text (startC)
                // and its length(lenC).
                var startC = lenK + lenIV + 8;
                var lenC = (int)inFs.Length - startC;

                // Create the byte arrays for
                // the encrypted Aes key,
                // the IV, and the cipher text.
                var KeyEncrypted = new byte[lenK];
                var IV = new byte[lenIV];

                // Extract the key and IV
                // starting from index 8
                // after the length values.
                inFs.Seek(8, SeekOrigin.Begin);
                inFs.Read(KeyEncrypted, 0, lenK);
                inFs.Seek(8 + lenK, SeekOrigin.Begin);
                inFs.Read(IV, 0, lenIV);

                // Use RSACryptoServiceProvider
                // to decrypt the AES key.
                var KeyDecrypted = _rsa.Decrypt(KeyEncrypted, false);

                // Decrypt the key.
                var transform = aes.CreateDecryptor(KeyDecrypted, IV);

                // Decrypt the cipher text from
                // from the FileSteam of the encrypted
                // file (inFs) into the FileStream
                // for the decrypted file (outFs).
                using (MemoryStream outFs = new MemoryStream())
                {
                    var count = 0;
                    var offset = 0;

                    // blockSizeBytes can be any arbitrary size.
                    var blockSizeBytes = aes.BlockSize / 8;
                    var data = new byte[blockSizeBytes];

                    // By decrypting a chunk a time,
                    // you can save memory and
                    // accommodate large files.

                    // Start at the beginning
                    // of the cipher text.
                    inFs.Seek(startC, SeekOrigin.Begin);
                    using (var outStreamDecrypted = new CryptoStream(outFs, transform, CryptoStreamMode.Write))
                    {
                        outStreamDecrypted.Write(bytesToDecrypt, 0, bytesToDecrypt.Length);
                        outStreamDecrypted.FlushFinalBlock();
                        return outFs.ToArray();

                    }
                    outFs.Close();

                }
                inFs.Close();

            }
        }

        public void CreateKeyPair(string keyName)
        {
            // Stores a key pair in the key container.
            _cspp.KeyContainerName = keyName;
            _rsa = new RSACryptoServiceProvider(_cspp);
            _rsa.PersistKeyInCsp = true;
        }


        public void ReadPublicKey(string rsaKey)
        {
            _cspp.KeyContainerName = "keyName";
            _rsa = new RSACryptoServiceProvider(_cspp);
            _rsa.FromXmlString(rsaKey);
            _rsa.PersistKeyInCsp = true;
        }

        public void GetPrivateKey_Method()
        {
            _rsa = new RSACryptoServiceProvider(_cspp) { PersistKeyInCsp = true };
        }

    }
}