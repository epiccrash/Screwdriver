using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakerOnCup : MonoBehaviour
{
    private bool snapped = false;
    private GameObject snapLocation;
    private MixerScript cup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (snapped) {
            transform.position = snapLocation.transform.position;
            transform.rotation = snapLocation.transform.rotation;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Container"&& !snapped) {
            snapLocation = collision.transform.GetChild(8).gameObject;
            snapped = true;
            cup = collision.transform.GetComponent<MixerScript>();
            if (cup != null) {
                cup.canMix(true);
            }
        }
    }

    public void unSnap() {
        if (cup != null) {
            cup.canMix(false);
        }
        snapped = false;
    }
}
