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

    private bool movedDown=false;
    private bool full;
    private float juiceLeft;
    private GameObject juiceableObject;
    private GameObject juiceDrop;

    private float SaveAngle;

    private AudioSource juiceSound;

    private float SQUISHSTART = .145f, SQUISHSTOP = .178f, SQUISHPERC=.8f, SNAPSTART=.95f;

    
    // Start is called before the first frame update
    void Start()
    {
        SaveAngle = handle.outAngle;

        juiceSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handle.outAngle > SaveAngle && full)
        {
            
            GameObject newDrop = Instantiate(juiceDrop);
            newDrop.transform.position = spout.transform.position;
            SaveAngle += 90.0f / juiceableObject.GetComponent<Juiceable>().jucieUnits;
            juiceLeft--;
            AudioManager.S.PlaySound(juiceSound);
        }


        float scaleMove = ((handle.outAngle / -90) * (SQUISHSTOP-SQUISHSTART));

        
        squisher.transform.localPosition = new Vector3(squisher.transform.localPosition.x, SQUISHSTART + scaleMove, squisher.transform.localPosition.z);
        
        if ((SQUISHSTART + scaleMove < .17 && transform.localScale.y>1-SQUISHPERC)) {

            float snapMove = (handle.outAngle / -90) * (SNAPSTART * .2f);
            float snapScale = (handle.outAngle / -90) * SQUISHPERC;


            transform.localPosition = new Vector3(transform.localPosition.x, SNAPSTART+snapMove, transform.localPosition.z);
            transform.localScale = new Vector3(transform.localScale.x, snapScale, transform.localScale.z);

            if ( scaleMove > 0) {
                movedDown = true;
            }
            
            if (movedDown == true && scaleMove == 0) {
                movedDown = false;
                unloadJuicer();
            }

            
            transform.localPosition = new Vector3(transform.localPosition.x, .077f + scaleMove, transform.localPosition.z);
            
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
        handle.outAngle = -90f;
        handleObj.transform.localEulerAngles = Vector3.zero;
        SaveAngle = -90;
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
