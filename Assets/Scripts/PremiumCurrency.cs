using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Random = Unity.Mathematics.Random;

public class PremiumCurrency : MonoBehaviour, IDataPersistence
{
    public TMP_Text premiumCurrencyText;
    public TMP_Text unlockPriceText;

    [Header("Settings")] [Range(1, 10000f)]
    public int clickPremiumChanceDenominator;

    [Range(1, 10000f)] public int generatorPremiumChanceDenominator;

    public int basePremiumAmount = 1;

    public Clicker clicker;

    [Header("Unlock Settings")]
    [HideInInspector] public bool premiumGenerationUnlocked = false;
    public int unlockCost;
    public Button unlockButton;
    public GameObject lockedIndicator;
    public string purchasedText = "Purchased";
    public TMP_Text buttonText;
    public GameObject premiumCurrencyCount;

    [Header("Feedback Effect")]
    public ParticleSystem premiumParticleEffect;
    public Color textHighlightColor = Color.yellow;

    private Color originalTextColor;
    //private bool animatingText = false;
    private float animationTime = 0f;
    private const float animationDuration = 0.5f;


    private void Start()
    {
        originalTextColor = premiumCurrencyText.color;
        PurchasedState();
        UpdateUI();
    }

    private void Update()
    {
        animationTime += Time.deltaTime;
        float t = animationTime / animationDuration;

        if (t >= 1.0f)
        {
            //animatingText = false;
            premiumCurrencyText.color = originalTextColor;
            premiumCurrencyText.transform.localScale = Vector3.one;
        }
        else
        {
            float pulse = 1.0f + 0.2f * Mathf.Sin(Mathf.PI * 4);
            premiumCurrencyText.transform.localScale = new Vector3(pulse, pulse, 1f);
            premiumCurrencyText.color = Color.Lerp(originalTextColor, textHighlightColor, t);
        }


        bool canAfford = clicker.premiumCurrencyCount >= unlockCost;
        unlockButton.interactable = canAfford;
    }


    public void PremiumFromClick()
    {
       if (!premiumGenerationUnlocked) return;

        int randomValue = UnityEngine.Random.Range(1, clickPremiumChanceDenominator + 1);

        if (randomValue == 1)
        {
            AddPremiumCurrency(basePremiumAmount);
            PlayPremiumEffects();
        }
    }

    public void PremiumFromGenerator()
    {
       if (!premiumGenerationUnlocked) return;

        int randomValue = UnityEngine.Random.Range(1, generatorPremiumChanceDenominator + 1);

        if (randomValue == 1)
        {
            AddPremiumCurrency(basePremiumAmount);
            PlayPremiumEffects();
        }
    }

    public void AddPremiumCurrency(int amount)
    {
        clicker.premiumCurrencyCount += amount;
        UpdateUI();
    }

    private void PlayPremiumEffects()
    {
        premiumParticleEffect.Play();
        //animatingText = true;
        animationTime = 0f;
    }

    public void ClickAction()
    {
        if (premiumGenerationUnlocked) return;
        int price = unlockCost;
        bool purchaseSuccesful = clicker.purchaseAction(price);

        if (purchaseSuccesful)
        {
            premiumGenerationUnlocked = true;
            UpdateUI();
            PurchasedState();
        }
    }

    private void PurchasedState()
    {
        if (premiumGenerationUnlocked)
        {
            unlockButton.interactable = false;
            lockedIndicator.SetActive(false);
            buttonText.text = purchasedText;
            premiumCurrencyText.text = purchasedText;
            premiumCurrencyCount.SetActive(true);
        }
        else
        {
            unlockButton.interactable = true;
            lockedIndicator.SetActive(true);
            buttonText.text = "Unlock";
            premiumCurrencyCount.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        premiumCurrencyText.text = clicker.premiumCurrencyCount.ToString();
        unlockPriceText.text = unlockCost.ToString();
    }

    public void LoadData(GameData data)
    {
        premiumGenerationUnlocked = data.premiumGenerationUnlocked;
        PurchasedState();
        UpdateUI();

    }

    public void SaveData(ref GameData data)
    {
        data.premiumGenerationUnlocked = premiumGenerationUnlocked;
    }
}
