using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    List<IngredientUnit> _itemsInCup;

    private void Start()
    {
        _itemsInCup = new List<IngredientUnit>();
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
