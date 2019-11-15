using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Juicer : MonoBehaviour
{
    public CircularDrive handle;
    public GameObject handleObj;
    public GameObject spout;
    public GameObject squisher;


    private bool full;
    private float juiceLeft;
    private GameObject juiceableObject;
    private GameObject juiceDrop;

    private float SaveAngle;

    private AudioSource juiceSound;

    // Start is called before the first frame update
    void Start()
    {
        SaveAngle = handle.outAngle;

        juiceSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handle.outAngle < SaveAngle && full)
        {
            
            Debug.Log("handle angle: " + handle.outAngle);
            GameObject newDrop = Instantiate(juiceDrop);
            newDrop.transform.position = spout.transform.position;
            Debug.Log("before: " + SaveAngle);
            Debug.Log("subtraction: " + (90.0f / juiceableObject.GetComponent<Juiceable>().jucieUnits));
            SaveAngle -= 90.0f / juiceableObject.GetComponent<Juiceable>().jucieUnits;
            Debug.Log("after: " + SaveAngle);
            juiceLeft--;
            AudioManager.S.PlaySound(juiceSound);
        }

        float scaleMove = ((handle.outAngle / -90) * .84f);


        squisher.transform.localScale = new Vector3(squisher.transform.localScale.x, .16f + scaleMove, squisher.transform.localScale.z);

        if (juiceLeft <= 0)
        {
            full = false;
            SaveAngle = 0;
            juiceLeft = 0;
            if (juiceableObject != null)
            {
                Destroy(juiceableObject);
                juiceableObject = null;

            }


        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Juiceable")
        {   
            

            if (juiceableObject != null)
            {

                Destroy(juiceableObject);
                juiceableObject = null;
            }
            juiceableObject = other.gameObject;
            LoadJuicer();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Juiceable")
        {
            
            juiceableObject = null;
            unloadJuicer();

        }
    }


    public void LoadJuicer()
    {
        handle.outAngle = 0f;
        handleObj.transform.localEulerAngles = Vector3.zero;
        SaveAngle = 0;
        full = true;
        juiceLeft = juiceableObject.GetComponent<Juiceable>().jucieUnits;
        juiceDrop = juiceableObject.GetComponent<Juiceable>().juiceDrop;
    }

    public void unloadJuicer()
    {
        full = false;
        juiceLeft = 0;
        juiceDrop = null;
    }

    public bool IsFull()
    {
        return full;
    }


}
