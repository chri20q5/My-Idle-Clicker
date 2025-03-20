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


    private int level;
    private float incomeTimer;
    private float boostMultiplier;
    private float boostTimeRemaining;

    [Header("Managers")]
    public Clicker clicker;
    public PremiumCurrency premiumCurrency;

    public Generator1Upgrade boostUpgrade;

    private void Start()
    {
        incomeTimer = incomeInterval;
        intervalTimer.value = 0;
        UpdateUI();
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

        if (boostTimeRemaining > 0f)
        {
            boostTimeRemaining -= Time.deltaTime;
            if (boostTimeRemaining <= 0f)
            {
                boostMultiplier = 1f;
            }
        }

        bool canAfford = clicker.currencyCount >= CalculatePrice();
        buyButton.interactable = canAfford;
        UpdateUI();
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

    void GenerateIncome()
    {
        float growthMultiplier = GetIncomeGrowthMultiplier();
        float totalIncome = baseIncome * Mathf.Pow(growthMultiplier, level);

       totalIncome *= boostMultiplier;

       premiumCurrency.PremiumFromGenerator();
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

    public void ApplyBoost(float multiplier, float duration)
    {
        boostMultiplier = multiplier;
        boostTimeRemaining = duration;

        UpdateUI();
    }

    private string GenerateBasicIncomeText()
    {
        float currentIncome = CalculateIncomeForLevel(level);
        float nextIncome = CalculateIncomeForLevel(level + 1);

        if (level == 0)
        {
            return $"Income: {baseIncome:F1} every {incomeInterval:F1}/s ";
        }
        else
        {
            return $"Income: {currentIncome:F1} -> {nextIncome:F1} every {incomeInterval:F1}/s ";
        }
    }

    private string GenerateBoostedIncomeText()
    {
        float currentIncome = CalculateIncomeForLevel(level);
        float nextIncome = CalculateIncomeForLevel(level + 1);
        float boostedIncome = currentIncome * boostMultiplier;
        float nextBoostedIncome = nextIncome * boostMultiplier;

        if (level == 0)
        {
            return $"Income: {boostedIncome:F1} every {incomeInterval:F1}s";
        }
        else
        {
            return
                $"Income: {boostedIncome:F1} -> {nextBoostedIncome:F1} every {incomeInterval:F1}/s";
        }
    }

    private float CalculateIncomeForLevel(int targetLevel)
    {
        if (targetLevel == 0)
        {
            return baseIncome;
        }

        float growthMultipler = GetIncomeGrowthMultiplier();
        return baseIncome * Mathf.Pow(growthMultipler, targetLevel);
    }

    public void UpdateUI()
    {
        priceText.text = CalculatePrice().ToString();
        levelText.text = $"Level: {level}";

        bool isBoostActive = boostMultiplier > 1f && boostTimeRemaining > 0f;
        incomeInfoText.text = isBoostActive
            ? GenerateBoostedIncomeText()
            : GenerateBasicIncomeText();
    }


}
