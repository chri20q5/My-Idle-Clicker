using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public float count;
    public float clickPower;

    public Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

    public string saveTime;
    public int saveVersion = 1;
}
