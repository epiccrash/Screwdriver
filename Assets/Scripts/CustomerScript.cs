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
    private const int FallTimerMax = 2;
    private const int MinFallAngle = 10;
    private const float SlotDistanceTolerance = 1.5f;

    [SerializeField]
    private List<DrinkRecipe> _orderableDrinks;

    [SerializeField]
    private float _drunkThreshhold;

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

    private float _alcoholLevel;
    private float _timeUntilNextDrink;
    private float _fallTimer;
    private CustomerState _state;

    private bool _isNormalRound;
    private bool _isLightningRound;

    private CustomerMovementController _movementController;
    private NavMeshAgent _agent;
    private Rigidbody _rigidBody;
    private BoxCollider _collider;

    [Header("Sound Making")]
    [SerializeField] private int frequency;
    [SerializeField] private int soundUpperBound = 2500;
    private AudioSource noise;

    private bool _isTutorial;

    private void Awake()
    {
        noise = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _alcoholLevel = 0;
        _isNormalRound = false;
        _isLightningRound = false;
        _currentDrinkOrder = null;
        _isTutorial = false;

        _movementController = GetComponent<CustomerMovementController>();
        _agent = GetComponent<NavMeshAgent>();
        _rigidBody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();

        // We're really good at avoiding things! Unless we're drunk.
        _agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;

        // Add ourselves as listeners to some important game events.
        GameManager.Instance.OnGameStart.AddListener(this.OnGameStart);
        GameManager.Instance.OnLightningRoundStart.AddListener(this.OnLightningRoundStart);
        GameManager.Instance.OnGameStart.AddListener(this.OnGameOver);

        ChangeState(CustomerState.Idle);
    }

    private void Update()
    {
        // Countdown drink timer.
        if (_isNormalRound && _state == CustomerState.Idle)
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

            if (!_isTutorial && (Vector3.Angle(transform.up, Vector3.up) >= MinFallAngle
                || Vector3.SqrMagnitude(transform.position - _currentSlot.StandLocation.position) > SlotDistanceTolerance
                || _fallTimer < FallTimerMax))
            {
                if (_fallTimer <= 0)
                {
                    ChangeState(CustomerState.Idle);
                }
                else
                {
                    _currentSlot.WipeRecipeDisplay();
                    _fallTimer -= Time.deltaTime;
                }
            }
        }
    }

    private void OnGameStart()
    {
        _isNormalRound = true;
    }

    private void OnLightningRoundStart()
    {
        // Move away from the seats.
        ChangeState(CustomerState.Idle);

        _isNormalRound = false;
        _isLightningRound = true;
    }

    private void OnGameOver()
    {
        // Move away from the seats.
        ChangeState(CustomerState.Idle);

        _isLightningRound = false;
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
                _collider.enabled = false;
                _currentSlot?.Unlock();
                _currentSlot = null;
                _currentDrinkOrder = null;
                RandomizeDrinkTimer();
                _movementController.StartRandomWanderBehavior();
                break;
            case CustomerState.WaitingForDrink:
                if (!_isTutorial)
                {
                    _agent.enabled = false;
                    _rigidBody.isKinematic = false;
                    _collider.enabled = true;
                    _fallTimer = FallTimerMax;
                }
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
            if (_isTutorial && _currentDrinkOrder != null)
            {
                GameManager.Instance.OnTutorialCustomerArrived();

                _currentSlot.InitializeRecipeDisplay(_currentDrinkOrder);

                ChangeState(CustomerState.WaitingForDrink);

                // Play customer grunt.
                AudioManager.S?.PlaySound(noise);
            }
            else if (_orderableDrinks.Count > 0)
            {
                int idx = UnityEngine.Random.Range(0, _orderableDrinks.Count);
                _currentDrinkOrder = _orderableDrinks[idx];

                _currentSlot.InitializeRecipeDisplay(_currentDrinkOrder);

                ChangeState(CustomerState.WaitingForDrink);

                // Play customer grunt.
                AudioManager.S?.PlaySound(noise);
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

    public void AssignTutorialSlot(CustomerSlot slot, DrinkRecipe drinkOrder)
    {
        _currentDrinkOrder = drinkOrder;
        _isTutorial = true;
        this.AssignSlot(slot);
    }

    public void OnDrinkReceived(GameObject drinkObj)
    {
        CupScript drink = drinkObj.GetComponentInChildren<CupScript>();

        Dictionary<IngredientType, float> ingredientsAndCorrectness = new Dictionary<IngredientType, float>();

        // Add all ingredients to the dictionary.
        foreach (IngredientType ingredient in _currentDrinkOrder.GetIngredientList())
        {
            IngredientUnit unit = _currentDrinkOrder.GetUnitFromIngredientType(ingredient);

            ingredientsAndCorrectness.Add(unit.ingredient, 0);
        }

        // Lets check everything in the cup and test how close it is to the drink we ordered.
        float tip = 0;
        bool hasIncorrectIngredients = false;

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

                hasIncorrectIngredients = true;
            }
        }

        // Now lets calculate the tip.
        bool isPerfectDrink = true;

        foreach (IngredientType ingredient in ingredientsAndCorrectness.Keys)
        {
            if (Mathf.Abs(ingredientsAndCorrectness[ingredient] - 1) > GameConstants.DrinkPerfectionPercentageEpsilon)
            {
                print("Ingredient: " + ingredient + " Value: " + ingredientsAndCorrectness[ingredient]);
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
            GameManager.Instance.gameData.perfectDrinks++;
            tip += _tipBonusOnPerfect;
        }
        else if (_isTutorial)
        {
            // A really dumb way to try to make sure that tutorial drinks are perfect.
            if (!hasIncorrectIngredients)
            {
                return;
            }
        }

        GameManager.Instance.gameData.totalDrinks++;

        // Destroy the drink.
        Destroy(drinkObj);

        tip = Mathf.Max(0, tip);
        print("Tip: " + tip);

        // Use the tip jar to add the tip.
        TipScript.Instance.AddTip(tip);

        // Are we drunk?
        _alcoholLevel += drink.GetABV();

        if (_alcoholLevel >= _drunkThreshhold)
        {
            // If we're drunk, we won't be as good at avoiding obstacles.
            _agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        }

        _currentDrinkOrder = null;

        if (_isTutorial)
        {
            _isTutorial = false;
        }

        // Go back to partying before our next drink.
        ChangeState(CustomerState.Idle);
    }
}
