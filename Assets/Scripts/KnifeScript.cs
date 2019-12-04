using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{

    public bool canCut = true;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(ResetCut());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    //private IEnumerator ResetCut() {

    //    while (true) {
    //        canCut = true;
    //        yield return new WaitForSeconds(1);

    //    }

        
    //}

    

}
