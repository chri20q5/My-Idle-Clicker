using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Generator1Upgrade : MonoBehaviour, IDataPersistence
{
    [Header("References")] public TMP_Text priceText;
    public TMP_Text levelText;
    public TMP_Text effectText;
    public Button buyButton;
    public GeneratorUpgrades targetGenerator;
    public Clicker clicker;

    [Header("Settings")]
    public string upgradeID;

    public float startPrice;
    public float priceMultiplier;
    public float baseBoostPercent;
    public float boostDuration;

    private int level = 0;
    private bool boostActive = false;
    private float boostTimer = 0f;
    private float currentBoostMultiplier = 1f;

    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        upgradeID = Guid.NewGuid().ToString();
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        bool canAfford = clicker.currencyCount >= CalculatePrice();
        buyButton.interactable = canAfford;

        if (boostActive)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                boostActive = false;
                currentBoostMultiplier = 1f;
            }
        }
    }

    public void ClickAction()
    {
        int price = CalculatePrice();
        bool purchaseSuccesful = clicker.purchaseAction(price);

        if (purchaseSuccesful)
        {
            level++;
            UpdateUI();
        }
    }

    public void TriggerBoostEffect()
    {
        if (level > 0 && targetGenerator != null)
        {
            boostActive = true;
            boostTimer = boostDuration;

            currentBoostMultiplier = 1f + (baseBoostPercent * level / 100f);
        }
    }

    public float GetCurrentBoostMultiplier()
    {
        return boostActive ? currentBoostMultiplier : 1f;
    }

    private int CalculatePrice()
    {
        return Mathf.RoundToInt(startPrice * Mathf.Pow(priceMultiplier, level));
    }

    private void UpdateUI()
    {
        priceText.text = CalculatePrice().ToString();
        levelText.text = $"Level: {level}";
        float boostPercent = baseBoostPercent * level;
        effectText.text = $"+{boostPercent}% to Generator 1 when clicking";
    }

    public void LoadData(GameData data)
    {
        data.generatorBoostUpgradeLevel.TryGetValue(upgradeID, out level);
    }

    public void SaveData(ref GameData data)
    {
        data.generatorBoostUpgradeLevel.TryGetValue(upgradeID, out level);
    }
}
