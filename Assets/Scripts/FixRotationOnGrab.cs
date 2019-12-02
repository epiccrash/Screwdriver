using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotationOnGrab : MonoBehaviour
{
    Transform target;

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null) 
        {
            target = null;
        }
        else 
        {
            if (target == null) {
                target = transform.parent.GetChild(0);
            }

            if (transform.parent.name == "LeftHand" || transform.parent.name == "RightHand")
            {
                transform.localPosition = target.localPosition;
                transform.rotation = target.rotation * Quaternion.Euler(0, 90, 90);
                //transform.LookAt(target, -Vector3.right);
            }
        }
    }
}
