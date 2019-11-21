using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.LoadedFromResources, true, "GameManager")]
public class GameManager : Singleton<GameManager>
{
    protected override void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        return;
    }
}
