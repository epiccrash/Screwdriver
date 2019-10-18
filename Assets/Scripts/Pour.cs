﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pour : MonoBehaviour
{
    private GameObject spout;

    public GameObject water;
    public int force;
    // Start is called before the first frame update
    void Start()
    {
        spout = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = spout.transform.position - transform.position;
        direction.Normalize();

        if (spout.transform.position.y < transform.position.y) {
            GameObject newDrop = Instantiate(water);
            newDrop.transform.position = spout.transform.position;
            newDrop.GetComponent<Rigidbody>().AddForce(direction*force);
        }
    }
}
