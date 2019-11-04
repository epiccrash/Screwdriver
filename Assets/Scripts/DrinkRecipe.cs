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
    public int alcoholContent;

    [Header("Special Steps")]
    public bool needsShaker;

    [Header("Recipe")]
    public List<IngredientUnit> recipe;

    public IngredientUnit GetUnitFromIngredientType(IngredientType type)
    {
        foreach (IngredientUnit unit in this.recipe)
        {
            if (unit.ingredient == type)
            {
                return unit;
            }
        }

        return null;
    }
}