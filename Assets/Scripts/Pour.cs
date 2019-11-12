using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pour : MonoBehaviour
{
    private GameObject spout;
    private AudioSource pouringSource;

    public GameObject water;
    public int force;
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

    IEnumerator pour() {
        while (true) {
            Vector3 direction = spout.transform.position - transform.position;
            direction.Normalize();
            
            if (spout.transform.position.y < transform.position.y)
            {
                GameObject newDrop = Instantiate(water);
                newDrop.transform.position = spout.transform.position;
                newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
                AudioManager.S.PlaySound(pouringSource);
            } else
            {
                AudioManager.S.StopSound(pouringSource);
            }

            yield return new WaitForSeconds(0.005f);
        }
    }
}
