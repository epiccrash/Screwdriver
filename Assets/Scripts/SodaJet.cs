using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SodaJet : MonoBehaviour
{
    private const float DropletSize = 0.02f;
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
    private bool snapping = false;

    [SerializeField]
    private GameObject spout;

    [SerializeField] Transform joystickObj;
    [SerializeField] GameObject rotationPoint;

    private void Start()
    {
        // GetComponent<Rigidbody>().isKinematic = true;

        // spout = transform.GetChild(0).gameObject;
        StartCoroutine(Squirt());

        GameObject leftHandObj = GameObject.Find("LeftHand");
        if (leftHandObj != null)
        {
            leftHand = leftHandObj.transform;
        }

        GameObject rightHandObj = GameObject.Find("RightHand");
        if (rightHandObj != null)
        {
            rightHand = rightHandObj.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        joystickL = SteamVR_Actions._default.Joystick.GetAxis(inputSource);
        joystickR = SteamVR_Actions._default.Joystick.GetAxis(inputSource2);
        if (snapping)
        {
            transform.position = spoutLocation.position;
            transform.rotation = Quaternion.identity;
            snapping = false;
        }
        if (transform.position == spoutLocation.position)
        {
            snapping = false;
            // GetComponent<Rigidbody>().isKinematic = false;
            // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        } else if (transform.parent == leftHand || transform.parent == rightHand)
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
        //joystickL = new Vector2(Input.GetAxis("JoystickLeftH"), Input.GetAxis("JoystickLeftV"));
        //joystickR = new Vector2(Input.GetAxis("JoystickLeftH"), Input.GetAxis("JoystickLeftV"));

        if (transform.parent == leftHand)
        {
            currentJoystick = joystickL;
        } else if (transform.parent == rightHand)
        {
            currentJoystick = joystickR;
        } else
        {
            currentJoystick = Vector2.zero;
        }

        if (currentJoystick == joystickL || currentJoystick == joystickR)
        {
            // joystickObj.RotateAround(rotationPoint, Vector3.forward, Time.deltaTime);
            // transform.eulerAngles = new Vector3(currentJoystick.x * 30, 0.0f, currentJoystick.y * 30);
        }
    }

    IEnumerator Squirt()
    {
        while (true)
        {
            if (transform.parent != null)
            {
                if ((transform.parent == leftHand && joystickL.y >= 0.1f) ||
                    (transform.parent == rightHand && joystickR.y >= 0.1f) || Input.GetKey(KeyCode.K))
                {
                    CreateDrop(liquidPrefab);
                }
                else if ((transform.parent == leftHand && joystickL.y <= -0.1f) ||
                         (transform.parent == rightHand && joystickR.y <= -0.1f) || Input.GetKey(KeyCode.L))
                {
                    CreateDrop(liquidPrefab2);
                }
            }

            yield return new WaitForSeconds(0.005f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SodaSnap")
        {
            snapping = true;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "SodaSnap")
        {
            snapping = false;
        }
    }

    private void CreateDrop(GameObject liquid)
    {
        
        Vector3 direction = spout.transform.up + new Vector3(0, 0.5f, 0);
        direction.Normalize();

        GameObject newDrop = Instantiate(liquid);

        newDrop.transform.localScale = new Vector3(DropletSize, DropletSize, DropletSize);
        newDrop.GetComponent<MeshRenderer>().enabled = false;

        newDrop.GetComponent<TrailRenderer>().enabled = true;

        newDrop.transform.position = spout.transform.position;
        newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
    }
}
