using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkSlot : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // Draw the drink zone collider.
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
