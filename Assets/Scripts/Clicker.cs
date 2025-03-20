using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Clicker : MonoBehaviour, IDataPersistence
{
    [SerializeField] TMP_Text currencyText;
    [SerializeField] GeneratorUpgrades[] generatorUpgrades;

    [HideInInspector] public float currencyCount = 0;
    [HideInInspector] public int premiumCurrencyCount = 0;
    [HideInInspector] public float clickPower = 1f;

    public PremiumCurrency premiumCurrency;

    private void Start()
    {
        UpdateUI();
    }

    public void LoadData(GameData data)
    {
        this.currencyCount = data.currencyCount;
        this.premiumCurrencyCount = data.premiumCurrencyCount;
    }

    public void SaveData(ref GameData data)
    {
        data.currencyCount = this.currencyCount;
        data.premiumCurrencyCount = this.premiumCurrencyCount;
    }


    public void ActiveClicker()
    {
        currencyCount += clickPower;
        premiumCurrency.PremiumFromClick();

        Generator1Upgrade[] boostUpgrades = FindObjectsByType<Generator1Upgrade>(FindObjectsSortMode.None);
        foreach (var boostUpgrade in boostUpgrades)
        {
            boostUpgrade.TriggerBoostEffect();
        }

        UpdateUI();
    }

    public void AddIncome(float amount)
    {
        currencyCount += amount;
        UpdateUI();
    }

    public bool purchaseAction(int cost)
    {
        if (currencyCount >= cost)
        {
            currencyCount -= cost;
            UpdateUI();
            return true;
        }

        return false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    void UpdateUI()
    {
     currencyText.text =$"<sprite=5>{Mathf.RoundToInt(currencyCount).ToString()}";
    }
}