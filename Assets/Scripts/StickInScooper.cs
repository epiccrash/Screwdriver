using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickInScooper : MonoBehaviour
{
    private AudioSource digInIce;
    private AudioSource iceExit;

    // Start is called before the first frame update
    void Start()
    {
        digInIce = GetComponents<AudioSource>()[0];
        iceExit = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("IceCube")){
            other.gameObject.transform.SetParent(transform.parent);
            AudioManager.S.PlaySound(digInIce);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Equals("IceCube")){
            other.gameObject.transform.SetParent(null);
            AudioManager.S.PlaySound(iceExit);
        }
    }
}
