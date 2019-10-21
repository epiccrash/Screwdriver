using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientUnit
{
    public IngredientType ingredient;
    public bool amountMatters;
    public int amount;
}

[CreateAssetMenu]
public class DrinkRecipe : ScriptableObject
{
    [Header("Basic Info")]
    public string drinkName;

    [Header("Special Steps")]
    public bool needsShaker;

    [Header("Recipe")]
    public List<IngredientUnit> recipe;
}