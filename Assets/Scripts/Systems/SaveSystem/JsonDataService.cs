using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;


public class JsonDataService : IDataService
{
    private const string KEY = "ggdPhkeOoiv6YMiPWa34kIuOdDUL7NwQFg6l1DVdwN8=";
    private const string IV = "JZuM0HQsWSBVpRHTeRZMYQ==";

    public bool SaveData<T>(string relativePath, T data, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("Data exists. Deleting old file and writing a new one!");
                File.Delete(path);
            }
            else
            {
                Debug.Log("Writing file for the first time!");
            }

            using FileStream stream = File.Create(path);
            
            if (encrypted)
            {
                WriteEncryptedData(data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
            }

            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
            return false;
        }
    }

    private void WriteEncryptedData<T>(T Data, FileStream Stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            Stream,
            cryptoTransform,
            CryptoStreamMode.Write
        );

        byte[] dataBytes = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(Data));

        // Use the Length property to get the count (number of bytes)
        int count = dataBytes.Length;

        // Write the data to the CryptoStream
        cryptoStream.Write(dataBytes, 0, count);

        // Flush and close the CryptoStream and underlying streams
        cryptoStream.FlushFinalBlock();
        cryptoStream.Close();
    }


    public T LoadData<T>(string relativePath, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;
        Debug.Log($"Attempting to load data from file: {path}");

        try
        {
            T data;

            if (File.Exists(path))
            {
                if (encrypted)
                {
                    data = ReadEncryptedData<T>(path);
                }
                else
                {
                    string jsonContent = File.ReadAllText(path);


                    //data = JsonConvert.DeserializeObject<T>(jsonContent);
                    data = JsonUtility.FromJson<T>(jsonContent);
                    Debug.Log($"Deserialized Object Contents: {JsonUtility.ToJson(data)}");
                }
            }
            else
            {
                Debug.LogWarning($"File at {path} does not exist. Creating a new instance.");

                // Don't create a new instance immediately, load the default data from elsewhere
                T defaultData = GetDefaultData<T>();
                SaveData(relativePath, defaultData, encrypted);

                return defaultData;
            }

            Debug.Log($"Object Contents: {JsonUtility.ToJson(data)}");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    private T GetDefaultData<T>()
    {
        // You may implement logic to return default data based on the type T
        // For now, just create a new instance
        return Activator.CreateInstance<T>();
    }

    private T ReadEncryptedData<T>(string Path)
    {
        byte[] fileBytes = File.ReadAllBytes(Path);

        using Aes aesProvider = Aes.Create();

        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV);

        using MemoryStream decryptionStream = new MemoryStream(fileBytes);
        using CryptoStream cryptoStream = new CryptoStream(decryptionStream, cryptoTransform, CryptoStreamMode.Read);
        using StreamReader reader = new StreamReader(cryptoStream);

        // Create a StringBuilder to efficiently handle large amounts of data
        StringBuilder decryptedData = new StringBuilder();

        // Read the decrypted data in chunks
        char[] buffer = new char[4096]; // Adjust the buffer size as needed
        int bytesRead;
        while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            decryptedData.Append(buffer, 0, bytesRead);
        }

        string result = decryptedData.ToString();

        Debug.Log($"Decrypted result: {result}");

        return JsonConvert.DeserializeObject<T>(result);
    }
}