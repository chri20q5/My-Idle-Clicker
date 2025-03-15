using System;
using TMPro;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text incomeText;
    [SerializeField] Upgrade[] upgrades;


    private float count = 0;
    private float nextTimeCheck = 1;
    private float lastIncomeValue = 0;


    private void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (nextTimeCheck < Time.timeSinceLevelLoad)
        {
            IdleCalculate();
            nextTimeCheck = Time.timeSinceLevelLoad + 1f;
        }
    }


    public void ActiveClicker()
    {
        count++;
        UpdateUI();
    }



    public bool purchaseAction(int cost)
    {
        if (count >= cost)
        {
            count -= cost;
            UpdateUI();
            return true;
        }

        return false;
    }

   public void IdleCalculate()
   {
       float sum = 0;
        foreach (var upgrade in upgrades)
        {
            sum += upgrade.CalculateIncomePerSecond();
        }
        lastIncomeValue = sum;
        count += sum;
        UpdateUI();
    }

    void UpdateUI()
    {
        countText.text = Mathf.RoundToInt(count).ToString();

        float totalIncomePerSecond = 0;
        foreach (var upgrade in upgrades)
        {
            totalIncomePerSecond += upgrade.CalculateIncomePerSecond();
        }

        incomeText.text = totalIncomePerSecond.ToString("F2") + "/s";
    }
}