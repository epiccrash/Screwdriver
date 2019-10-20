using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInGlass : MonoBehaviour
{

    private float force = 1;
    private bool jitter = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jitter) {
            
            Vector3 dir= new Vector3(Random.Range(-1.0f, 1.0f),0,Random.Range(-1.0f, 1.0f));
            GetComponent<Rigidbody>().AddForce(dir*force);
        }

        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //gameObject.layer = LayerMask.NameToLayer("Water");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag=="Container") {
            this.gameObject.GetComponent<TrailRenderer>().time = 3;

            jitter = true;

            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
            

            gameObject.layer = LayerMask.NameToLayer("Water2MoreWet");
        }

        if (collision.transform.tag == "Water")
        {
            Debug.Log("i hit a "+collision.transform.tag);
            this.gameObject.GetComponent<TrailRenderer>().time = 3;

            jitter = true;

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;


            gameObject.layer = LayerMask.NameToLayer("Water2MoreWet");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "water") {
            this.gameObject.GetComponent<TrailRenderer>().time = 3;

            jitter = true;

            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;


            gameObject.layer = LayerMask.NameToLayer("Water2MoreWet");
        }
    }
}
