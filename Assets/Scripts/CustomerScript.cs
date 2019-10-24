using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Idle,
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

    [Header("Drink Order Timing")]

    [SerializeField]
    private float _minTimeBetweenDrinks;

    [SerializeField]
    private float _maxTimeBetweenDrinks;

    private CustomerSlot _currentSlot;

    private int _alchoholLevel;
    private float _timeUntilNextDrink;
    private CustomerState _state;

    private CustomerMovementController _movementController;

    private void Start()
    {
        _alchoholLevel = 0;
        RandomizeDrinkTimer();

        _movementController = GetComponent<CustomerMovementController>();

        // We'll manually set state this first time, since we don't want to apply special changes.
        ChangeState(CustomerState.Idle);
    }

    private void Update()
    {
        // Countdown drink timer.
        if (_state == CustomerState.Idle)
        {
            _timeUntilNextDrink -= Time.deltaTime;

            // If it's time to order, get in the queue for a slot.
            if (_timeUntilNextDrink <= 0)
            {
                _state = CustomerState.ReadyToOrder;
                BarManager.Instance.EnterCustomerQueue(this);
            }
        }
    }

    private void RandomizeDrinkTimer()
    {
        _timeUntilNextDrink = UnityEngine.Random.Range(_minTimeBetweenDrinks, _maxTimeBetweenDrinks);
    }

    private void ChangeState(CustomerState newState)
    {
        // If we're transitioning to idle, we should start wandering again.
        if (newState == CustomerState.Idle)
        {
            _movementController.StartRandomWanderBehavior();
        }

        _state = newState;
    }

    public void OnArrivedAtDest()
    {
        if (_state == CustomerState.WalkingToSlot)
        {
            ChangeState(CustomerState.WaitingForDrink);
        }
    }

    public void AssignSlot(CustomerSlot slot)
    {
        _currentSlot = slot;
        ChangeState(CustomerState.WalkingToSlot);

        _movementController.MoveTo(slot.StandLocation, OnArrivedAtDest);
    }

    public void OnDrinkReceived()
    {
        // Test if we got the right drink.

        // Are we drunk?

        // Give tips if we want.

        // Go back to partying before our next drink.
        RandomizeDrinkTimer();
        ChangeState(CustomerState.Idle);
    }
}
