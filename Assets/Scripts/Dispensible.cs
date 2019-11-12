using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispensible : MonoBehaviour
{

    private bool dispensed=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dispensed)
        {
            // Commented this out just for testing
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else
        {
            // Commented this out just for testing
             GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void dispense() {
        dispensed = true;
    }
}
