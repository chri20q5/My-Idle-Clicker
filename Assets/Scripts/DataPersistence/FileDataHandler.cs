using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "qew!pij#ac213¤3m%fef2l32&coå1o12fnmv@4rq€rqo@$@£€";


    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream fs = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (useEncryption)
                {
                    string[] parts = dataToLoad.Split('|');
                    if (parts.Length != 2)
                    {
                        string encryptedData = parts[0];
                        string storedHash = parts[1];

                        string decryptedData = EncryptDecrypt(encryptedData);

                        string computedHash = ComputeSHA256Hash(decryptedData);

                        if (storedHash == computedHash)
                        {
                            dataToLoad = decryptedData;
                        }
                        else
                        {
                            Debug.LogError("Data file is corrupted. Cannot decrypt data.");
                        }
                    }
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load data from file:" + fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            if (useEncryption)
            {
                string hash = ComputeSHA256Hash(dataToStore);

                string encryptedData = EncryptDecrypt(dataToStore);

                dataToStore = encryptedData + "|" + hash;
            }

            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file:" + fullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }

    private string ComputeSHA256Hash(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
