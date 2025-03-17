using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerUpgrades : MonoBehaviour
{
    [Header("Components")]
    public TMP_Text priceText;
    public TMP_Text levelText;
    public Button buyButton;

    [Header("Price")]
    public float startPrice = 25f;
    public float priceMultiplier = 1.8f;

    [Header("Power")]
    public float powerBase = 1.3f;
    public float baseClickPower = 1f;


    [Header("Managers")]
    public Clicker clicker;

    public int level = 0;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
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
            UpdateClickPower();
            UpdateUI();
        }
    }

    public void UpdateClickPower()
    {
        clicker.clickPower = baseClickPower * Mathf.Pow(powerBase, level);
    }

    private int CalculatePrice()
    {
        return Mathf.RoundToInt(startPrice * Mathf.Pow(priceMultiplier, level));
    }

    public void UpdateUI()
    {
        priceText.text = CalculatePrice().ToString();
        levelText.text = $"Level: {level}";
    }
}