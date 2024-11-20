using ClassLibrary_DTOs.PasswordManager;
using System.Security.Cryptography;
using System.Text;

namespace MauiAppAdmin.Services
{
    public class EncryptionService
    {
        public CoreDTO EncryptData(CoreDTO coreDTO, string pass, string iv)
        {
            byte[] aesKey = GetAesKey(pass);

            coreDTO.Data01 = Encrypt(coreDTO.Data01, aesKey, iv);
            coreDTO.Data02 = Encrypt(coreDTO.Data02, aesKey, iv);
            coreDTO.Data03 = Encrypt(coreDTO.Data03, aesKey, iv);

            return coreDTO;
        }

        public CoreDTO DecryptData(CoreDTO coreDTO, string pass, string iv)
        {
            byte[] aesKey = GetAesKey(pass);

            coreDTO.Data01 = Decrypt(coreDTO.Data01, aesKey, iv);
            coreDTO.Data02 = Decrypt(coreDTO.Data02, aesKey, iv);
            coreDTO.Data03 = Decrypt(coreDTO.Data03, aesKey, iv);

            return coreDTO;
        }

        private string Encrypt(string plainText, byte[] key, string iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            byte[] encrypted = ms.ToArray();

            return Convert.ToBase64String(encrypted);
        }

        private string Decrypt(string encryptedText, byte[] key, string iv)
        {
            byte[] cipherBytes = Convert.FromBase64String(encryptedText);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = Convert.FromBase64String(iv);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }

        private byte[] GetAesKey(string pass)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(pass);

            while (keyBytes.Length < 32)
                keyBytes = keyBytes.Concat(keyBytes).ToArray(); // Concatenamos la clave

            return keyBytes.Take(32).ToArray();
        }
    }
}
