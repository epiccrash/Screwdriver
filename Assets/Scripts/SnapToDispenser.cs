using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToDispenser : MonoBehaviour
{
    public string tagToCheck;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
       
        if (other.tag == tagToCheck)
        {

            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                other.transform.position = transform.position;
                other.transform.rotation = transform.rotation;
            }
            else {
                GameObject realCup = other.transform.parent.gameObject;
                if (realCup.GetComponent<Rigidbody>() != null)
                {
                    realCup.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    realCup.transform.position = transform.position;
                    realCup.transform.rotation = transform.rotation;
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == tagToCheck)
        {
            if (other.GetComponent<Rigidbody>() != null)
            {
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
            else
            {
                GameObject realCup = other.transform.parent.gameObject;
                if (realCup.GetComponent<Rigidbody>() != null)
                {
                    realCup.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }
            }
        }
    }
}
