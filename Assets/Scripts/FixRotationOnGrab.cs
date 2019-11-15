using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotationOnGrab : MonoBehaviour
{
    [SerializeField] float rotationTime = 0.5f;
    bool check;

    // Start is called before the first frame update
    void Start()
    {
        check = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent != null && (transform.parent.name == "LeftHand" || transform.parent.name == "RightHand"))
        {
            // transform.rotation = Quaternion.Lerp(transform.rotation, transform.parent.rotation, rotationTime * Time.deltaTime);
            //transform.LookAt(Vector3.forward);
            Quaternion rotation = Quaternion.LookRotation(transform.parent.transform.right, Vector3.up);
            // transform.rotation = rotation;
            // transform.LookAt(transform.parent);
            // transform.Rotate(new Vector3(0, 90, 0));
            check = true;
        }
        else {
            check = false;
        }
    }
}
