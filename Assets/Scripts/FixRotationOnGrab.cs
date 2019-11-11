using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotationOnGrab : MonoBehaviour
{
    [SerializeField] float rotationTime = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.name == "Left Hand" || transform.parent.name == "Right Hand")
        {
            //transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation, rotationTime);
            transform.LookAt(Vector3.forward);
        }
    }
}
