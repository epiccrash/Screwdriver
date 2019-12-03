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

    private bool movedDown = false;
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

            GameObject newDrop = Instantiate(juiceDrop);
            newDrop.transform.position = spout.transform.position;
            SaveAngle -= 90.0f / juiceableObject.GetComponent<Juiceable>().jucieUnits;
            juiceLeft--;
            AudioManager.S.PlaySound(juiceSound);
        }

        float scaleMove = ((handle.outAngle / -90) * .84f);


        squisher.transform.localScale = new Vector3(squisher.transform.localScale.x, .16f + scaleMove, squisher.transform.localScale.z);

        if (-0.074f - scaleMove / 4 < transform.localPosition.x || scaleMove == 0)
        {

            // Debug.Log("new location: " + -0.074f + scaleMove / 4);
            // Debug.Log("old location: " + transform.localPosition.x);
            // Debug.Log("Movement: " + scaleMove);
            if (scaleMove > 0)
            {
                movedDown = true;
            }
            if (movedDown == true && scaleMove == 0)
            {
                movedDown = false;
                unloadJuicer();
            }
            transform.localPosition = new Vector3(-0.074f - scaleMove / 4, transform.localPosition.y, transform.localPosition.z);
        }


        if (juiceLeft <= 0)
        {
            full = false;
            SaveAngle = 0;
            juiceLeft = 0;
            if (juiceableObject != null)
            {
                juiceableObject.GetComponent<Juiceable>().LoadFruit();
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
                juiceableObject.GetComponent<Juiceable>().LoadFruit();
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
        //juiceableObject.GetComponent<Juiceable>().LoadFruit();
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
