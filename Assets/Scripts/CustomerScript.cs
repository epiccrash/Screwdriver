using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CustomerState
{
    Partying,
    ReadyToOrder,
    WalkingToSlot,
    WaitingForDrink
}

public class CustomerScript : MonoBehaviour
{
    [SerializeField]
    private List<DrinkRecipe> _orderableDrinks;

    [SerializeField]
    private int _drunkThreshhold;

    private int _alchoholLevel;

    private CustomerState _state;

    private void Start()
    {
        _alchoholLevel = 0;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
