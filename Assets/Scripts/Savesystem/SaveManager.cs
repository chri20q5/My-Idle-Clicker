using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager instance;

    public static SaveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveManager>();
                if (instance == null)
                {
                    GameObject saveManagerObj = new GameObject("SaveManager");
                    instance = saveManagerObj.AddComponent<SaveManager>();
                    DontDestroyOnLoad(saveManagerObj);
                }
            }
            return instance;
        }
    }

    private string saveFilePath;
    private readonly string encryptionKey = "E+d72f77SNs85hc0gtLAOb9YseVJQj8g9Ss4luMlLCtPwMO1OA7IZOh+W6iUdoB4";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, saveFilePath);
    }

    public void SaveGame(Clicker clicker)
    {
        try
        {
            SaveData saveData = new SaveData
            {
                count = clicker.count,
                clickPower = clicker.clickPower,
                saveTime = DateTime.Now.ToString(),
                saveVersion = 1
            };

            foreach (var upgrade in clicker.upgrades)
            {
                string upgradeKey = upgrade.gameObject.name;

                ClickerUpgrades clickerUpgrade = upgrade as ClickerUpgrades;
                if (clickerUpgrade != null)
                {
                    saveData.upgradeLevels[upgradeKey] = clickerUpgrade.level;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to save game: {e.Message}");
        }
    }

    public SaveData LoadGame()
    {
        try
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.Log("No save file found. Starting new game.");
                return null;
            }

            string encryptedData = File.ReadAllText(saveFilePath);

            string jsonData = DecryptData(encryptedData);

            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(jsonData);

            Debug.Log($"Game loaded Successfully! Last saved: {saveData.saveTime}");
            return saveData;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load game: {e.Message}");
            return null;
        }
    }

    private string EncryptData(string data)
    {
        byte[] dataBytes = System.Text.Encoding.UTF8.GetBytes(data);

        using (Aes aes = Aes.Create())
        {
            byte[] key = new byte[32];
            byte[] passwordBytes = Encoding.UTF8.GetBytes(encryptionKey);
            Array.Copy(passwordBytes, key, Math.Min(passwordBytes.Length, key.Length));

            aes.Key = key;
            aes.IV = new byte[16];

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataBytes, 0, dataBytes.Length);
                    cs.Close();
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private string DecryptData(string encryptedData)
    {
        byte[] encryptedbytes = Convert.FromBase64String(encryptedData);

        using (Aes aes = Aes.Create())
        {
            byte[] key = new byte[32];
            byte[] passwordBytes = Encoding.UTF8.GetBytes(encryptionKey);
            Array.Copy(passwordBytes, key, Math.Min(passwordBytes.Length, key.Length));

            aes.Key = key;
            aes.IV = new byte[16];

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedbytes, 0, encryptedbytes.Length);
                    cs.Close();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Save file deleted.");
        }
    }
}
