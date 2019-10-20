using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       
        if (collision.transform.tag == "floor")
        {
            Debug.Log("im in");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("i hit a " + other.transform.tag);
        if (other.transform.tag == "floor")
        {
            Debug.Log("im in");
            Destroy(this.gameObject);
        }
    }
}
