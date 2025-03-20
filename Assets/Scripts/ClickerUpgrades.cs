using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUpgrades : MonoBehaviour, IDataPersistence
{
    [Header("Components")]
    public TMP_Text priceText;
    public TMP_Text levelText;
    public Button buyButton;
    public string upgradeID;

    [Header("Price")]
    public float startPrice = 25f;
    public float priceMultiplier = 1.8f;

    [Header("Power")]
    public float powerBase = 1.3f;
    public float baseClickPower = 1f;


    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        upgradeID = Guid.NewGuid().ToString();
    }


    [Header("Managers")]
    public Clicker clicker;

    private int level = 0;

    private void Start()
    {
        UpdateUI();
    }

    public void LoadData(GameData data)
    {
        this.level = data.clickerUpgradesLevel;
    }

    public void SaveData(ref GameData data)
    {
        data.clickerUpgradesLevel = this.level;

    }
    private void Update()
    {
        bool canAfford = clicker.currencyCount >= CalculatePrice();
        buyButton.interactable = canAfford;
    }

    public void ClickAction()
    {
        int price = CalculatePrice();
        bool purchaseSuccesful = clicker.purchaseAction(price);

        if (purchaseSuccesful)
        {
            level++;
            UpdateClickPower();
            UpdateUI();
        }
    }

    private void UpdateClickPower()
    {
        clicker.clickPower = baseClickPower * Mathf.Pow(powerBase, level);
    }

    private int CalculatePrice()
    {
        return Mathf.RoundToInt(startPrice * Mathf.Pow(priceMultiplier, level));
    }

    private void UpdateUI()
    {
        priceText.text = CalculatePrice().ToString();
        levelText.text = $"Level: {level}";
    }
}