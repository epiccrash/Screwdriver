using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DrinkSlot : MonoBehaviour
{
    // Delegate to call when the drink is served.
    private delegate void OnDrinkServed(GameObject drink);
    private OnDrinkServed _onDrinkServed;

    // Action for when the drink is put down.
    private UnityAction _onDrinkDetachedFromHand;

    private GameObject _drinkInSlot;

    private void OnDrawGizmos()
    {
        // Draw the drink zone collider.
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }

    private void Start()
    {
        _onDrinkDetachedFromHand += OnDrinkPutDown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Container"))
        {
            GameObject parent = other.gameObject.transform.parent.gameObject;

            if (parent != null && _drinkInSlot != parent)
            {
                _drinkInSlot = parent;

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

                // Now that the drink isn't in the slot, we'll stop listening for the drop.
                other.gameObject.GetComponentInParent<Valve.VR.InteractionSystem.Throwable>()
                    .onDetachFromHand.RemoveListener(_onDrinkDetachedFromHand);
            }
        }
    }

    private void OnDrinkPutDown()
    {
        Debug.Log("Drink put down");
        _onDrinkServed?.Invoke(_drinkInSlot);
    }
}
