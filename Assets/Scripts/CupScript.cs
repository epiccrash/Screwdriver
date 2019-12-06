﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CupScript : MonoBehaviour
{
    [SerializeField]
    private ConeModify _cupConeModify;

    [SerializeField]
    private Transform _orangeWedgeSpot;

    [SerializeField]
    private Transform _lemonSliceSpot;

    [SerializeField]
    private Transform _limeSliceSpot;

    [SerializeField]
    private Transform _cherrySpot;

    private List<GameObject> _solidIngredientsInCup;
    private IDictionary<IngredientType, int> _ingredientsInCup;

    private float _alcoholByVolume;
    private int _totalDropsInCup;
    private bool _hasBeenShaken;

    private float _maxPossibleDrops;

    private bool _isTutorial;

    private void Start()
    {
        _isTutorial = false;
        _ingredientsInCup = new Dictionary<IngredientType, int>();
        _solidIngredientsInCup = new List<GameObject>();

        _totalDropsInCup = 0;
        _maxPossibleDrops = GameConstants.RecipeToCupConversion * GameConstants.RecipeDropsToACup;

        GameManager.Instance.OnTutorialStart.AddListener(OnTutorialStart);
        GameManager.Instance.OnGameStart.AddListener(OnGameStart);
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
                other.gameObject.transform.SetParent(transform);
            }
        }
        else if (other.gameObject.layer == 4) // Water Layer
        {
            IngredientScript ingredient = other.gameObject.GetComponent<IngredientScript>();

            // If this is a liquid, let's increase the cup fill.
            if (_cupConeModify != null && (other.transform.CompareTag("Alcohol") || other.transform.CompareTag("Water")))
            {
                _cupConeModify.ChangeFill(other.gameObject);
            }

            if (_totalDropsInCup < _maxPossibleDrops)
            {
                _totalDropsInCup++;
                AddOrIncreaseIngredient(ingredient.IngredientType);
            }
        }
        else
        {
            IngredientScript ingredient = other.gameObject.GetComponent<IngredientScript>();

            if (ingredient != null && !_solidIngredientsInCup.Contains(other.gameObject))
            {
                switch (ingredient.IngredientType)
                {
                    case (IngredientType.OrangeWedge):
                        _solidIngredientsInCup.Add(other.gameObject);
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        other.gameObject.transform.position = _orangeWedgeSpot.position;
                        other.gameObject.transform.rotation = _orangeWedgeSpot.rotation;
                        AddOrIncreaseIngredient(IngredientType.OrangeWedge);
                        break;
                    case IngredientType.LemonSlice:
                        _solidIngredientsInCup.Add(other.gameObject);
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        other.gameObject.transform.position = _lemonSliceSpot.position;
                        other.gameObject.transform.rotation = _lemonSliceSpot.rotation;
                        AddOrIncreaseIngredient(IngredientType.LemonSlice);
                        break;
                    case IngredientType.LimeSlice:
                        _solidIngredientsInCup.Add(other.gameObject);
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        other.gameObject.transform.position = _limeSliceSpot.position;
                        other.gameObject.transform.rotation = _limeSliceSpot.rotation;
                        AddOrIncreaseIngredient(IngredientType.LimeSlice);
                        break;
                    case IngredientType.Cherry:
                        _solidIngredientsInCup.Add(other.gameObject);
                        other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        other.gameObject.transform.position = _cherrySpot.position;
                        other.gameObject.transform.rotation = _cherrySpot.rotation;
                        AddOrIncreaseIngredient(IngredientType.Cherry);
                        break;
                    default:
                        break;
                }

            }
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

            other.gameObject.transform.SetParent(null);

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

        if (_isTutorial)
        {
            switch (ingredient)
            {
                case IngredientType.Ice:
                    GameManager.Instance.OnIceAdded();
                    break;
                case IngredientType.OrangeJuice:
                    GameManager.Instance.OnOjAdded();
                    break;
                case IngredientType.Vodka:
                    GameManager.Instance.OnVodkaAdded();
                    break;
                case IngredientType.OrangeWedge:
                    GameManager.Instance.OnWedgeAdded();
                    break;
                default:
                    break;
            }
        }
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

    public void shakeIt()
    {
        _hasBeenShaken = true;
    }

    private void OnTutorialStart()
    {
        _isTutorial = true;
    }

    private void OnGameStart()
    {
        _isTutorial = false;
    }
}