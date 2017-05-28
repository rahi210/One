using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace WR.Utils
{
    public class Encrypt3DES
    {
        private static byte[] key = Convert.FromBase64String("YWJjZGVmZ2hpamtsbW5vcHFyc3R1dnd4");
        private static byte[] iv = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };      //当模式为ECB时，IV无用   

        #region CBC模式**

        public static string EncryptCBC(string data)
        {
            //System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            //////key为abcdefghijklmnopqrstuvwx的Base64编码
            //byte[] data1 = utf8.GetBytes(data);
            ////System.Console.WriteLine("ECB模式:");
            ////byte[] str1 = EncryptECB(data1);
            ////byte[] str2 = DecryptECB(str1);
            ////System.Console.WriteLine(Convert.ToBase64String(str1));
            ////System.Console.WriteLine(System.Text.Encoding.UTF8.GetString(str2));
            ////System.Console.WriteLine();
            ////System.Console.WriteLine("CBC模式:");
            //byte[] str3 = EncryptCBC(data1);
            //byte[] str4 = DecryptCBC(str3);
            //System.Console.WriteLine(Convert.ToBase64String(str3));
            //System.Console.WriteLine(utf8.GetString(str4));
            //System.Console.WriteLine();

            byte[] d1 = Encoding.UTF8.GetBytes(data);
            byte[] d2 = EncryptCBC(d1);
            string result = Convert.ToBase64String(d2);

            return result;
        }

        /// <summary>   
        /// DES3 CBC模式加密   
        /// </summary>   
        /// <param name="data">明文的byte数组</param>   
        /// <returns>密文的byte数组</returns>   
        public static byte[] EncryptCBC(byte[] data)
        {
            //复制于MSDN   
            try
            {
                // Create a MemoryStream.   
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值   
                tdsp.Padding = PaddingMode.PKCS7;       //默认值   
                // Create a CryptoStream using the MemoryStream    
                // and the passed key and initialization vector (IV).   
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.   
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the    
                // MemoryStream that holds the    
                // encrypted data.   
                byte[] ret = mStream.ToArray();
                // Close the streams.   
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.   
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        public static string DecryptCBC(string data)
        {
            byte[] d1 = Convert.FromBase64String(data);
            byte[] d2 = DecryptCBC(d1);
            return Encoding.UTF8.GetString(d2);
        }

        /// <summary>   
        /// DES3 CBC模式解密   
        /// </summary>    
        /// <param name="data">密文的byte数组</param>   
        /// <returns>明文的byte数组</returns>   
        public static byte[] DecryptCBC(byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed    
                // array of encrypted data.   
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream    
                // and the passed key and initialization vector (IV).   
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.   
                //byte[] fromEncrypt = new byte[data.Length];
                // Read the decrypted data out of the crypto stream   
                // and place it into the temporary buffer.
                List<byte> result = new List<byte>();
                int i =0;
                while ((i = csDecrypt.ReadByte()) != -1)
                {
                    result.Add((byte)i);
                }
                //csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                //Convert the buffer into a string and return it.   
                return result.ToArray();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        #endregion
        #region ECB模式

        public static string EncryptECB(string data)
        {
            byte[] d1 = Encoding.UTF8.GetBytes(data);
            byte[] d2 = EncryptECB(d1);
            string result = Convert.ToBase64String(d2);

            return result;
        }

        /// <summary>   
        /// DES3 ECB模式加密   
        /// </summary>
        /// <param name="str">明文的byte数组</param>   
        /// <returns>密文的byte数组</returns>   
        public static byte[] EncryptECB(byte[] data)
        {
            try
            {
                // Create a MemoryStream.   
                MemoryStream mStream = new MemoryStream();
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream    
                // and the passed key and initialization vector (IV).   
                CryptoStream cStream = new CryptoStream(mStream,
                    tdsp.CreateEncryptor(key, iv),
                    CryptoStreamMode.Write);
                // Write the byte array to the crypto stream and flush it.   
                cStream.Write(data, 0, data.Length);
                cStream.FlushFinalBlock();
                // Get an array of bytes from the    
                // MemoryStream that holds the    
                // encrypted data.   
                byte[] ret = mStream.ToArray();
                // Close the streams.   
                cStream.Close();
                mStream.Close();
                // Return the encrypted buffer.   
                return ret;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        public static string DecryptECB(string data)
        {
            byte[] d1 = Convert.FromBase64String(data);
            byte[] d2 = DecryptECB(d1);
            return Encoding.UTF8.GetString(d2);
        }

        /// <summary>   
        /// DES3 ECB模式解密   
        /// </summary>
        /// <param name="str">密文的byte数组</param>   
        /// <returns>明文的byte数组</returns>   
        public static byte[] DecryptECB(byte[] data)
        {
            try
            {
                // Create a new MemoryStream using the passed    
                // array of encrypted data.   
                MemoryStream msDecrypt = new MemoryStream(data);
                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.ECB;
                tdsp.Padding = PaddingMode.PKCS7;
                // Create a CryptoStream using the MemoryStream    
                // and the passed key and initialization vector (IV).   
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(key, iv),
                    CryptoStreamMode.Read);
                // Create buffer to hold the decrypted data.   
                //byte[] fromEncrypt = new byte[data.Length];
                //// Read the decrypted data out of the crypto stream   
                //// and place it into the temporary buffer.   
                //csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);
                List<byte> result = new List<byte>();
                int i = 0;
                while ((i = csDecrypt.ReadByte()) != -1)
                {
                    result.Add((byte)i);
                }
                //Convert the buffer into a string and return it.   
                return result.ToArray();
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }
        #endregion

        /// <summary>   
        /// 类测试   
        /// </summary>   
        public static void Test()
        {
            System.Text.Encoding utf8 = System.Text.Encoding.UTF8;
            //key为abcdefghijklmnopqrstuvwx的Base64编码   

            byte[] data = utf8.GetBytes("中国ABCabc123");
            System.Console.WriteLine("ECB模式:");
            byte[] str1 = Encrypt3DES.EncryptECB(data);
            byte[] str2 = Encrypt3DES.DecryptECB(str1);
            System.Console.WriteLine(Convert.ToBase64String(str1));
            System.Console.WriteLine(System.Text.Encoding.UTF8.GetString(str2));
            System.Console.WriteLine();
            System.Console.WriteLine("CBC模式:");
            byte[] str3 = Encrypt3DES.EncryptCBC(data);
            byte[] str4 = Encrypt3DES.DecryptCBC(str3);
            System.Console.WriteLine(Convert.ToBase64String(str3));
            System.Console.WriteLine(utf8.GetString(str4));
            System.Console.WriteLine();
        }
    }
}