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

    private void Start()
    {
        _drinkNameDisplay.enabled = false;

        DestroyIngredientRings();
    }

    private void DestroyIngredientRings()
    {
        foreach (Transform child in _ingredientRingParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
