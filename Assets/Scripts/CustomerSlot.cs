using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSlot : MonoBehaviour
{
    [SerializeField]
    private Transform _standLocation;

    private bool _isFree;

    public bool IsFree { get { return _isFree; } }
    public Transform StandLocation { get { return _standLocation; } }

    private void Start()
    {
        Unlock();
    }

    public void Lock()
    {
        _isFree = false;
    }

    public void Unlock()
    {
        _isFree = true;
    }
}
