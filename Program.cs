using System;
using System.IO;
using System.Security.Cryptography;

namespace FileEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get the file to encrypt
            Console.WriteLine("Enter the file path:");
            string filePath = Console.ReadLine();

            // Get the password to use for encryption
            Console.WriteLine("Enter the password:");
            string password = Console.ReadLine();

            // Create a new instance of the AesCryptoServiceProvider
            // class with a 256-bit key and a 16-byte initialization vector
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.IV = new byte[16];
            aes.Key = GetKey(password);

            // Encrypt the file
            EncryptFile(filePath, aes);

            Console.WriteLine("Encryption complete. Press any key to exit.");
            Console.ReadKey();
        }

        // This method generates the key used for encryption
        static byte[] GetKey(string password)
        {
            // Create a new instance of the Rfc2898DeriveBytes class
            // with the specified password and salt
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, new byte[16]);

            // Return the derived key
            return key.GetBytes(32);
        }

        // This method encrypts the specified file using the specified
        // AesCryptoServiceProvider object
        static void EncryptFile(string filePath, AesCryptoServiceProvider aes)
        {
            // Create a new instance of the FileStream class with the
            // specified file path and access mode
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            // Create a new instance of the CryptoStream class with the
            // specified FileStream and CryptoTransform object
            CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            // Encrypt the file data
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                cryptoStream.Write(buffer, 0, bytesRead);
            }

            // Close the streams
            fileStream.Close();
            cryptoStream.Close();
        }
    }
}
