using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class MixerScript : MonoBehaviour
{
    private Vector3 velocity, lastVelocity= Vector3.zero;
    private int shakes = 0, stillCount=0;

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
            if (GetComponent<VelocityEstimator>().GetVelocityEstimate().magnitude != 0)
                velocity = GetComponent<VelocityEstimator>().GetVelocityEstimate();
            

                

            if (Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x) || Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x) || Mathf.Sign(velocity.x) != Mathf.Sign(lastVelocity.x))
            {
                shakes += 1;
            }


            //Debug.Log(velocity);
            //Debug.Log(shakes);
            lastVelocity = velocity;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void ResetShakes()
    {
        shakes = 0;
    }
}
