using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.LoadedFromResources, true, "BarManager")]
public class BarManager : Singleton<BarManager>
{
    private List<CustomerSlot> _customerSlots;

    protected override void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        _customerSlots = new List<CustomerSlot>();

        // Find all slots in the scene.
        GameObject[] slotObjs = GameObject.FindGameObjectsWithTag("CustomerSlot");

        foreach (GameObject slot in slotObjs)
        {
            _customerSlots.Add(slot.GetComponent<CustomerSlot>());
        }
    }

    public CustomerSlot GetAndLockAvailableSlot()
    {
        List<CustomerSlot> availableSeats = new List<CustomerSlot>();

        foreach (CustomerSlot slot in _customerSlots)
        {
            if (slot.IsFree)
            {
                availableSeats.Add(slot);
            }
        }

        if (availableSeats.Count <= 0)
        {
            return null;
        }

        // Get a random seat from the free seats.
        int randomIndx = UnityEngine.Random.Range(0, availableSeats.Count);

        availableSeats[randomIndx].Lock();
        return availableSeats[randomIndx];
    }
}
