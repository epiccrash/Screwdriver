using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float lifetime;
#pragma warning restore 0649

    private void Start()
    {
        StartCoroutine(killAfterSecs());
    }

    private IEnumerator killAfterSecs()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
