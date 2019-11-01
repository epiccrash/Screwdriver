using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public enum CustomerState
{
    Idle,
    ReadyToOrder,
    WalkingToSlot,
    WaitingForDrink
}

[RequireComponent(typeof(CustomerMovementController), typeof(NavMeshAgent))]
public class CustomerScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _drinkOrderText;

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
    private DrinkRecipe _currentDrinkOrder;

    private int _alchoholLevel;
    private float _timeUntilNextDrink;
    private CustomerState _state;

    private CustomerMovementController _movementController;

    private void Start()
    {
        _alchoholLevel = 0;

        _drinkOrderText.enabled = false;

        _movementController = GetComponent<CustomerMovementController>();

        // Setting the state this time is a special case, so we'll do it without ChangeState().
        RandomizeDrinkTimer();
        _state = CustomerState.Idle;
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
                ChangeState(CustomerState.ReadyToOrder);
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
        switch (newState)
        {
            case CustomerState.Idle:
                _drinkOrderText.enabled = false;
                RandomizeDrinkTimer();
                _movementController.StartRandomWanderBehavior();
                break;
            case CustomerState.WaitingForDrink:
                _drinkOrderText.text = _currentDrinkOrder.drinkName;
                _drinkOrderText.enabled = true;
                break;
            default:
                break;
        }

        _state = newState;
    }

    public void OnArrivedAtDest()
    {
        if (_state == CustomerState.WalkingToSlot)
        {
            if (_orderableDrinks.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, _orderableDrinks.Count);
                _currentDrinkOrder = _orderableDrinks[idx];

                ChangeState(CustomerState.WaitingForDrink);
            }
            else
            {
                // We can't order any drinks!
                ChangeState(CustomerState.Idle);
            }
        }
    }

    public void AssignSlot(CustomerSlot slot)
    {
        _currentSlot = slot;
        _currentSlot.SetOnDrinkServed(OnDrinkReceived);

        _movementController.MoveTo(slot.StandLocation, OnArrivedAtDest);
        ChangeState(CustomerState.WalkingToSlot);
    }

    public void OnDrinkReceived(GameObject drink)
    {
        // Test if we got the right drink.

        // Are we drunk?

        // Give tips if we want.

        // Go back to partying before our next drink.
        _currentSlot.Unlock();
        _currentSlot = null;
        ChangeState(CustomerState.Idle);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Water")
        {
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
