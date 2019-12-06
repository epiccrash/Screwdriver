using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juiceable : MonoBehaviour
{
    public GameObject juiceDrop;
    public int jucieUnits;
    private SpawnerScript spawnpoint;
    

    // Start is called before the first frame update
    void Start()
    {
        spawnpoint = GetComponentInParent<SpawnerScript>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadFruit() {
        
        if (spawnpoint != null) {
            spawnpoint.needSpawn = true;
        }
    }

}
