using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinePour : MonoBehaviour
{
    public GameObject spout;
    public GameObject wine;

    [SerializeField]
    private ParticleSystem _particleSystem;

    private bool pour;
    // Start is called before the first frame update
    void Start()
    {
        pour = false;
        StartCoroutine(PourWine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //print(collision.gameObject.name);
        pour = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //print(collision.gameObject.name);
        pour = false;
    }

    IEnumerator PourWine()
    {
        while (true)
        {
            
            if (pour)
            {
                GameObject newDrop = Instantiate(wine);
                newDrop.transform.position = spout.transform.position;

                if (_particleSystem != null && !_particleSystem.isPlaying)
                {
                    _particleSystem.Play();
                }
            }
            else
            {
                if (_particleSystem != null && _particleSystem.isPlaying)
                {
                    _particleSystem.Stop();
                }
            }
            yield return new WaitForSeconds(0.005f);
        }
    }

    public void StartPour()
    {
        pour = true;
        GetComponent<AudioSource>().Play();
    }

    public void StopPour()
    {
        pour = false;
        GetComponent<AudioSource>().Stop();
    }
}
