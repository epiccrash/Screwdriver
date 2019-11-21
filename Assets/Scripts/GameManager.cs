using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    StartMenu,
    NormalRound,
    LightningRound,
    GameOver
}

[Singleton(SingletonAttribute.Type.LoadedFromResources, true, "GameManager")]
public class GameManager : Singleton<GameManager>
{
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;
    public UnityEvent OnLightningRoundStart;

    [SerializeField]
    private float _regularGameLength;

    [SerializeField]
    private float _lightningRoundLength;

    private GameState _state;
    private float _currentPhaseTime;

    protected override void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        ChangeState(GameState.StartMenu);
        return;
    }

    private void Update()
    {
        if (_state == GameState.StartMenu)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ChangeState(GameState.NormalRound);
            }
        }
        else if (_state == GameState.NormalRound)
        {
            _currentPhaseTime += Time.deltaTime;

            if (_currentPhaseTime >= _regularGameLength)
            {
                ChangeState(GameState.LightningRound);
            }
        }
        else if (_state == GameState.LightningRound)
        {
            _currentPhaseTime += Time.deltaTime;

            if (_currentPhaseTime >= _lightningRoundLength)
            {
                ChangeState(GameState.GameOver);
            }
        }
    }

    private void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.StartMenu:
                print("Moved to start menu");
                break;
            case GameState.NormalRound:
                print("Playing game!");
                OnGameStart.Invoke();
                _currentPhaseTime = 0;
                break;
            case GameState.LightningRound:
                print("LIGHTNING ROUND!!!!!!");
                OnLightningRoundStart.Invoke();
                _currentPhaseTime = 0;
                break;
            case GameState.GameOver:
                OnGameOver.Invoke();
                print("Game Over");
                break;
            default:
                break;
        }

        _state = newState;
    }
}
