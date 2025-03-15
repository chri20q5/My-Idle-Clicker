using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

public class Upgrade : MonoBehaviour
{
    [Header("Components")]
    public TMP_Text priceText;
    public TMP_Text incomeInfoText;
    public Slider intervalTimer;
    public TMP_Text timerText;
    public Button buyButton;

    [Header("Generator Values")]
    public int startPrice = 15;
    public float upgradePriceMultiplier;
    public float currencyPerUpgrade = 0.2f;
    public float baseIncome;
    public float incomeInterval;

    int level = 0;
    private float incomeTimer;

    [Header("Managers")]
    public Clicker clicker;

    private void Start()
    {
        incomeTimer = incomeInterval;
        intervalTimer.value = 0;
        updateUI();
    }

    private void Update()
    {
        if (level > 0)
        {
            incomeTimer -= Time.deltaTime;

            intervalTimer.value = 1f - (incomeTimer / incomeInterval);
            timerText.text = incomeTimer.ToString("f1");

            if (incomeTimer <= 0f)
            {
                GenerateIncome();
                incomeTimer = incomeInterval;
                intervalTimer.value = 0f;

            }
        }
        else
        {
            intervalTimer.value = 0f;
            timerText.text = incomeInterval.ToString("f1");
        }
        bool canAfford = clicker.count >= CalculatePrice();
        buyButton.interactable = canAfford;
    }
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

    void GenerateIncome()
    {
        float totalIncome = baseIncome;
        if (level > 1)
        {
            totalIncome += currencyPerUpgrade * (level - 1);
        }
        clicker.AddIncome(totalIncome);
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

    public void updateUI()
    {
    priceText.text = CalculatePrice().ToString();

    if (level == 0)
    {
        incomeInfoText.text = "Income: " + baseIncome + " every " + incomeInterval + "s";
    }
    else if (level == 1)
    {
        incomeInfoText.text = "Income: " + baseIncome + " every " + incomeInterval + "s";
    }
    else
    {
        float totalIncome = baseIncome + (currencyPerUpgrade * (level - 1));
        incomeInfoText.text = "Income: " + totalIncome + " every " + incomeInterval + "s";
    }
    }



}
