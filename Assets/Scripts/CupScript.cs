using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    private List<GameObject> _iceCubesInCup;
    private IDictionary<IngredientType, int> _itemsInCup;

    private void Start()
    {
        _itemsInCup = new Dictionary<IngredientType, int>();
        _iceCubesInCup = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IceCube"))
        {
            // We need to keep track of specific ice cubes, so we don't double count.
            if (!_iceCubesInCup.Contains(other.gameObject))
            {
                AddOrIncreaseIngredient(IngredientType.Ice);
                _iceCubesInCup.Add(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Water"))
        {
            IngredientScript ingredient = other.gameObject.GetComponent<IngredientScript>();
            AddOrIncreaseIngredient(ingredient.IngredientType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IceCube"))
        {
            _iceCubesInCup.Remove(other.gameObject);
            _itemsInCup[IngredientType.Ice]--;
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
