using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    private IDictionary<IngredientType, int> _itemsInCup;

    private void Start()
    {
        _itemsInCup = new Dictionary<IngredientType, int>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IceCube"))
        {
            AddOrIncreaseIngredient(IngredientType.Ice);
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            IngredientScript ingredient = other.gameObject.GetComponent<IngredientScript>();
            AddOrIncreaseIngredient(ingredient.IngredientType);
        }
    }

    private void AddOrIncreaseIngredient(IngredientType ingredient)
    {
        if (_itemsInCup.ContainsKey(ingredient))
        {
            _itemsInCup[ingredient]++;
        }
        else
        {
            _itemsInCup.Add(ingredient, 1);
        }

        print(ingredient + ": " + _itemsInCup[ingredient]);
    }
}
