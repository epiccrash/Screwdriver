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
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        else {
             GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void dispense() {
        dispensed = true;
    }
}
