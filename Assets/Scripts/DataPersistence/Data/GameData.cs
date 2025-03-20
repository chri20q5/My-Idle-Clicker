using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public float currencyCount;
    public SerializableDictionary<string, int> generatorLevels;
    public SerializableDictionary<string, float> generatorTimers;
    public int clickerUpgradesLevel;
    public int generatorBoostUpgradeLevel;

    public GameData()
    {
        this.currencyCount = 0;
        generatorLevels = new SerializableDictionary<string, int>();
        generatorTimers = new SerializableDictionary<string, float>();
        this.clickerUpgradesLevel = 0;
        this.generatorBoostUpgradeLevel = 0;
    }
}
