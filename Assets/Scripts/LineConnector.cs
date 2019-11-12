using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    [Header("Cable Parent Object")]
    [SerializeField] Transform cable;

    private LineRenderer basicLineRenderer;
    private Vector3[] positions;

    // Start is called before the first frame update
    private void Start()
    {
        basicLineRenderer = GetComponent<LineRenderer>();

        positions = new Vector3[cable.childCount];

        for (int i = 0; i < cable.childCount - 1; i++)
        {
            /*
            GameObject child = cable.GetChild(i).gameObject;
            child.AddComponent(typeof(LineRenderer));
            LineRenderer childLineRenderer = child.GetComponent<LineRenderer>();
            childLineRenderer.material = basicLineRenderer.material;
            childLineRenderer.startWidth = basicLineRenderer.startWidth;
            childLineRenderer.endWidth = basicLineRenderer.endWidth;
            childLineRenderer.startColor = new Color(0, 0, 0);
            childLineRenderer.endColor = new Color(0, 0, 0);
            Vector3[] vArray = new[] { cable.GetChild(i).position, cable.GetChild(i + 1).position };
            childLineRenderer.SetPositions(vArray);
            */

            positions[i] = cable.GetChild(i).position;
        }

        positions[cable.childCount - 1] = cable.GetChild(cable.childCount - 1).position;

        basicLineRenderer.positionCount = cable.childCount;
        basicLineRenderer.SetPositions(positions);
    }

    void Update()
    {

        for (int i = 0; i < cable.childCount - 1; i++)
        {
            /*
            LineRenderer childLineRenderer = cable.GetChild(i).gameObject.GetComponent<LineRenderer>();
            Vector3[] vArray = new[] { cable.GetChild(i).position, cable.GetChild(i + 1).position };
            childLineRenderer.SetPositions(vArray);
            */

            positions[i] = cable.GetChild(i).position;
        }

        positions[cable.childCount - 1] = cable.GetChild(cable.childCount - 1).position;

        //print(positions.Length);
        basicLineRenderer.SetPositions(positions);
    }
}
