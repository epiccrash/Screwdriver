using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MixerScript : MonoBehaviour
{
    private Vector3 velocity=Vector3.zero, lastVelocity= Vector3.zero;
    private int shakes = 0, stillCount=0;

    public GameObject fillCone;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShakeTest());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShakeTest() {
        while (true)
        {
            //makes sure the mixer is moving
            if (GetComponent<VelocityEstimator>().GetVelocityEstimate().magnitude != 0)
                velocity = GetComponent<VelocityEstimator>().GetVelocityEstimate();

            //if the velocity changes directions
            if (Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x) || Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x) || Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x))
                fillCone.GetComponent<ConeModify>().MakeOpaque();
            

            lastVelocity = velocity;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ResetShakes()
    {
        shakes = 0;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "IceCube") {
            if (other.transform.parent == null)
                other.transform.parent = this.gameObject.transform;
            else
                other.transform.parent = null;
            // other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
            // other.gameObject.GetComponent<Interactable>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "IceCube")
        {
            if (other.transform.parent == this.gameObject.transform)
                other.transform.parent = null;
        }
    }
}
