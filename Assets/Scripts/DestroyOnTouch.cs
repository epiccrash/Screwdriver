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
            || (_isLiquid && !(collision.transform.CompareTag("Container") || collision.transform.CompareTag("Spout"))))
        {
            if (spawnScript != null)
                spawnScript.needSpawn = true;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isLiquid && other.transform.tag == "Fill")
        {
            other.GetComponent<ConeModify>().ChangeFill(this.gameObject);
        }
        else if (other.transform.tag == "floor"
            || (_isLiquid && !(other.transform.CompareTag("Container") || other.transform.CompareTag("Spout") || other.transform.CompareTag("SnapSpot"))))
        {
            Destroy(this.gameObject);
        }
    }
}