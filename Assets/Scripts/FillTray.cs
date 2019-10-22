using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTray : MonoBehaviour
{
    public GameObject IceCube;
    // Start is called before the first frame update
    void Start()
    {
        Fill();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fill()
    {
        float xMax = transform.position.x + GetComponent<Collider>().bounds.extents.x;
        float xMin = transform.position.x - GetComponent<Collider>().bounds.extents.x;
        float zMax = transform.position.z + GetComponent<Collider>().bounds.extents.z;
        float zMin = transform.position.z - GetComponent<Collider>().bounds.extents.z;
        float x;
        float z;
        GameObject cube;
        for (int i = 0; i<150; i++)
        {
            x = Random.Range(xMin, xMax);
            z = Random.Range(zMin, zMax);
            cube = GameObject.Instantiate(IceCube);
            cube.transform.position = new Vector3(x, transform.position.y, z);
            cube.transform.rotation = Random.rotation;
        }
    }
}
