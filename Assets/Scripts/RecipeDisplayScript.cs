using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecipeDisplayScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _drinkNameDisplay;

    [SerializeField]
    private GameObject _ingredientRingParent;

    [SerializeField]
    private GameObject _radialProgressBarPrefab;

    [SerializeField]
    private TextMeshProUGUI _needsShakerDisplay;

    [SerializeField]
    private Color _highlightColor;

    [SerializeField]
    private Color32 IncorrectAmtColor = new Color32(235, 158, 52, 255);

    [SerializeField]
    private Color32 CorrectAmtColor = new Color32(28, 184, 28, 255);

    private Dictionary<IngredientType, RadialProgressBar> _ingredientRings;
    private Color _originalTextColor;
    private DrinkRecipe _currentDrink;

    private void Awake()
    {
        _originalTextColor = _drinkNameDisplay.color;
        _drinkNameDisplay.enabled = false;
        _needsShakerDisplay.enabled = false;
        _ingredientRings = new Dictionary<IngredientType, RadialProgressBar>();
    }

    private void DestroyIngredientRings()
    {
        if (_ingredientRings != null && _ingredientRings.Count > 0)
        {
            foreach (RadialProgressBar child in _ingredientRings.Values)
            {
                Destroy(child.gameObject);
            }

            _ingredientRings.Clear();
        }
    }

    public void InitializeForNewDrink(DrinkRecipe newDrink)
    {
        _currentDrink = newDrink;
        _drinkNameDisplay.text = newDrink.drinkName;
        _drinkNameDisplay.enabled = true;
        UnhighlightDrinkName();

        if (_currentDrink.needsShaker)
        {
            _needsShakerDisplay.color = IncorrectAmtColor;
            _needsShakerDisplay.enabled = true;
        }

        foreach (IngredientType ingredient in newDrink.GetIngredientList())
        {
            IngredientUnit ingredientUnit = newDrink.GetUnitFromIngredientType(ingredient);

            GameObject newIngredientRing = Instantiate(_radialProgressBarPrefab, _ingredientRingParent.transform) as GameObject;

            RadialProgressBar radialBarScript = newIngredientRing.GetComponent<RadialProgressBar>();

            _ingredientRings[ingredientUnit.ingredient] = radialBarScript;

            radialBarScript.InitializeForNewIngredient(ingredientUnit.ingredient);
        }
    }

    public void WipeDisplay()
    {
        _drinkNameDisplay.enabled = false;
        DestroyIngredientRings();
    }

    public void UpdateIngredientRing(IngredientType ingredient)
    {
        if (_currentDrink != null)
        {
            IngredientUnit recipeUnit = _currentDrink.GetUnitFromIngredientType(ingredient);

            if (recipeUnit != null && _ingredientRings.ContainsKey(ingredient))
            {
                if (recipeUnit.amountMatters)
                {
                    float correctness = BarManager.Instance.GetCurrentCupIngredientCorrectness(ingredient, recipeUnit.amount);

                    _ingredientRings[ingredient].SetIngredientAmount(correctness);
                }
                else if (BarManager.Instance.GetCurrentCupIngredientAmount(ingredient) > 0)
                {
                    // Amount doesn't matter, so we're 100% correct!
                    _ingredientRings[ingredient].SetIngredientAmount(1);
                }
                else
                {
                    _ingredientRings[ingredient].SetIngredientAmount(0);
                }
            }
        }
    }

    public void ResetIngredientDisplayForNewCup()
    {
        foreach (IngredientType ingredient in _ingredientRings.Keys)
        {
            UpdateIngredientRing(ingredient);
        }

        if (BarManager.Instance.GetCupIsShaken())
        {
            _needsShakerDisplay.color = CorrectAmtColor;
        }
        else
        {
            _needsShakerDisplay.color = IncorrectAmtColor;
        }
    }

    public void HighlightDrinkName()
    {
        _drinkNameDisplay.color = _highlightColor;
    }

    public void UnhighlightDrinkName()
    {
        _drinkNameDisplay.color = _originalTextColor;
    }

    public void UpdateShakerDisplay()
    {
        if (BarManager.Instance.GetCupIsShaken())
        {
            _needsShakerDisplay.color = CorrectAmtColor;
        }
        else
        {
            _needsShakerDisplay.color = IncorrectAmtColor;
        }
    }
}
