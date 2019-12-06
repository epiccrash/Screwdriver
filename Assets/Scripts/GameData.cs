using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    public int totalDrinks;
    public int perfectDrinks;
    public int totalShots;
    public float totalTips;

    public void Reset()
    {
        totalDrinks = 0;
        perfectDrinks = 0;
        totalShots = 0;
        totalTips = 0;
    }
}
