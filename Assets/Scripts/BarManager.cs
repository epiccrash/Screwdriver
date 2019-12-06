using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.LoadedFromResources, true, "BarManager")]
public class BarManager : Singleton<BarManager>
{
    [SerializeField]
    private float _customerWanderZLimit;

    [SerializeField]
    private DrinkRecipe _tutorialDrink;

    private List<CustomerSlot> _customerSlots;
    private List<RecipeDisplayScript> _recipeDisplays;
    private Queue<CustomerScript> _customerQueue;
    private CupScript _currentTrackedCup;

    private CustomerScript _tutorialCustomer;
    private CustomerSlot _centerSlot;

    private bool _isRegularGame;
    private bool _isLightningRound;
    private bool _isTutorial;

    public float CustomerZWanderLimit { get { return _customerWanderZLimit; } }

    protected override void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        _customerSlots = new List<CustomerSlot>();
        _customerQueue = new Queue<CustomerScript>();

        _isRegularGame = false;
        _isLightningRound = false;

        // Find the bride to lead the tutorial.
        GameObject brideObj = GameObject.Find("Bride");

        if (brideObj != null)
        {
            _tutorialCustomer = brideObj.GetComponent<CustomerScript>();

            if (_tutorialCustomer == null)
            {
                Debug.LogError("Bride game object has no CustomerScript component.");
            }
        }
        else
        {
            Debug.LogError("Could not find customer named 'Bride' for tutorial.");
        }

        // Find the center seat to lead the tutorial from.
        GameObject centerSeatObj = GameObject.Find("Center Seat");

        if (centerSeatObj != null)
        {
            _centerSlot = centerSeatObj.GetComponent<CustomerSlot>();

            if (_centerSlot == null)
            {
                Debug.LogError("Center seat object has no CustomerSlot script component.");
            }
        }
        else
        {
            Debug.LogError("Could not find seat named 'Center Seat' for tutorial.");
        }

        // Find all slots in the scene.
        GameObject[] slotObjs = GameObject.FindGameObjectsWithTag("CustomerSlot");

        foreach (GameObject slot in slotObjs)
        {
            _customerSlots.Add(slot.GetComponent<CustomerSlot>());
        }

        RecipeDisplayScript[] displayArray = FindObjectsOfType<RecipeDisplayScript>();
        _recipeDisplays = new List<RecipeDisplayScript>(displayArray);

        // Add listeners for game events.
        GameManager.Instance.OnTutorialStart.AddListener(this.OnTutorialStart);
        GameManager.Instance.OnGameStart.AddListener(this.OnGameStart);
        GameManager.Instance.OnLightningRoundStart.AddListener(this.OnLightningRoundStart);
        GameManager.Instance.OnGameOver.AddListener(this.OnGameOver);
    }

    private void Update()
    {
        if (_isRegularGame)
        {
            // People take turns being seated.
            CustomerSlot freeSlot = GetRandomAvailableSlot();

            if (freeSlot != null)
            {
                CustomerScript nextCustomer = GetCustomerFromQueue();

                if (nextCustomer != null)
                {
                    freeSlot.Lock();
                    nextCustomer.AssignSlot(freeSlot);
                }
            }
        }
    }

    private void OnTutorialStart()
    {
        _isTutorial = true;

        if (_tutorialDrink == null)
        {
            Debug.LogError("No tutorial drink assigned to BarManager prefab.");
        }
        else
        {
            _tutorialCustomer.AssignTutorialSlot(_centerSlot, _tutorialDrink);
        }
    }

    private void OnGameStart()
    {
        _isRegularGame = true;
    }

    private void OnLightningRoundStart()
    {
        _isRegularGame = false;
        _isLightningRound = true;
    }

    private void OnGameOver()
    {
        _isLightningRound = false;

        // Nobody is sitting anymore!
        foreach (CustomerSlot slot in _customerSlots)
        {
            slot.Unlock();
        }
    }

    private CustomerSlot GetRandomAvailableSlot()
    {
        List<CustomerSlot> availableSeats = new List<CustomerSlot>();

        foreach (CustomerSlot slot in _customerSlots)
        {
            if (slot.IsFree)
            {
                availableSeats.Add(slot);
            }
        }

        if (availableSeats.Count > 0)
        {
            // Get a random seat from the free seats.
            int randomIndx = UnityEngine.Random.Range(0, availableSeats.Count);

            return availableSeats[randomIndx];
        }

        return null;
    }

    private CustomerScript GetCustomerFromQueue()
    {
        if (_customerQueue.Count > 0)
        {
            return _customerQueue.Dequeue();
        }

        return null;
    }

    public void EnterCustomerQueue(CustomerScript customer)
    {
        _customerQueue.Enqueue(customer);
    }

    public void UpdateIngredientDisplays(IngredientType type, CupScript updatedCup)
    {
        bool changedTrackedCup = false;

        if (_currentTrackedCup != updatedCup)
        {
            changedTrackedCup = true;
            _currentTrackedCup = updatedCup;
        }

        foreach (RecipeDisplayScript display in _recipeDisplays)
        {
            if (changedTrackedCup)
            {
                display.ResetIngredientDisplayForNewCup();
            }
            else
            {
                display.UpdateIngredientRing(type);
            }
        }
    }

    public void UpdateShakeValue(bool isShaken, CupScript updatedCup)
    {
        bool changedTrackedCup = false;

        if (_currentTrackedCup != updatedCup)
        {
            changedTrackedCup = true;
            _currentTrackedCup = updatedCup;
        }

        foreach (RecipeDisplayScript display in _recipeDisplays)
        {
            if (changedTrackedCup)
            {
                display.ResetIngredientDisplayForNewCup();
            }
            else
            {
                display.UpdateShakerDisplay();
            }
        }
    }

    public void OnCupPickedUp(CupScript pickedUpCup)
    {

        if (_currentTrackedCup != pickedUpCup)
        {
            _currentTrackedCup = pickedUpCup;

            foreach (RecipeDisplayScript display in _recipeDisplays)
            {
                display.ResetIngredientDisplayForNewCup();
            }
        }
    }

    public void OnCupServed(GameObject servedCupObj)
    {
        CupScript servedCup = servedCupObj.GetComponent<CupScript>();

        if (_currentTrackedCup == servedCup)
        {
            _currentTrackedCup = null;

            foreach (RecipeDisplayScript display in _recipeDisplays)
            {
                display.ResetIngredientDisplayForNewCup();
            }
        }

        if (_isTutorial)
        {
            GameManager.Instance.OnTutorialDrinkServed();
            _isTutorial = false;
        }
    }

    public float GetCurrentCupIngredientCorrectness(IngredientType type, int amount)
    {
        if (_currentTrackedCup != null)
        {
            return _currentTrackedCup.GetIngredientCorrectness(type, amount);
        }

        return 0;
    }

    public float GetCurrentCupIngredientAmount(IngredientType type)
    {
        if (_currentTrackedCup != null)
        {
            return _currentTrackedCup.GetIngredientAmount(type);
        }

        return 0;
    }

    public bool GetCupIsShaken()
    {
        if (_currentTrackedCup != null)
        {
            return _currentTrackedCup.HasBeenShaken;
        }

        return false;
    }
}
