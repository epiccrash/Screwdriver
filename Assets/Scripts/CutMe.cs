using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutMe : MonoBehaviour
{
    public GameObject cutPieces;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag=="Knife") {
            GameObject piece = Instantiate(cutPieces);
            piece.transform.position = this.transform.position;
            piece.transform.position = new Vector3(piece.transform.position.x - piece.GetComponent<Collider>().bounds.extents.x, piece.transform.position.y, piece.transform.position.z);
            piece = Instantiate(cutPieces);
            piece.transform.position = this.transform.position;
            piece.transform.position = new Vector3(piece.transform.position.x + piece.GetComponent<Collider>().bounds.extents.x, piece.transform.position.y, piece.transform.position.z);

        }
    }
}
