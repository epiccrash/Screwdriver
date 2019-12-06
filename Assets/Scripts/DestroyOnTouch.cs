using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    private const float LiquidDropLifetime = 10;

    private SpawnerScript spawnScript;
    private bool _isLiquid;

    // Start is called before the first frame update
    void Start()
    {
        spawnScript = GetComponentInParent<SpawnerScript>();
        _isLiquid = transform.CompareTag("Water") || transform.CompareTag("Alcohol");

        // If we're a liquid drop, we should destroy ourselves after 10 seconds.
        if (_isLiquid)
        {
            Destroy(this.gameObject, LiquidDropLifetime);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.transform.tag == "floor"
            || (_isLiquid && !(collision.transform.CompareTag("Container") || collision.transform.CompareTag("Spout") || collision.transform.CompareTag("SnapSpot"))))
        {
            //print(gameObject.name);
            if (spawnScript != null) { spawnScript.needSpawn = true; }

            SmashOnCollision smashOnCollision = GetComponent<SmashOnCollision>();
            if (smashOnCollision != null)
            {
                smashOnCollision.Smash();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.transform.tag == "floor"
            || (_isLiquid && !(other.transform.CompareTag("Container") || other.transform.CompareTag("Spout") || other.transform.CompareTag("SnapSpot"))))
        {
            Destroy(this.gameObject);
        }
    }
}