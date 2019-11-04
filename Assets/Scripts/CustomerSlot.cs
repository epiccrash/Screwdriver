using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSlot : MonoBehaviour
{
    [SerializeField]
    private Transform _standLocation;

    [SerializeField]
    private DrinkSlot _drinkSlot;

    private bool _isFree;

    public bool IsFree { get { return _isFree; } }
    public Transform StandLocation { get { return _standLocation; } }

    private void OnDrawGizmos()
    {
        // Draw the sphere to indicate the stand location.
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(_standLocation.position, 0.1f);
    }

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

    public void SetOnDrinkServed(Delegates.onDrinkServedDel onServedFunc)
    {
        _drinkSlot.onDrinkServed = onServedFunc;
    }
}
