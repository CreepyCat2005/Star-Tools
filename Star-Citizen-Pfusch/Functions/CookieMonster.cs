using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Star_Citizen_Pfusch
{
    internal class CookieMonster
    {
     
        public static string GetCookieString()
        {
            var cookieList = GetCookies("%robertsspaceindustries%");

            string token = "";
            for (int i = 0; i < cookieList.Count; i++)
            {
                token += cookieList[i].Name + "=" + cookieList[i].Value + "; ";
            }

            return token;
        }
        private static List<Cookie> GetCookies(string hostname)
        {
            string ChromeCookiePath = Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Network\\Cookies";
            List<Cookie> data = new List<Cookie>();
            if (File.Exists(ChromeCookiePath))
            {
                try
                {
                    using (var conn = new SQLiteConnection($"Data Source={ChromeCookiePath}"))
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"SELECT name,encrypted_value,host_key FROM cookies WHERE host_key LIKE '{hostname}'";
                        byte[] key = AesGcm256.GetKey();

                        conn.Open();
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (!data.Any(a => a.Name == reader.GetString(0)))
                                {
                                    byte[] encryptedData = GetBytes(reader, 1);
                                    byte[] nonce, ciphertextTag;
                                    AesGcm256.prepare(encryptedData, out nonce, out ciphertextTag);
                                    string value = AesGcm256.decrypt(ciphertextTag, key, nonce);

                                    data.Add(new Cookie()
                                    {
                                        Name = reader.GetString(0),
                                        Value = value
                                    });
                                }
                            }
                        }
                        conn.Close();
                    }
                }
                catch { }
            }
            return data;

        }

        private static byte[] GetBytes(SQLiteDataReader reader, int columnIndex)
        {
            const int CHUNK_SIZE = 2 * 1024;
            byte[] buffer = new byte[CHUNK_SIZE];
            long bytesRead;
            long fieldOffset = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                while ((bytesRead = reader.GetBytes(columnIndex, fieldOffset, buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, (int)bytesRead);
                    fieldOffset += bytesRead;
                }
                return stream.ToArray();
            }
        }

        public class Cookie
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }



        class AesGcm256
        {
            public static byte[] GetKey()
            {
                string path = @"C:\Users\" + Environment.UserName + @"\AppData\Local\Google\Chrome\User Data\Local State";

                string v = File.ReadAllText(path);

                dynamic json = JsonConvert.DeserializeObject(v);
                string key = json.os_crypt.encrypted_key;

                byte[] src = Convert.FromBase64String(key);
                byte[] encryptedKey = src.Skip(5).ToArray();

                byte[] decryptedKey = ProtectedData.Unprotect(encryptedKey, null, DataProtectionScope.CurrentUser);

                return decryptedKey;
            }

            public static string decrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
            {
                string sR = String.Empty;
                try
                {
                    GcmBlockCipher cipher = new GcmBlockCipher(new AesEngine());
                    AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, iv, null);

                    cipher.Init(false, parameters);
                    byte[] plainBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
                    Int32 retLen = cipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, plainBytes, 0);
                    cipher.DoFinal(plainBytes, retLen);

                    sR = Encoding.UTF8.GetString(plainBytes).TrimEnd("\r\n\0".ToCharArray());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }

                return sR;
            }

            public static void prepare(byte[] encryptedData, out byte[] nonce, out byte[] ciphertextTag)
            {
                nonce = new byte[12];
                ciphertextTag = new byte[encryptedData.Length - 3 - nonce.Length];

                Array.Copy(encryptedData, 3, nonce, 0, nonce.Length);
                Array.Copy(encryptedData, 3 + nonce.Length, ciphertextTag, 0, ciphertextTag.Length);
            }
        }
    }
}
