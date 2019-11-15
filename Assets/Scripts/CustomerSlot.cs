using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSlot : MonoBehaviour
{
    [SerializeField]
    private DrinkSlot _drinkSlot;

    [SerializeField]
    private RecipeDisplayScript _recipeDisplay;

    private bool _isFree;

    public bool IsFree { get { return _isFree; } }
    public Transform StandLocation { get { return transform; } }

    private void OnDrawGizmos()
    {
        // Draw the sphere to indicate the stand location.
        Gizmos.color = new Color(0, 0.5f, 0.5f, 0.8f);
        Gizmos.DrawSphere(transform.position, 0.1f);

        if (Application.IsPlaying(this))
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position, transform.forward);
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
        _recipeDisplay.WipeDisplay();

        _isFree = true;
    }

    public void SetOnDrinkServed(Delegates.onDrinkServedDel onServedFunc)
    {
        _drinkSlot.onDrinkServed = onServedFunc;
    }

    public void InitializeRecipeDisplay(DrinkRecipe drinkOrder)
    {
        _recipeDisplay.InitializeForNewDrink(drinkOrder);
    }

    public void WipeRecipeDisplay()
    {
        _recipeDisplay.WipeDisplay();
    }

    public void HighlightCustomer()
    {
        _recipeDisplay.HighlightDrinkName();
    }

    public void UnhighlightCustomer()
    {
        _recipeDisplay.UnhighlightDrinkName();
    }
}
