using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourFromCup : MonoBehaviour
{

    public GameObject baseLiquid;
    private Material liquidColor;
    private bool pourable=false;
    public GameObject spout;
    public float force;
    private AudioSource pouringSource;
    public Shader trailShader;

    public ConeModify cone;

    // Start is called before the first frame update
    void Start()
    {
        spout = transform.GetChild(0).gameObject;
        pouringSource = GetComponent<AudioSource>();
        StartCoroutine(pour());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 spoutPos = spout.transform.localPosition;
        //spoutPos.x = Mathf.Clamp(spout.transform.localPosition.x, -0.6f, 0.6f);
        //spoutPos.z = Mathf.Clamp(spout.transform.localPosition.z, -0.6f, 0.6f);
        spout.transform.localPosition = spoutPos;
        // spout.GetComponent<Rigidbody>().AddForce(-Vector3.up);
    }


    IEnumerator pour()
    {
        
        while (true)
        {
            Vector3 direction = spout.transform.position - transform.position;
            direction.Normalize();
            
            if (spout.transform.position.y < transform.position.y && pourable)
            {
                cone.DecreaseFill();
                //baseLiquid.GetComponent<TrailRenderer>().materials[0] = liquidColor;
                GameObject newDrop = Instantiate(baseLiquid);
                Material dropMat = new Material(trailShader);
                dropMat.color = new Color(liquidColor.color.r, liquidColor.color.g, liquidColor.color.b, 0.572549f);

                newDrop.GetComponent<TrailRenderer>().sharedMaterial = dropMat;
                // newDrop.GetComponent<TrailRenderer>().enabled=true;
                
                newDrop.transform.position = spout.transform.position;
                newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
                AudioManager.S.PlaySound(pouringSource);
                
            }
            else
            {
                AudioManager.S.StopSound(pouringSource);
            }

            yield return new WaitForSeconds(0.005f);
        }
    }



    public void Fill(Material color) {
        pourable = true;
   
        liquidColor = color;
        
        
    }

    public void Empty() {
        pourable = false;
        liquidColor = null;
       
    }

}
