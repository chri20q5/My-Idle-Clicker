using System;
using TMPro;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    [SerializeField] TMP_Text countText;
    [SerializeField] TMP_Text incomeText;
    [SerializeField] Upgrade[] upgrades;


    [HideInInspector] public float count = 0;
    [HideInInspector] public float clickPower = 1f;



    private void Start()
    {
        UpdateUI();
    }


    public void ActiveClicker()
    {
        count += clickPower;
        UpdateUI();
    }

    public void AddIncome(float amount)
    {
        count += amount;
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


    void UpdateUI()
    {
     countText.text = Mathf.RoundToInt(count).ToString();
    }
}