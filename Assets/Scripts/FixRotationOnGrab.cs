using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotationOnGrab : MonoBehaviour
{
    bool check;

    Transform target;

    // Start is called before the first frame update
    void Start()
    {
        check = false;
    }

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
                transform.LookAt(target);
            }
        }
    }
}
