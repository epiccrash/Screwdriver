using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutMe : MonoBehaviour
{
    public GameObject cutPieces;
    public int numPieces = 5;
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

            print("weee");

            KnifeScript knife = collision.transform.GetComponent<KnifeScript>();

            if (knife != null && knife.canCut) {
                knife.canCut = false;
                /*for (int i = 0; i < numPieces; i++)
                {
                    GameObject piece = Instantiate(cutPieces);
                    piece.transform.position = this.transform.position;
                    // This logic needs adjusting
                    piece.transform.position = new Vector3(piece.transform.position.x - piece.GetComponent<Collider>().bounds.extents.x, piece.transform.position.y, piece.transform.position.z);
                }*/
                
                GameObject piece = Instantiate(cutPieces);
                piece.transform.position = this.transform.position;
                piece.transform.position = new Vector3(piece.transform.position.x - piece.GetComponent<Collider>().bounds.extents.x, piece.transform.position.y, piece.transform.position.z);
                piece = Instantiate(cutPieces);
                piece.transform.position = this.transform.position;
                piece.transform.position = new Vector3(piece.transform.position.x + piece.GetComponent<Collider>().bounds.extents.x, piece.transform.position.y, piece.transform.position.z);
                
                // Added this in
                Destroy(this.gameObject);
            }


            

        }
    }
}
