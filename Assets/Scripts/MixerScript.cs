using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MixerScript : MonoBehaviour
{
    private Vector3 velocity=Vector3.zero, lastVelocity= Vector3.zero;
    private int shakes = 0, stillCount=0;
    private CupScript liquid;
    private bool mixable = false;
    //public GameObject fillCone;

    // Start is called before the first frame update
    void Start()
    {
        liquid = GetComponent<CupScript>();
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
            if ((Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x) || Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x) || Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x))&&mixable)
                Mix();
            

            lastVelocity = velocity;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ResetShakes()
    {
        shakes = 0;
    }

    //the method that is run when the mixer is shaken
    private void Mix() {


        if(shakes>10)
            liquid.shakeIt();

        shakes++;
    }

    public void canMix(bool canIt) {
        mixable = canIt;
    }
}
