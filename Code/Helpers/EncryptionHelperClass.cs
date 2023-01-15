using System.Security.Cryptography;
using System.Text;

namespace template.Code.Helpers;

// EXAMPLE
// string content = "Example test";
// string key = "E546C8DF278CD5931069B522E695D4F2";

// var encrypted = EncryptString(content, key);
// var decrypted = DecryptString(encrypted, key);

public class EncryptionHelperClass
{
      public static string EncryptString(string text, string keyString)
        {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
            using var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }
            byte[] iv = aesAlg.IV;
            byte[] decryptedContent = msEncrypt.ToArray();
            byte[] result = new byte[iv.Length + decryptedContent.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);
            return Convert.ToBase64String(result);
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            byte[] iv = new byte[16];
            byte[] cipher = new byte[16];
            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            using Aes aesAlg = Aes.Create();
            using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv))
            {
                string result;
                using (MemoryStream msDecrypt = new MemoryStream(cipher))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }

                return result;
            }
        }
        
//         public class ByteCipher
// {
//     // This constant is used to determine the keysize of the encryption algorithm in bits.
//     // We divide this by 8 within the code below to get the equivalent number of bytes.
//     private int _Keysize = (int)GlobalConfiguration.DataEncode_Key_Size;
//
//     private byte[] saltStringBytes;
//
//     private byte[] ivStringBytes;
//     // This constant determines the number of iterations for the password bytes generation function.
//     private const int DerivationIterations = 1000;
//     private string _passPhrase = GlobalConfiguration.DataEncode_Key;
//
//     private const string salt128 = "kljsdkkdlo4454GG";
//     private const string salt256 = "kljsdkkdlo4454GG00155sajuklmbkdl";
//
//     public ByteCipher(string passPhrase = null, DataCipherKeySize keySize = DataCipherKeySize.Key_128)
//     {
//         if (!string.IsNullOrEmpty(passPhrase?.Trim()))
//             _passPhrase = passPhrase;
//         _Keysize = keySize == DataCipherKeySize.Key_256 ? 256 : 128;
//         saltStringBytes = _Keysize == 256 ? Encoding.UTF8.GetBytes(salt256) : Encoding.UTF8.GetBytes(salt128);
//         ivStringBytes = _Keysize == 256 ? Encoding.UTF8.GetBytes("SSljsdkkdlo4454Maakikjhsd55GaRTP") : Encoding.UTF8.GetBytes("SSljsdkkdlo4454M");
//     }
//
//     public byte[] Encrypt(byte[] plainTextBytes)
//     {
//         if (plainTextBytes.Length <= 0)
//             return plainTextBytes;
//
//         using (var password = new Rfc2898DeriveBytes(_passPhrase, saltStringBytes, DerivationIterations))
//         {
//             var keyBytes = password.GetBytes(_Keysize / 8);
//             using (var symmetricKey = new RijndaelManaged())
//             {
//                 symmetricKey.BlockSize = _Keysize;
//                 symmetricKey.Mode = CipherMode.CBC;
//                 symmetricKey.Padding = PaddingMode.PKCS7;
//                 using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
//                 {
//                     using (var memoryStream = new MemoryStream())
//                     {
//                         using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
//                         {
//                             cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
//                             cryptoStream.FlushFinalBlock();
//                             // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
//                             var cipherTextBytes = saltStringBytes;
//                             cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
//                             cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
//                             memoryStream.Close();
//                             cryptoStream.Close();
//                             return cipherTextBytes;
//                         }
//                     }
//                 }
//             }
//         }
//     }
//
//     public byte[] Decrypt(byte[] cipherTextBytesWithSaltAndIv)
//     {
//         if (cipherTextBytesWithSaltAndIv.Length <= 0)
//             return cipherTextBytesWithSaltAndIv;
//         var v = Encoding.UTF8.GetString(cipherTextBytesWithSaltAndIv.Take(_Keysize / 8).ToArray());
//         if (v != salt256 && v != salt128)
//             return cipherTextBytesWithSaltAndIv;
//
//         var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((_Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((_Keysize / 8) * 2)).ToArray();
//
//         using (var password = new Rfc2898DeriveBytes(_passPhrase, saltStringBytes, DerivationIterations))
//         {
//             var keyBytes = password.GetBytes(_Keysize / 8);
//             using (var symmetricKey = new RijndaelManaged())
//             {
//                 symmetricKey.Mode = CipherMode.CBC;
//                 symmetricKey.Padding = PaddingMode.PKCS7;
//                 symmetricKey.BlockSize = _Keysize;
//
//                 using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
//                 {
//                     using (var memoryStream = new MemoryStream(cipherTextBytes))
//                     {
//                         using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
//                         {
//                             var plainTextBytes = new byte[cipherTextBytes.Length];
//                             var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
//                             memoryStream.Close();
//                             cryptoStream.Close();
//                             return plainTextBytes;
//                         }
//                     }
//                 }
//             }
//         }
//     }
// }
}