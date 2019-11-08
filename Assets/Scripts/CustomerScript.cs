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
    private const float DrinkPerfectionPercentageEpsilon = 0.35f;
    private const int FallTimerMax = 4;
    private const int MinFallAngle = 5;

    [SerializeField]
    private TextMeshPro _drinkNameText;

    [SerializeField]
    private TextMeshPro _drinkRecipeText;

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
    private GameObject _tipJar;

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
    private float _fallTimer;
    private CustomerState _state;

    private CustomerMovementController _movementController;
    private NavMeshAgent _agent;
    private Rigidbody _rigidBody;
    private BoxCollider _collider;

    [Header("Sound Making")]
    [SerializeField] private int frequency;
    [SerializeField] private int soundUpperBound = 2500;
    private AudioSource noise;

    private void Awake()
    {
        noise = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _alchoholLevel = 0;

        _drinkNameText.enabled = false;
        _drinkRecipeText.enabled = false;

        _movementController = GetComponent<CustomerMovementController>();
        _agent = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();

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
        else if (_state == CustomerState.WaitingForDrink)
        {
            AudioManager.S?.PlaySound(noise, frequency, soundUpperBound);

            if (Mathf.Abs(transform.rotation.eulerAngles.x) >= MinFallAngle)
            {
                if (_fallTimer <= 0)
                {
                    ChangeState(CustomerState.Idle);
                }
                else
                {
                    _drinkNameText.enabled = false;
                    _drinkRecipeText.enabled = false;
                    _fallTimer -= Time.deltaTime;
                }
            }
        }
    }

    private void RandomizeDrinkTimer()
    {
        _timeUntilNextDrink = UnityEngine.Random.Range(_minTimeBetweenDrinks, _maxTimeBetweenDrinks);
    }

    private string GenerateRecipeString()
    {
        List<string> recipeLines = new List<string>();

        foreach (IngredientUnit unit in _currentDrinkOrder.recipe)
        {
            string line = unit.ingredient.ToString();

            if (unit.amountMatters)
            {
                line += " (" + unit.amountDescription + ")";
            }

            line += "\n";

            recipeLines.Add(line);
        }

        string result = "";

        foreach (string line in recipeLines)
        {
            result += line;
        }

        return result;
    }

    private void ChangeState(CustomerState newState)
    {
        switch (newState)
        {
            case CustomerState.Idle:
                _agent.enabled = true;
                _rigidBody.isKinematic = true;
                _drinkNameText.enabled = false;
                _drinkRecipeText.enabled = false;
                _currentSlot?.Unlock();
                _currentSlot = null;
                RandomizeDrinkTimer();
                _movementController.StartRandomWanderBehavior();
                break;
            case CustomerState.WaitingForDrink:
                _agent.enabled = false;
                _rigidBody.isKinematic = false;

                _drinkNameText.text = _currentDrinkOrder.drinkName;
                _drinkNameText.enabled = true;

                _drinkRecipeText.text = GenerateRecipeString();
                _drinkRecipeText.enabled = true;

                _fallTimer = FallTimerMax;
                break;
            default:
                break;
        }

        _state = newState;
    }

    public void OnArrivedAtDest()
    {
        _tipJar.GetComponentInChildren<TipScript>().AddTip(0.99f);
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

            if (_state == CustomerState.WaitingForDrink)
            {
                // Play customer grunt
                AudioManager.S?.PlaySound(noise);
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
        _tipJar.GetComponentInChildren<TipScript>().AddTip(tip);
        // Use the tip jar to add the tip.

        // Are we drunk?
        _drunkThreshhold += _currentDrinkOrder.alcoholContent;

        // Go back to partying before our next drink.
        ChangeState(CustomerState.Idle);
    }
}
