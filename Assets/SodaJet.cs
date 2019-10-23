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

    private GameObject leftHand;
    private GameObject rightHand;

    [SerializeField] private GameObject liquidPrefab;
    [SerializeField] private GameObject liquidPrefab2;
    [SerializeField] private int force = 3000;
    [SerializeField] private float deadZone = 0.5f;

    private GameObject spout;

    private void Start()
    {
        spout = transform.GetChild(0).gameObject;
        StartCoroutine(Squirt());

        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");
        joystickL = SteamVR_Actions._default.Joystick.GetAxis(inputSource);
    }

    // Update is called once per frame
    void Update()
    {
        joystickL = SteamVR_Actions._default.Joystick.GetAxis(inputSource);
        joystickR = SteamVR_Actions._default.Joystick.GetAxis(inputSource2);
        //joystickL = new Vector2(Input.GetAxis("JoystickLeftH"), Input.GetAxis("JoystickLeftV"));
        //joystickR = new Vector2(Input.GetAxis("JoystickLeftH"), Input.GetAxis("JoystickLeftV"));
    }

    IEnumerator Squirt()
    {
        while (true)
        {
            Vector3 direction = spout.transform.position - transform.position;
            direction.Normalize();

            if (transform.parent != null)
            {
                print(transform.parent);
                if ((transform.parent == leftHand && joystickL.y >= 0.1f) ||
                    (transform.parent == rightHand && joystickR.y >= 0.1f))
                {
                    GameObject newDrop = Instantiate(liquidPrefab);
                    newDrop.transform.position = spout.transform.position;
                    newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
                }
                else if ((transform.parent == leftHand && joystickL.y <= -0.1f) ||
                         (transform.parent == rightHand && joystickR.y <= -0.1f))
                {
                    GameObject newDrop = Instantiate(liquidPrefab2);
                    newDrop.transform.position = spout.transform.position;
                    newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
                }
            }

            yield return new WaitForSeconds(0.005f);
        }
    }
}
