using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Juicer : MonoBehaviour
{
    public CircularDrive handle;
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
            GameObject newDrop = Instantiate(juiceDrop);
            newDrop.transform.position = spout.transform.position;
            SaveAngle -= 90 / juiceableObject.GetComponent<Juiceable>().jucieUnits;
            juiceLeft--;
            AudioManager.S.PlaySound(juiceSound);
        }

        float scaleMove = ((handle.outAngle / -90) * .84f);


        squisher.transform.localScale = new Vector3(squisher.transform.localScale.x, .16f + scaleMove, squisher.transform.localScale.z);

        if (juiceLeft <= 0)
        {
            full = false;
            SaveAngle = 0;
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


    public void LoadJuicer()
    {
        full = true;
        juiceLeft = juiceableObject.GetComponent<Juiceable>().jucieUnits;
        juiceDrop = juiceableObject.GetComponent<Juiceable>().juiceDrop;
    }

    public bool IsFull()
    {
        return full;
    }


}
