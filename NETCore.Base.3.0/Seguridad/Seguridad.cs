using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace BCP.NETCore.Base
{
    public static class Seguridad
    {
        static RegistroLogs Logs = new RegistroLogs();
        public static class T3DES
        {
            static string key = "ACDCSCORPIONSGUNSANDROSESblacksabhataerosmith19961992";
            public static string EncryptKeyTripleDes(string cadena)
            {
                byte[] keyArray;
                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(cadena);
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);
                tdes.Clear();
                return Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);
            }
            public static string DecryptKeyTripleDes(string clave)
            {
                try
                {
                    byte[] keyArray;
                    byte[] Array_a_Descifrar = Convert.FromBase64String(clave);
                    MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                    TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                    tdes.Key = keyArray;
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tdes.CreateDecryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
                    tdes.Clear();
                    return UTF8Encoding.UTF8.GetString(resultArray);
                }
                catch (Exception Ex)
                {
                    return null;
                }
            }
        }
        
        public static class RSA
        {
            public static void Certificado(string NombreCertificado)
            {
                StoreLocation CertStoreLocation = StoreLocation.CurrentUser;
                StoreName CertStoreName = StoreName.My;
                X509Store store = new X509Store(CertStoreName, CertStoreLocation);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(store.Certificates, NombreCertificado, "Select a certificate from the following list to get information on that certificate", System.Security.Cryptography.X509Certificates.X509SelectionFlag.SingleSelection);
                foreach (X509Certificate2 cert in scollection)
                {

                    var rsa = cert.PrivateKey as RSACryptoServiceProvider;
                    if (rsa == null) continue; // not smart card cert again

                    if (!string.IsNullOrEmpty(rsa.CspKeyContainerInfo.KeyContainerName))
                    {
                        // This is how we can get it! :)  
                        var keyContainerName = rsa.CspKeyContainerInfo.KeyContainerName;
                        ContenedorLlaves(keyContainerName);
                    }
                }
            }
            private static CspParameters cp;
            public static void ContenedorLlaves(string KeyContainer)
            {
                try
                {
                    cp = new CspParameters();
                    cp.KeyContainerName = KeyContainer;
                    cp.Flags = CspProviderFlags.UseMachineKeyStore;
                    CspKeyContainerInfo info = new CspKeyContainerInfo(cp);
                    string fileName = info.UniqueKeyContainerName;
                }
                catch
                {
                    try
                    {
                        cp = new CspParameters();
                        cp.KeyContainerName = KeyContainer;
                        cp.Flags = CspProviderFlags.UseUserProtectedKey;
                        CspKeyContainerInfo info = new CspKeyContainerInfo(cp);
                        string fileName = info.UniqueKeyContainerName;
                    }
                    catch (Exception ex)
                    {
                        Logs.RegistrarEvento(TipoEvento.Error, ex, "Nombre de Llave: "+KeyContainer);
                    }
                }
            }
            private static RSACryptoServiceProvider rsaCryptoServiceProvider;
            public static string Encriptar(string inputString)
            {
                try
                {
                    rsaCryptoServiceProvider = new RSACryptoServiceProvider(RSA.cp);
                    rsaCryptoServiceProvider.PersistKeyInCsp = true;
                    int keySize = rsaCryptoServiceProvider.KeySize / 8;
                    byte[] bytes = Encoding.UTF32.GetBytes(inputString);
                    int maxLength = keySize - 42;
                    int dataLength = bytes.Length;
                    int iterations = dataLength / maxLength;
                    StringBuilder stringBuilder = new StringBuilder();
                    for (int i = 0; i <= iterations; i++)
                    {
                        byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                        Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                        byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);
                        Array.Reverse(encryptedBytes);
                        stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
                    }
                    return stringBuilder.ToString();
                }
                catch (CryptographicException ex)
                {
                    Logs.RegistrarEvento(TipoEvento.Error, ex);
                    return inputString;
                }
            }
            public static string Desencriptar(string inputString)
            {
                try
                {
                    rsaCryptoServiceProvider = new RSACryptoServiceProvider(RSA.cp);
                    rsaCryptoServiceProvider.PersistKeyInCsp = true;
                    int base64BlockSize = ((rsaCryptoServiceProvider.KeySize / 8) % 3 != 0) ? (((rsaCryptoServiceProvider.KeySize / 8) / 3) * 4) + 4 : ((rsaCryptoServiceProvider.KeySize / 8) / 3) * 4;
                    int iterations = inputString.Length / base64BlockSize;
                    ArrayList arrayList = new ArrayList();
                    for (int i = 0; i < iterations; i++)
                    {
                        byte[] encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
                        Array.Reverse(encryptedBytes);
                        arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
                    }
                    return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
                }
                catch (CryptographicException ex)
                {
                    Logs.RegistrarEvento(TipoEvento.Error, ex);
                    return inputString;
                }
            }
        }
    }
}
