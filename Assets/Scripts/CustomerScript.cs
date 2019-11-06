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
    private const float DrinkPerfectionPercentageEpsilon = 0.05f;

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

    [Header("Tipping")]

    [SerializeField]
    private float _tipPerCorrectIngredient;

    [SerializeField]
    private float _tipBonusOnPerfect;

    [SerializeField]
    private float _tipPenaltyPerWrongIngredient;

    private CustomerSlot _currentSlot;
    private DrinkRecipe _currentDrinkOrder;

    private int _alchoholLevel;
    private float _timeUntilNextDrink;
    private CustomerState _state;

    private CustomerMovementController _movementController;
    private NavMeshAgent _agent;
    private Rigidbody _rigidBody;
    private BoxCollider _collider;

    private void Start()
    {
        _alchoholLevel = 0;

        _drinkOrderText.enabled = false;

        _movementController = GetComponent<CustomerMovementController>();
        _agent = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();

        // Setting the state this time is a special case, so we'll do it without ChangeState().
        //RandomizeDrinkTimer();
        //_state = CustomerState.Idle;
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
                _agent.enabled = true;
                _rigidBody.isKinematic = true;
                _drinkOrderText.enabled = false;
                RandomizeDrinkTimer();
                _movementController.StartRandomWanderBehavior();
                break;
            case CustomerState.WaitingForDrink:
                _agent.enabled = false;
                _rigidBody.isKinematic = false;
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

    public void OnDrinkReceived(GameObject drinkObj)
    {
        CupScript drink = drinkObj.GetComponentInChildren<CupScript>();

        Dictionary<IngredientType, float> ingredientsAndCorrectness = new Dictionary<IngredientType, float>();

        // Add all ingredients to the dictionary.
        foreach (IngredientUnit unit in _currentDrinkOrder.recipe)
        {
            ingredientsAndCorrectness.Add(unit.ingredient, 0);
        }

        // Lets check everything in the cup and test how close it is to the drink we ordered.
        float tip = 0;

        foreach (IngredientType ingredientType in drink.GetIngredientList())
        {
            IngredientUnit unit = _currentDrinkOrder.GetUnitFromIngredientType(ingredientType);

            // Check if the ingredient is in the recipe.
            if (unit != null)
            {
                // Check amounts, add to the tip
                if (unit.amountMatters)
                {
                    ingredientsAndCorrectness[ingredientType] = drink.GetIngredientCorrectness(ingredientType, unit.amount);
                }
                else
                {
                    ingredientsAndCorrectness[ingredientType] = 1;
                }
            }
            else
            {
                // Subtract some amount from the tip
                tip -= _tipPenaltyPerWrongIngredient;
            }
        }

        // Now lets calculate the tip.
        bool isPerfectDrink = true;

        foreach (IngredientType ingredient in ingredientsAndCorrectness.Keys)
        {
            if (Mathf.Abs(ingredientsAndCorrectness[ingredient] - 1) > DrinkPerfectionPercentageEpsilon)
            {
                tip += ingredientsAndCorrectness[ingredient] * _tipPerCorrectIngredient;
                isPerfectDrink = false;
            }
            else
            {
                tip += _tipPerCorrectIngredient;
            }
        }

        if (isPerfectDrink)
        {
            tip += _tipBonusOnPerfect;
        }

        tip = Mathf.Max(0, tip);
        print("Tip: " + tip);

        // Use the tip jar to add the tip.

        // Are we drunk?
        _drunkThreshhold += _currentDrinkOrder.alcoholContent;

        // Go back to partying before our next drink.
        _currentSlot.Unlock();
        _currentSlot = null;

        ChangeState(CustomerState.Idle);
    }
}
