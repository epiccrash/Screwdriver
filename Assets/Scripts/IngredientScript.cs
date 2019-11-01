using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientScript : MonoBehaviour
{
    [SerializeField]
    private IngredientType _ingredientType;

    public IngredientType IngredientType { get { return _ingredientType; } }

}