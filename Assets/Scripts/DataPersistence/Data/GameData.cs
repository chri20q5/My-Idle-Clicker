using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public float currencyCount;
    public SerializableDictionary<string, int> generatorLevels;
    public SerializableDictionary<string, float> generatorTimers;
    public SerializableDictionary<string, int> clickerUpgradesLevel;
    public SerializableDictionary<string, int> generatorBoostUpgradeLevel;

    public GameData()
    {
        this.currencyCount = 0;
        generatorLevels = new SerializableDictionary<string, int>();
        generatorTimers = new SerializableDictionary<string, float>();
        clickerUpgradesLevel = new SerializableDictionary<string, int>();
        generatorBoostUpgradeLevel = new SerializableDictionary<string, int>();
    }
}
