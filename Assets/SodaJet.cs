using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SodaJet : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any;

    [SerializeField] private Vector2 joystick;
    [SerializeField] private GameObject liquidPrefab;
    [SerializeField] private GameObject liquidPrefab2;
    [SerializeField] private int force = 3000;
    [SerializeField] private float deadZone = 0.5f;

    private GameObject spout;

    private void Start()
    {
        spout = transform.GetChild(0).gameObject;
        StartCoroutine(Squirt());
    }

    // Update is called once per frame
    void Update()
    {
        joystick = SteamVR_Actions.default_Joystick.GetAxis(inputSource);
    }

    IEnumerator Squirt()
    {
        while (true)
        {
            Vector3 direction = spout.transform.position - transform.position;
            direction.Normalize();

            if (joystick.y >= 0.1f)
            {
                GameObject newDrop = Instantiate(liquidPrefab);
                newDrop.transform.position = spout.transform.position;
                newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
            } else if (joystick.y <= -0.1f)
            {
                GameObject newDrop = Instantiate(liquidPrefab2);
                newDrop.transform.position = spout.transform.position;
                newDrop.GetComponent<Rigidbody>().AddForce(direction * force);
            }

            yield return new WaitForSeconds(0.005f);
        }
    }
}
