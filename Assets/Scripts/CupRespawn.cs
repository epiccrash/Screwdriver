using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupRespawn : MonoBehaviour
{
    public List<GameObject> cupPositions;

    public GameObject cupObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < cupPositions.Count - 1; i++)
        {
            GameObject antPos = cupPositions[i];

            if (cupPositions[i].transform.childCount == 0 && cupPositions[i + 1].transform.childCount > 0)
            {

                cupPositions[i + 1].transform.GetChild(0).parent = cupPositions[i].transform;


            }
            if (cupPositions[i].transform.childCount > 0)
            {
                GameObject childAnt = cupPositions[i].transform.GetChild(0).gameObject;
                childAnt.transform.position = Vector3.Slerp(childAnt.transform.position, antPos.transform.position, .1f);
                Vector3 newQuats = new Vector3(antPos.transform.rotation.eulerAngles.z, antPos.transform.rotation.eulerAngles.y - 90, antPos.transform.rotation.eulerAngles.x);

                childAnt.transform.rotation = Quaternion.Slerp(childAnt.transform.rotation, Quaternion.Euler(newQuats), .1f);
            }



        }


        if (cupPositions[cupPositions.Count - 1].transform.childCount == 0)
        {
            GameObject newAnt = Instantiate(cupObject);
            newAnt.transform.parent = cupPositions[cupPositions.Count - 1].transform;
            newAnt.transform.position = cupPositions[cupPositions.Count - 1].transform.position;
        }

    }


}
