using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    // Just checks to see if the spawner can spawn an object in
    private bool open = true;

    public bool needSpawn = false;
    // [SerializeField] float spawnDelay = 0.0f;

    public bool GetOpen()
    {
        return open;
    }

    public void SetOpen(bool state)
    {
        open = state;
    }

    void OnTriggerStay(Collider other)
    {
        SetOpen(false);
    }

    void OnTriggerEnter(Collider other)
    {
        SetOpen(false);
    }
    void OnTriggerExit(Collider other)
    {
        SetOpen(true);
    }
}
