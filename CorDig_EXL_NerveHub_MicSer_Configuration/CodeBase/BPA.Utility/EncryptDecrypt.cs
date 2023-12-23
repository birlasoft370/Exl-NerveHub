using System.Security.Cryptography;
using System.Text;

namespace BPA.Utility
{
    public static class EncryptDecrypt
    {
        static string Key = "EmailManagement";
        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] keyBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:  
            byte[] saltBytes = keyBytes;
            // Example:  
            //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };  

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] keyBytes)
        {
            try
            {
                byte[] decryptedBytes = null;
                // Set your salt here to meet your flavor:  
                byte[] saltBytes = keyBytes;
                // Example:  
                //saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };  

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        //AES.Mode = CipherMode.CBC;  

                        using (CryptoStream cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            //If(cs.Length = ""  
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }
                return decryptedBytes;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }
        public static string Encrypt(string text)
        {
            string strReturn = "";

            byte[] originalBytes = Encoding.UTF8.GetBytes(text);
            byte[] encryptedBytes = null;
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

            // Hash the key with SHA512  
            keyBytes = SHA512Managed.Create().ComputeHash(keyBytes);

            // Getting the salt size  
            int saltSize = GetSaltSize(keyBytes);
            // Generating salt bytes  
            byte[] saltBytes = GetRandomBytes(saltSize);

            // Appending salt bytes to original bytes  
            byte[] bytesToBeEncrypted = new byte[saltBytes.Length + originalBytes.Length];
            for (int i = 0; i < saltBytes.Length; i++)
            {
                bytesToBeEncrypted[i] = saltBytes[i];
            }
            for (int i = 0; i < originalBytes.Length; i++)
            {
                bytesToBeEncrypted[i + saltBytes.Length] = originalBytes[i];
            }

            encryptedBytes = AES_Encrypt(bytesToBeEncrypted, keyBytes);
            strReturn = Convert.ToBase64String(encryptedBytes);
            strReturn = strReturn.Replace("/", "ampC7Aamp");
            strReturn = strReturn.Replace("=", "amtC7Aamt");
            strReturn = strReturn.Replace("+", "amzC7Aamz");
            return strReturn;
        }
        public static string Decrypt(string decryptedText)
        {
            string strReturn = "";
            strReturn = decryptedText.Replace("ampC7Aamp", "/");
            strReturn = strReturn.Replace("amtC7Aamt", "=");
            strReturn = strReturn.Replace("amzC7Aamz", "+");

            byte[] bytesToBeDecrypted = Convert.FromBase64String(strReturn);
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

            // Hash the key with SHA512  
            keyBytes = SHA512Managed.Create().ComputeHash(keyBytes);

            byte[] decryptedBytes = AES_Decrypt(bytesToBeDecrypted, keyBytes);

            if (decryptedBytes != null)
            {
                // Getting the size of salt  
                int saltSize = GetSaltSize(keyBytes);

                // Removing salt bytes, retrieving original bytes  
                byte[] originalBytes = new byte[decryptedBytes.Length - saltSize];
                for (int i = saltSize; i < decryptedBytes.Length; i++)
                {
                    originalBytes[i - saltSize] = decryptedBytes[i];
                }
                return Encoding.UTF8.GetString(originalBytes);
            }
            else
            {
                return null;
            }
        }
        private static int GetSaltSize(byte[] keyBytes)
        {
            var key = new Rfc2898DeriveBytes(keyBytes, keyBytes, 2000);
            byte[] ba = key.GetBytes(2);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ba.Length; i++)
            {
                sb.Append(Convert.ToInt32(ba[i]).ToString());
            }
            int saltSize = 0;
            string s = sb.ToString();
            foreach (char c in s)
            {
                int intc = Convert.ToInt32(c.ToString());
                saltSize = saltSize + intc;
            }

            return saltSize;
        }
        private static byte[] GetRandomBytes(int length)
        {
            byte[] ba = new byte[length];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

    }
}