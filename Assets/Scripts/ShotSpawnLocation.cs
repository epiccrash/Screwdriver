using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawnLocation : MonoBehaviour
{
    [SerializeField]
    private GameObject _shotGlassPrefab;

    private CupScript _currentCup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnNewGlass()
    {
        GameObject newCup = Instantiate(_shotGlassPrefab, this.transform);
        newCup.transform.localPosition = new Vector3(0, 0.2f, 0);

        //Rigidbody rb = newCup.GetComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.None;

        _currentCup = newCup.GetComponent<CupScript>();
    }
}
