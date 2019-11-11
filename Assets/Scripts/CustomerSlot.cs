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
        Gizmos.color = new Color(0, 0.5f, 0.5f, 0.8f);
        Gizmos.DrawSphere(_standLocation.position, 0.1f);

        if (Application.IsPlaying(this))
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(_standLocation.transform.position, _standLocation.transform.forward);
        }
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
