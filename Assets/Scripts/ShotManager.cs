using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.ExistsInScene)]
public class ShotManager : Singleton<ShotManager>
{
    private List<ShotSpawnLocation> _spawners;

    private bool _isTimeForShots = false;

    protected override void Awake()
    {
        // Get all our spawn locations.
        _spawners = new List<ShotSpawnLocation>(GetComponentsInChildren<ShotSpawnLocation>());

        foreach (ShotSpawnLocation spawn in _spawners)
        {
            spawn.gameObject.SetActive(false);
        }

        GameManager.Instance.OnLightningRoundStart.AddListener(OnLightningRoundBegin);
        GameManager.Instance.OnGameOver.AddListener(OnGameOver);
    }

    public override void Initialize()
    {
        return;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isTimeForShots)
        {
            bool shouldRespawn = true;

            foreach (ShotSpawnLocation spawn in _spawners)
            {
                if (!spawn.isWaitingForShot)
                {
                    shouldRespawn = false;
                }
            }

            if (shouldRespawn)
            {
                _ = StartCoroutine("SpawnFirstRow");
            }
        }
    }

    private void OnLightningRoundBegin()
    {
        _isTimeForShots = true;
    }

    private void OnGameOver()
    {
        _isTimeForShots = false;
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
