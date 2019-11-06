﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Items and Spawner Parent")]
    [SerializeField] List<GameObject> spawnItems;
    [SerializeField] Transform spawnerParent;
    [Header("Number of children of spawnerParent MUST match the number of items")]

    private List<GameObject> spawners;

    // Start is called before the first frame update
    void Awake()
    {
        spawners = new List<GameObject>();
        foreach (Transform child in spawnerParent)
        {
            spawners.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < spawners.Count; i++)
        {
            GameObject g = spawners[i];
            if (g.GetComponent<SpawnerScript>().GetOpen())
            {
                Instantiate(spawnItems[i], g.transform.position, Quaternion.identity);
            }
        }
    }
}
