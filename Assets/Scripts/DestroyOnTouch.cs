using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTouch : MonoBehaviour
{
    private SpawnerScript spawnScript;

    // Start is called before the first frame update
    void Start()
    {
        spawnScript = GetComponentInParent<SpawnerScript>();
        Debug.Log(spawnScript);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HEALP");
       
        if (collision.transform.tag == "floor")
        {
            if(spawnScript!=null)
                spawnScript.needSpawn = true;
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {  
        if (other.transform.tag == "floor")
        {
            Destroy(this.gameObject);
        } else if (other.transform.tag == "Fill")
        {
            other.GetComponent<ConeModify>().ChangeFill(this.gameObject);
        }
    }
}
