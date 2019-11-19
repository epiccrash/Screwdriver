﻿using System.Collections;
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
        
    }


    IEnumerator pour()
    {
        Debug.Log("coroutine started");
        while (true)
        {
            Vector3 direction = spout.transform.position - transform.position;
            direction.Normalize();
            Debug.Log("spout: "+spout.transform.position.y);
            Debug.Log("cup: "+transform.position.y);
            if (spout.transform.position.y < transform.position.y && pourable)
            {
                Debug.Log("made it in the if");
                cone.DecreaseFill();
                GameObject newDrop = Instantiate(baseLiquid);
                newDrop.GetComponent<Renderer>().material = liquidColor;
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
        Debug.Log("i am filled");
    }

    public void Empty() {
        pourable = false;
        liquidColor = null;
        Debug.Log("i am empty");
    }

}
