using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSlot : MonoBehaviour
{
    [SerializeField]
    private Transform _standLocation;

    private bool _isFree = true;

    public bool IsFree { get { return _isFree; } }

    private void Start()
    {
        CustomerSlot slot = BarManager.Instance.GetAndLockAvailableSlot();
        slot.Lock();
        Debug.Log(slot);
    }

    public void Lock()
    {
        _isFree = false;
    }
}
