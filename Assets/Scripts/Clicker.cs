using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Clicker : MonoBehaviour, IDataPersistence
{
    [SerializeField] TMP_Text currencyText;
    [SerializeField] GeneratorUpgrades[] generatorUpgrades;

    [HideInInspector] public float currencyCount = 0;
    [HideInInspector] public float clickPower = 1f;



    private void Start()
    {
        UpdateUI();
    }

    public void LoadData(GameData data)
    {
        this.currencyCount = data.currencyCount;
    }

    public void SaveData(ref GameData data)
    {
        data.currencyCount = this.currencyCount;
    }


    public void ActiveClicker()
    {
        currencyCount += clickPower;

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
     currencyText.text = Mathf.RoundToInt(currencyCount).ToString();
    }
}