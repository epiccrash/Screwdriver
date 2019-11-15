using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLineConnector : MonoBehaviour
{
    private Transform other;
    private LineRenderer line;

    // Start is called before the first frame update
    void Awake()
    {
        other = GetComponent<SpringJoint>().connectedBody.transform;

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        line.SetPositions(new [] {transform.position, other.position });
    }
}
