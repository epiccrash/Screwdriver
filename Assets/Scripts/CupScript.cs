﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CupScript : MonoBehaviour
{
    private List<GameObject> _solidIngredientsInCup;
    private IDictionary<IngredientType, int> _ingredientsInCup;
    private float _alcoholByVolume;

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
        else if (other.gameObject.layer == 4) // Water Layer
        {
            IngredientScript ingredient = other.gameObject.GetComponent<IngredientScript>();
            AddOrIncreaseIngredient(ingredient.IngredientType);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("IceCube"))
        {
            if (_solidIngredientsInCup.Remove(other.gameObject))
            {
                _ingredientsInCup[IngredientType.Ice]--;

                if (_ingredientsInCup[IngredientType.Ice] == 0)
                {
                    _ingredientsInCup.Remove(IngredientType.Ice);
                }

                BarManager.Instance.UpdateIngredientDisplays(IngredientType.Ice, this);
            }

            // print("Ice: " + _ingredientsInCup[IngredientType.Ice]);
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

        BarManager.Instance.UpdateIngredientDisplays(ingredient, this);

        int enumVal = (int)ingredient; // Calculating alcohol content of the drink
        if (enumVal < 100)
        {
            _alcoholByVolume += ((int)ingredient / 100.0f); // Value of the enum is the % alc of the drink
        }

        //print("Alcohol by Volume: " + _alcoholByVolume);
        //print(ingredient + ": " + _ingredientsInCup[ingredient]);
    }

    public List<IngredientType> GetIngredientList()
    {
        return new List<IngredientType>(_ingredientsInCup.Keys);
    }

    public float GetABV()
    {
        return _alcoholByVolume;
    }

    public float GetTotalAlcohol()
    {
        if (_ingredientsInCup != null && _ingredientsInCup.Count > 0)
        {
            float totalAlc = 0;

            foreach (IngredientType ingredient in _ingredientsInCup.Keys)
            {
                if ((int)ingredient < 100)
                {
                    totalAlc += _ingredientsInCup[ingredient];
                }
            }

            return totalAlc;
        }

        return 0;
    }

    public bool IsEmpty()
    {
        return _ingredientsInCup.Keys.Count == 0;
    }

    public float GetIngredientCorrectness(IngredientType type, int amount)
    {
        if (!_ingredientsInCup.ContainsKey(type))
        {
            return 0;
        }

        float diff = amount - Mathf.Abs(amount - _ingredientsInCup[type]);

        return Mathf.Max(0, diff / amount);
    }

    public int GetIngredientAmount(IngredientType type)
    {
        return _ingredientsInCup.ContainsKey(type) ? _ingredientsInCup[type] : 0;
    }

    public void AlertBarManager()
    {
        BarManager.Instance.OnCupPickedUp(this);
    }
}