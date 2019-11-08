using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    private List<GameObject> _solidIngredientsInCup;
    private IDictionary<IngredientType, int> _ingredientsInCup;

    private bool _hasBeenShaken;

    private void Start()
    {
        _ingredientsInCup = new Dictionary<IngredientType, int>();
        _solidIngredientsInCup = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("IceCube"))
        {
            // We need to keep track of specific ice cubes, so we don't double count.
            if (!_solidIngredientsInCup.Contains(other.gameObject))
            {
                AddOrIncreaseIngredient(IngredientType.Ice);
                _solidIngredientsInCup.Add(other.gameObject);
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
            _solidIngredientsInCup.Remove(other.gameObject);
            _ingredientsInCup[IngredientType.Ice]--;

            print("Ice: " + _ingredientsInCup[IngredientType.Ice]);
        }
    }

    private void AddOrIncreaseIngredient(IngredientType ingredient)
    {
        if (_ingredientsInCup.ContainsKey(ingredient))
        {
            _ingredientsInCup[ingredient]++;
        }
        else
        {
            _ingredientsInCup.Add(ingredient, 1);
        }

        print(ingredient + ": " + _ingredientsInCup[ingredient]);
    }

    public List<IngredientType> GetIngredientList()
    {
        return new List<IngredientType>(_ingredientsInCup.Keys);
    }

    public float GetIngredientCorrectness(IngredientType type, int amount)
    {
        if (!_ingredientsInCup.ContainsKey(type))
        {
            return 0;
        }

        float diff = amount - Mathf.Abs(amount - _ingredientsInCup[type]);

        return diff / amount;
    }
}
