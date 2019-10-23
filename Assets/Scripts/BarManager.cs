using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.LoadedFromResources, true, "BarManager")]
public class BarManager : Singleton<BarManager>
{
    private List<CustomerSlot> _customerSlots;
    private Queue<CustomerScript> _customerQueue;

    protected override void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        _customerSlots = new List<CustomerSlot>();
        _customerQueue = new Queue<CustomerScript>();

        // Find all slots in the scene.
        GameObject[] slotObjs = GameObject.FindGameObjectsWithTag("CustomerSlot");

        foreach (GameObject slot in slotObjs)
        {
            _customerSlots.Add(slot.GetComponent<CustomerSlot>());
        }
    }

    private void Update()
    {
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
}
