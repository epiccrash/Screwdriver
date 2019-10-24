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
    public const float DestTolerance = 0.1f;
    public const float WanderRadius = 3f;

    [SerializeField]
    private List<DrinkRecipe> _orderableDrinks;

    [SerializeField]
    private int _drunkThreshhold;

    [Header("Drink Order Timing")]

    [SerializeField]
    private float _minTimeBetweenDrinks;

    [SerializeField]
    private float _maxTimeBetweenDrinks;

    [Header("Wander Wait Timing")]

    [SerializeField]
    private float _minWanderPosWaitTime;

    [SerializeField]
    private float _maxWanderPosWaitTime;

    private CustomerSlot _currentSlot;

    private Transform _currentWanderLocation;
    private float _wanderPauseTimer;
    private bool _isWaitingAtWanderPos;

    private int _alchoholLevel;
    private float _timeUntilNextDrink;
    private CustomerState _state;
    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _alchoholLevel = 0;

        RandomizeDrinkTimer();
        RandomizeWanderWaitTimer();

        // We'll manually set state this first time, since we don't want to apply special changes.
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
                _state = CustomerState.ReadyToOrder;
                BarManager.Instance.EnterCustomerQueue(this);
            }
        }

        // Randomly wander around the room.
        if (_state == CustomerState.Idle || _state == CustomerState.ReadyToOrder)
        {
            // Have we reached our position?
            // If yes, lets count down our timer
            // Is our timer below 0?
            // Lets get a new position and restart the time
            // Have we reached our random wander position?
            if (_agent.remainingDistance <= DestTolerance)
            {
                _wanderPauseTimer -= Time.deltaTime;

                if (_wanderPauseTimer <= 0)
                {
                    RandomizeWanderWaitTimer();
                    _agent.destination = GetRandomWanderPosition();
                }
            }
        }

        // Check if we've arrived at a customer slot yet.
        if (_state == CustomerState.WalkingToSlot && _agent.remainingDistance <= DestTolerance)
        {
            ChangeState(CustomerState.WaitingForDrink);
        }
    }

    private void RandomizeDrinkTimer()
    {
        _timeUntilNextDrink = UnityEngine.Random.Range(_minTimeBetweenDrinks, _maxTimeBetweenDrinks);
    }

    private void RandomizeWanderWaitTimer()
    {
        _wanderPauseTimer = UnityEngine.Random.Range(_minWanderPosWaitTime, _maxWanderPosWaitTime);
    }

    private void ChangeState(CustomerState newState)
    {
        // If we're not transitioning between wander states, reset the timer.
        if (_state != CustomerState.Idle || newState != CustomerState.ReadyToOrder)
        {
            _wanderPauseTimer = 0;
        }

        _state = newState;
    }

    private Vector3 GetRandomWanderPosition()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * WanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, WanderRadius, -1);

        Vector3 newWanderPos = navHit.position;

        newWanderPos.x = Mathf.Max(newWanderPos.x, -3.5f);

        return newWanderPos;
    }

    public void AssignSlot(CustomerSlot slot)
    {
        _currentSlot = slot;
        ChangeState(CustomerState.WalkingToSlot);

        _agent.destination = slot.StandLocation.position;
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
