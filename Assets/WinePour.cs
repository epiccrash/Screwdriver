using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinePour : MonoBehaviour
{
    public GameObject spout;
    public GameObject wineLiquid;
    private bool pour;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PourWine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        pour = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        pour = false;
    }

    IEnumerator PourWine()
    {
        while (true)
        {
            if (pour)
            {
                GameObject newDrop = Instantiate(wineLiquid);
                newDrop.transform.position = spout.transform.position;
            }
        }
    }
}
