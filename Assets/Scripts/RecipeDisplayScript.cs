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
    private Color _highlightColor;

    private Dictionary<IngredientType, RadialProgressBar> _ingredientRings;
    private Color _originalTextColor;

    private void Start()
    {
        _originalTextColor = _drinkNameDisplay.color;
        _drinkNameDisplay.enabled = false;
        _ingredientRings = new Dictionary<IngredientType, RadialProgressBar>();
    }

    private void DestroyIngredientRings()
    {
        foreach (RadialProgressBar child in _ingredientRings.Values)
        {
            Destroy(child.gameObject);
        }

        _ingredientRings.Clear();
    }

    public void InitializeForNewDrink(DrinkRecipe newDrink)
    {
        _drinkNameDisplay.text = newDrink.drinkName;
        _drinkNameDisplay.enabled = true;

        foreach (IngredientUnit ingredientUnit in newDrink.recipe)
        {
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

    public void UpdateIngredientRing(IngredientType ingredient, float newAmt)
    {
        if (_ingredientRings.ContainsKey(ingredient))
        {
            _ingredientRings[ingredient].SetIngredientAmount(newAmt);
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
}
