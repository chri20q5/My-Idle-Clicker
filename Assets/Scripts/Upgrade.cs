using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Upgrade : MonoBehaviour
{
    [Header("Components")]
    public TMP_Text priceText;
    public TMP_Text incomeInfoText;

    [Header("Generator Values")]
    public int startPrice = 15;
    public float upgradePriceMultiplier;
    public float currencyPerUpgrade = 0.2f;
    public float baseIncome;
    public float incomeInterval;

    int level = 0;

    [Header("Managers")]
    public Clicker clicker;


    public void ClickAction()
    {
        int price = CalculatePrice();
        bool purchaseSuccesful = clicker.purchaseAction(price);
        if (purchaseSuccesful)
        {
            level++;
            updateUI();
        }
    }

    private void Start()
    {
        updateUI();
    }

    void updateUI()
    {
        if (level == 0)
        {
            incomeInfoText.text = "Income: " + baseIncome + "/" + incomeInterval + "s";
        }
        else
        {
            incomeInfoText.text = "Income: " + baseIncome + "+ (" + (currencyPerUpgrade * level) + "/" + incomeInterval + "s)";
        }
        priceText.text = CalculatePrice().ToString();
    }

    int CalculatePrice()
    {
        int price = Mathf.RoundToInt(startPrice * Mathf.Pow(upgradePriceMultiplier, level));
        return price;

    }

    public float CalculateIncomePerSecond()
    {
        if (level == 0)
            return 0;
        return (baseIncome + (currencyPerUpgrade * level)) / incomeInterval;
    }

}
