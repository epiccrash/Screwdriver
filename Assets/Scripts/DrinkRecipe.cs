using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientUnit
{
    public IngredientType ingredient;
    public bool amountMatters;

    [SerializeField]
    public int amount;

    public string amountDescription;
}

[CreateAssetMenu]
public class DrinkRecipe : ScriptableObject
{
    private const int MinimumAmountForConversion = 10;

    [Header("Basic Info")]
    public string drinkName;
    public int alcoholContent;

    [Header("Special Steps")]
    public bool needsShaker;

    [Header("Recipe")]
    public List<IngredientUnit> recipe;

    private List<IngredientType> _ingredientList;

    public IngredientUnit GetUnitFromIngredientType(IngredientType type)
    {
        foreach (IngredientUnit unit in recipe)
        {
            if (unit.ingredient == type)
            {
                if (unit.amount >= MinimumAmountForConversion)
                {
                    IngredientUnit adjustedUnit = new IngredientUnit
                    {
                        ingredient = unit.ingredient,
                        amountMatters = unit.amountMatters,
                        amountDescription = unit.amountDescription,
                        amount = Mathf.RoundToInt(unit.amount * GameConstants.RecipeToCupConversion)
                    };

                    return adjustedUnit;
                }

                return unit;
            }
        }

        return null;
    }

    public List<IngredientType> GetIngredientList()
    {
        if (_ingredientList.Count == 0)
        {
            _ingredientList = new List<IngredientType>();

            foreach (IngredientUnit unit in recipe)
            {
                _ingredientList.Add(unit.ingredient);
            }
        }

        return _ingredientList;
    }
}