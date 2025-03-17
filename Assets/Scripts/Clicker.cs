using System;
using TMPro;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text incomeText;
    [SerializeField] public Upgrade[] upgrades;


    [HideInInspector] public float count = 0;
    [HideInInspector] public float clickPower = 1f;

    [SerializeField] private float autoSaveInterval = 60f;
    private float autoSaveTimer;

    private void Start()
    {
        LoadGame();
        autoSaveTimer = autoSaveInterval;
        UpdateUI();
    }

    private void Update()
    {
        autoSaveTimer -= Time.deltaTime;
        if (autoSaveTimer <= 0)
        {
            SaveGame();
            autoSaveTimer = autoSaveInterval;
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    public void ActiveClicker()
    {
        count += clickPower;
        UpdateUI();
    }

    public void AddIncome(float amount)
    {
        count += amount;
        UpdateUI();
    }

    public bool purchaseAction(int cost)
    {
        if (count >= cost)
        {
            count -= cost;
            UpdateUI();
            return true;
        }

        return false;
    }

    public void SaveGame()
    {
        SaveManager.Instance.SaveGame(this);
    }

    public void LoadGame()
    {
        SaveData saveData = SaveManager.Instance.LoadGame();
        if (saveData != null)
        {
            return;
        }

        count = saveData.count;
        clickPower = saveData.clickPower;

        foreach (var upgrade in upgrades)
        {
            string upgradeKey = upgrade.gameObject.name;

            if (saveData.upgradeLevels.ContainsKey(upgradeKey))
            {
                int savedLevel = saveData.upgradeLevels[upgradeKey];
                ApplyUpgradeLevel(upgrade, savedLevel);
            }
        }

        Update();
    }

    private void ApplyUpgradeLevel(Upgrade upgrade, int SavedLevel)
    {
        ClickerUpgrades clickerUpgrade = upgrade as ClickerUpgrades;
        if (clickerUpgrade != null)
        {
            clickerUpgrade.level = SavedLevel;

            clickerUpgrade.UpdateClickPower();
            clickerUpgrade.UpdateUI();
        }
    }
    void UpdateUI()
    {
     countText.text = Mathf.RoundToInt(count).ToString();
    }
}