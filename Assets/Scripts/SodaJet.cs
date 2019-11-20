using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SodaJet : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources inputSource2 = SteamVR_Input_Sources.RightHand;

    private Vector2 joystickL;
    private Vector2 joystickR;
    private Vector2 currentJoystick;

    private Transform leftHand;
    private Transform rightHand;

    [SerializeField] private GameObject liquidPrefab;
    [SerializeField] private GameObject liquidPrefab2;
    [SerializeField] private int force = 3000;
    [SerializeField] private float deadZone = 0.5f;

    public Transform spoutLocation;
    private bool snapping= false;

    private GameObject spout;

    [SerializeField] Transform joystickObj;
    [SerializeField] GameObject rotationPoint;

    private void Start()
    {
        spout = transform.GetChild(0).gameObject;
        StartCoroutine(Squirt());

        leftHand = GameObject.Find("LeftHand").transform;
        rightHand = GameObject.Find("RightHand").transform;
    }

    // Update is called once per frame
    void Update()
    {
        joystickL = SteamVR_Actions._default.Joystick.GetAxis(inputSource);
        joystickR = SteamVR_Actions._default.Joystick.GetAxis(inputSource2);
        if (snapping) {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = spoutLocation.position;
            transform.rotation = spoutLocation.rotation;
            snapping = false;
        }
        if (transform.position == spoutLocation.position) {
            snapping = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
        //joystickL = new Vector2(Input.GetAxis("JoystickLeftH"), Input.GetAxis("JoystickLeftV"));
        //joystickR = new Vector2(Input.GetAxis("JoystickLeftH"), Input.GetAxis("JoystickLeftV"));

        if (transform.parent == leftHand) {
            currentJoystick = joystickL;
        } else if (transform.parent == rightHand) {
            currentJoystick = joystickR;
        } else {
            currentJoystick = Vector2.zero;
        }

        if (currentJoystick == joystickL || currentJoystick == joystickR) {
            // joystickObj.RotateAround(rotationPoint, Vector3.forward, Time.deltaTime);
            transform.eulerAngles = new Vector3(currentJoystick.x * 30, 0.0f, currentJoystick.y * 30);
        }
    }

    IEnumerator Squirt()
    {
        while (true)
        {
            if (transform.parent != null)
            {
                print(transform.parent == rightHand);
                if ((transform.parent == leftHand && joystickL.y >= 0.1f) ||
                    (transform.parent == rightHand && joystickR.y >= 0.1f))
                {
                    CreateDrop(liquidPrefab);
                }
                else if ((transform.parent == leftHand && joystickL.y <= -0.1f) ||
                         (transform.parent == rightHand && joystickR.y <= -0.1f))
                {
                    CreateDrop(liquidPrefab2);
                }
            }

            yield return new WaitForSeconds(0.005f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SodaSnap") {
            snapping = true;
        }
    }

    private void CreateDrop(GameObject liquid) {
        
        Vector3 direction = spout.transform.up + new Vector3(0, 0.5f, 0);
        direction.Normalize();

        GameObject newDrop = Instantiate(liquid);
        newDrop.transform.position = spout.transform.position;
        newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
    }
}
