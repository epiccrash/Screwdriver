using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.ExistsInScene)]
public class ShotManager : Singleton<ShotManager>
{
    private List<ShotSpawnLocation> _spawners;

    protected override void Awake()
    {
        // Get all our spawn locations.
        _spawners = new List<ShotSpawnLocation>(GetComponentsInChildren<ShotSpawnLocation>());

        foreach (ShotSpawnLocation spawn in _spawners)
        {
            spawn.gameObject.SetActive(false);
        }
    }

    public override void Initialize()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine("SpawnFirstRow");
        }
    }

    IEnumerator SpawnFirstRow()
    {
        foreach (ShotSpawnLocation spawn in _spawners)
        {
            spawn.gameObject.SetActive(true);
            spawn.SpawnNewGlass();
            yield return new WaitForSeconds(0.15f);
        }
    }
}
