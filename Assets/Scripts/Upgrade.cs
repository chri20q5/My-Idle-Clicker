using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Slider = UnityEngine.UI.Slider;

public class GeneratorUpgrades : MonoBehaviour, IDataPersistence
{
    [Header("Components")]
    public TMP_Text priceText;
    public TMP_Text incomeInfoText;
    public Slider intervalTimer;
    public TMP_Text levelText;
    public TMP_Text timerText;
    public Button buyButton;
    public string generatorId;


    [Header("Generator Values")]
    public int startPrice = 15;
    public float baseIncome;
    public float incomeInterval;

    [Header("Growth Rates - Income")]
    [Tooltip("Growth rate for levels 0-4")]
    public float earlyGameIncomeGrowth = 1.10f;
    [Tooltip("Growth rate for levels 5-14")]
    public float midGameIncomeGrowth = 1.15f;
    [Tooltip("Growth rate for levels 15+")]
    public float lateGameIncomeGrowth = 1.25f;

    [Header("Growth Rates - Price")]
    [Tooltip("Growth rate for levels 0-4")]
    public float earlyGamePriceGrowth = 1.10f;
    [Tooltip("Growth rate for levels 5-14")]
    public float midGamePriceGrowth = 1.15f;
    [Tooltip("Growth rate for levels 15+")]
    public float lateGamePriceGrowth = 1.25f;

    [ContextMenu("Generate ID")]
    private void GenerateGuid()
    {
        generatorId = Guid.NewGuid().ToString();
    }



    int level = 0;
    private float incomeTimer;

    [Header("Managers")]
    public Clicker clicker;

    public Generator1Upgrade boostUpgrade;

    private void Start()
    {
        incomeTimer = incomeInterval;
        intervalTimer.value = 0;
        updateUI();
    }

    public void LoadData(GameData data)
    {
        data.generatorLevels.TryGetValue(generatorId, out level);
        data.generatorTimers.TryGetValue(generatorId, out incomeTimer);
    }

    public void SaveData(ref GameData data)
    {
        data.generatorLevels[generatorId] = this.level;
        data.generatorTimers[generatorId] = this.incomeTimer;
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
            updateUI();
        }
    }

    void GenerateIncome()
    {
        float growthMultiplier = GetIncomeGrowthMultiplier();
        float totalIncome = baseIncome * Mathf.Pow(growthMultiplier, level);

        if (boostUpgrade != null)
        {
            totalIncome *= boostUpgrade.GetCurrentBoostMultiplier();
        }

        clicker.AddIncome(totalIncome);
    }

    int CalculatePrice()
    {
        float growthMultiplier = GetPriceGrowthMultiplier();
        int price = Mathf.RoundToInt(startPrice * Mathf.Pow(growthMultiplier, level));
        return price;

    }

    private float GetIncomeGrowthMultiplier()
    {
        if (level < 5)
        {
            return earlyGameIncomeGrowth;
        }
        else if (level < 15)
        {
            return midGameIncomeGrowth;
        }
        else
        {
            return lateGameIncomeGrowth;
        }
    }

    private float GetPriceGrowthMultiplier()
    {
        if (level < 5)
        {
            return earlyGamePriceGrowth;
        }
        else if (level < 15)
        {
            return midGamePriceGrowth;
        }
        else
        {
            return lateGamePriceGrowth;
        }
    }

    public void updateUI()
    {
    priceText.text = CalculatePrice().ToString();
    levelText.text = $"Level: {level}";

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
        float incomeGrowth = GetIncomeGrowthMultiplier();
        float totalIncome = baseIncome * Mathf.Pow(incomeGrowth, level);
        float nextLevelIncome = baseIncome * Mathf.Pow(incomeGrowth, level + 1);
        incomeInfoText.text = $"Income: {totalIncome:f1} -> {nextLevelIncome:f1} every {incomeInterval:f1}s";
    }
    }



}
