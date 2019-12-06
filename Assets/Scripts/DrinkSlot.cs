using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrinkSlot : MonoBehaviour
{
    public Delegates.onDrinkServedDel onDrinkServed;

    // Action for when the drink is put down.
    private UnityAction _onDrinkDetachedFromHand;

    private GameObject _drinkInSlot;
    private CustomerSlot _parentSlot;

    private bool _isTutorial;

    private void Start()
    {
        _onDrinkDetachedFromHand += OnDrinkPutDown;
        _parentSlot = transform.GetComponentInParent<CustomerSlot>();

        GameManager.Instance.OnTutorialStart.AddListener(OnTutorialStart);
        GameManager.Instance.OnGameStart.AddListener(OnGameStart);
    }

    private void OnTutorialStart()
    {
        _isTutorial = true;
    }

    private void OnGameStart()
    {
        _isTutorial = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Container"))
        {
            GameObject parent = other.gameObject.transform.parent.gameObject;

            if (parent != null && _drinkInSlot != parent)
            {
                _drinkInSlot = parent;

                _parentSlot.HighlightCustomer();

                // When the drink is put down we'll trigger this unity action.
                other.gameObject.GetComponentInParent<Valve.VR.InteractionSystem.Throwable>()
                    .onDetachFromHand.AddListener(_onDrinkDetachedFromHand);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Container"))
        {
            GameObject parent = other.gameObject.transform.parent.gameObject;

            if (parent != null && _drinkInSlot == parent)
            {
                _drinkInSlot = null;

                _parentSlot.UnhighlightCustomer();

                // Now that the drink isn't in the slot, we'll stop listening for the drop.
                other.gameObject.GetComponentInParent<Valve.VR.InteractionSystem.Throwable>()
                    .onDetachFromHand.RemoveListener(_onDrinkDetachedFromHand);
            }
        }
    }

    private void OnDrinkPutDown()
    {
        if (_drinkInSlot != null)
        {
            CupScript currentDrink = _drinkInSlot.GetComponentInChildren<CupScript>();

            // Don't try to serve an empty drink!
            if (currentDrink != null && !currentDrink.IsEmpty())
            {
                if (!_isTutorial)
                {
                    onDrinkServed?.Invoke(_drinkInSlot);
                    BarManager.Instance.OnCupServed(_drinkInSlot);
                }
                else if (this.gameObject.transform.parent.name == "Center Seat")
                {
                    onDrinkServed?.Invoke(_drinkInSlot);
                    BarManager.Instance.OnCupServed(_drinkInSlot);
                }

            }
        }
    }
}
