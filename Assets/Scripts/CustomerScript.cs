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
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _alchoholLevel = 0;
        _timeUntilNextDrink = UnityEngine.Random.Range(_minTimeBetweenDrinks, _maxTimeBetweenDrinks);

        Debug.Log(_timeUntilNextDrink);
        _state = CustomerState.Idle;
    }

    private void Update()
    {
        _timeUntilNextDrink -= Time.deltaTime;

        // If it's time to order, get in the queue for a slot.
        if (_state == CustomerState.Idle && _timeUntilNextDrink <= 0f)
        {
            _state = CustomerState.ReadyToOrder;
            BarManager.Instance.EnterCustomerQueue(this);
        }
        else if (_state == CustomerState.WalkingToSlot && _agent.remainingDistance <= 0.1f)
        {
            _state = CustomerState.WaitingForDrink;
        }
    }

    public void AssignSlot(CustomerSlot slot)
    {
        _currentSlot = slot;
        _state = CustomerState.WalkingToSlot;

        _agent.destination = slot.StandLocation.position;
    }

    public void OnDrinkReceived()
    {
        // Test if we got the right drink.

        // Give tips if we want/

        _timeUntilNextDrink = UnityEngine.Random.Range(_minTimeBetweenDrinks, _maxTimeBetweenDrinks);
        _state = CustomerState.Idle;
    }
}
