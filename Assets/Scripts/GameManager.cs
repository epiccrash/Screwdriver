using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    StartMenu,
    Tutorial,
    PreNormalRound,
    NormalRound,
    PreLightningRound,
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
    
    // When to play sounds; these need to be numbers less than 1 to get an actual duration
    [SerializeField]
    float[] roundSoundIntervals = new float[3] { 0.2f, 0.5f, 0.8f };
    [SerializeField]
    float lightningSoundInterval = 0.5f;

    // Audio bools
    private bool firstIntervalElapsed;
    private bool secondIntervalElapsed;
    private bool thirdIntervalElapsed;
    private bool lightningRoundIntervalElapsed;

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
                // ChangeState(GameState.NormalRound);
                ChangeState(GameState.PreNormalRound);
            }
        }
        else if (_state == GameState.PreNormalRound)
        {
            if (!AudioManager.S.DJIsSpeaking())
            {
                ChangeState(GameState.NormalRound);
            }
        }
        else if (_state == GameState.NormalRound)
        {
            _currentPhaseTime += Time.deltaTime;

            if (_currentPhaseTime >= _regularGameLength)
            {
                // ChangeState(GameState.LightningRound);
                ChangeState(GameState.PreLightningRound);
            }
            else if (!firstIntervalElapsed && _currentPhaseTime >= _regularGameLength * roundSoundIntervals[0])
            {
                AudioManager.S.PlayBarIsCooking();
                firstIntervalElapsed = true;
            }
            else if (!secondIntervalElapsed && _currentPhaseTime >= _regularGameLength * roundSoundIntervals[1])
            {
                AudioManager.S.PlayTryingToGetLow();
                secondIntervalElapsed = true;
            }
            else if (!thirdIntervalElapsed && _currentPhaseTime >= _regularGameLength * roundSoundIntervals[2])
            {
                AudioManager.S.PlayUhOh();
                thirdIntervalElapsed = true;
            }
        }
        else if (_state == GameState.PreLightningRound)
        {
            if (!AudioManager.S.DJIsSpeaking())
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
            else if (!lightningRoundIntervalElapsed && _currentPhaseTime >= _lightningRoundLength * lightningSoundInterval)
            {
                AudioManager.S.PlayLastCall();
                lightningRoundIntervalElapsed = true;
            }
        }
    }

    private void ChangeState(GameState newState)
    {
        switch (newState)
        {
            case GameState.StartMenu:
                print("Moved to start menu");
                StartMenuController.Instance.Show();
                StartMenuController.Instance.SetPlayButtonCallback(StartTutorial);
                break;
            case GameState.PreNormalRound:
                print("Moved to pre-normal round");
                AudioManager.S.PlayGetPartyStarted();
                break;
            case GameState.NormalRound:
                print("Playing game!");
                StartMenuController.Instance.Hide();
                OnGameStart.Invoke();
                _currentPhaseTime = 0;
                AudioManager.S.PlayMashup();
                break;
            case GameState.PreLightningRound:
                print("Moved to pre-lightning round");
                AudioManager.S.PlayTimeForShots();
                break;
            case GameState.LightningRound:
                print("LIGHTNING ROUND!!!!!!");
                OnLightningRoundStart.Invoke();
                AudioManager.S.PlayShots();
                _currentPhaseTime = 0;
                break;
            case GameState.GameOver:
                OnGameOver.Invoke();
                print("Game Over");
                AudioManager.S.PlayMashup();
                break;
            default:
                break;
        }

        _state = newState;
    }

    private void StartTutorial()
    {
        // TODO inf: NEEDS TO BE UPDATED WHEN TUTORIAL IS IMPLEMENTED.
        ChangeState(GameState.NormalRound);
    }
}
