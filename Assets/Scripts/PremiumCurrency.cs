using TMPro;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class PremiumCurrency : MonoBehaviour, IDataPersistence
{
    public TMP_Text premiumCurrencyText;

    [Header("Settings")]
    [Range(1, 10000f)]
    public int clickPremiumChanceDenominator;

    [Range(1, 10000f)]
    public int generatorPremiumChanceDenominator;

    public int basePremiumAmount = 1;

    public Clicker clicker;

    [Header("Feedback Effect")]
    public ParticleSystem premiumParticleEffect;
    public Color textHighlightColor = Color.yellow;

    private Color originalTextColor;
    private bool animatingText = false;
    private float animationTime = 0f;
    private const float animationDuration = 0.5f;


    private void Start()
    {
        UpdateUI();
        originalTextColor = premiumCurrencyText.color;
    }

    private void Update()
    {
        animationTime += Time.deltaTime;
        float t = animationTime / animationDuration;

        if (t >= 1.0f)
        {
            animatingText = false;
            premiumCurrencyText.color = originalTextColor;
            premiumCurrencyText.transform.localScale = Vector3.one;
        }
        else
        {
            float pulse = 1.0f + 0.2f * Mathf.Sin(Mathf.PI * 4);
            premiumCurrencyText.transform.localScale = new Vector3(pulse, pulse, 1f);
            premiumCurrencyText.color = Color.Lerp(originalTextColor, textHighlightColor, t);
        }
    }

    public void PremiumFromClick()
    {
        int randomValue = UnityEngine.Random.Range(1, clickPremiumChanceDenominator + 1);

        if (randomValue == 1)
        {
            AddPremiumCurrency(basePremiumAmount);
            PlayPremiumEffects();
        }
    }

    public void PremiumFromGenerator()
    {
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
        animatingText = true;
        animationTime = 0f;
    }

    public void UpdateUI()
    {
        premiumCurrencyText.text = clicker.premiumCurrencyCount.ToString();
    }

    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {

    }
}
