using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillTray : MonoBehaviour
{
    public GameObject IceCube;
    public int fillNum = 150;
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
        float xMax = transform.position.x + 0.0001f;
        float xMin = transform.position.x - 0.0001f;
        float zMax = transform.position.z + 0.0001f;
        float zMin = transform.position.z - 0.0001f;
        print(xMax + ", " + xMin + "; " + zMax + ", " + zMin);

        float x;
        float z;
        GameObject cube;
        for (int i = 0; i<fillNum; i++)
        {
            x = Random.Range(xMin, xMax);
            z = Random.Range(zMin, zMax);
            cube = GameObject.Instantiate(IceCube);
            cube.transform.position = new Vector3(x, transform.position.y, z);
            cube.transform.rotation = Random.rotation;
        }
    }
}
