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
    public UnityEvent OnTutorialStart;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;
    public UnityEvent OnLightningRoundStart;

    [SerializeField]
    private GameData _gameData;
    public GameData gameData => _gameData;

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

    // Tutorial Hint References.
    private GameObject _tutorialHintIce;
    private GameObject _tutorialHintOj;
    private GameObject _tutorialHintVodka;
    private GameObject _tutorialHintKnife;
    private GameObject _tutorialHintServe;

    protected override void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        // Get ice tutorial hint.
        _tutorialHintIce = GameObject.Find("TutorialHintIce");

        if (_tutorialHintIce == null)
        {
            Debug.LogError("Could not find game object called 'TutorialHintIce' in scene.");
        }
        else
        {
            _tutorialHintIce.SetActive(false);
        }

        // Get OJ tutorial hint.
        _tutorialHintOj = GameObject.Find("TutorialHintOj");

        if (_tutorialHintOj == null)
        {
            Debug.LogError("Could not find game object called 'TutorialHintOj' in scene.");
        }
        else
        {
            _tutorialHintOj.SetActive(false);
        }

        // Get vodka tutorial hint.
        _tutorialHintVodka = GameObject.Find("TutorialHintVodka");

        if (_tutorialHintVodka == null)
        {
            Debug.LogError("Could not find game object called 'TutorialHintVodka' in scene.");
        }
        else
        {
            _tutorialHintVodka.SetActive(false);
        }

        // Get knife tutorial hint.
        _tutorialHintKnife = GameObject.Find("TutorialHintKnife");

        if (_tutorialHintKnife == null)
        {
            Debug.LogError("Could not find game object called 'TutorialHintKnife' in scene.");
        }
        else
        {
            _tutorialHintKnife.SetActive(false);
        }

        // Get serve tutorial hint.
        _tutorialHintServe = GameObject.Find("TutorialHintServe");

        if (_tutorialHintServe == null)
        {
            Debug.LogError("Could not find game object called 'TutorialHintServe' in scene.");
        }
        else
        {
            _tutorialHintServe.SetActive(false);
        }

        ChangeState(GameState.StartMenu);
        return;
    }

    private void Update()
    {
        if (_state == GameState.StartMenu)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ChangeState(GameState.PreNormalRound);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                ChangeState(GameState.Tutorial);
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
                GameOverMenuController.Instance.Hide();
                _gameData.Reset();
                StartMenuController.Instance.Show();
                StartMenuController.Instance.SetPlayButtonCallback(StartTutorial);
                break;
            case GameState.Tutorial:
                print("Starting tutorial.");
                StartMenuController.Instance.Hide();
                GameOverMenuController.Instance.Hide();
                OnTutorialStart.Invoke();
                _gameData.Reset();
                break;
            case GameState.PreNormalRound:
                print("Moved to pre-normal round");
                StartMenuController.Instance.Hide();
                _gameData.Reset();
                GameOverMenuController.Instance.Hide();
                AudioManager.S.PlayGetPartyStarted();
                break;
            case GameState.NormalRound:
                print("Playing game!");
                HideAllTutorialHints();
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
                GameOverMenuController.Instance.SetReplayButtonCallback(Replay);
                GameOverMenuController.Instance.ConfigureAndShow();
                AudioManager.S.PlayMashup();
                break;
            default:
                break;
        }

        _state = newState;
    }

    private void Replay()
    {
        print("Replaying game");
        ChangeState(GameState.PreLightningRound);
    }

    // Functions to manage the tutorial.

    private void StartTutorial()
    {
        ChangeState(GameState.Tutorial);
    }

    public void OnTutorialDrinkServed()
    {
        HideAllTutorialHints();

        ChangeState(GameState.PreNormalRound);
    }

    private void HideAllTutorialHints()
    {
        _tutorialHintIce?.SetActive(false);
        _tutorialHintOj?.SetActive(false);
        _tutorialHintKnife?.SetActive(false);
        _tutorialHintVodka?.SetActive(false);
        _tutorialHintServe?.SetActive(false);
    }

    public void OnTutorialCustomerArrived()
    {
        _tutorialHintIce.SetActive(true);
    }

    public void OnIceAdded()
    {
        if (_state == GameState.Tutorial && _tutorialHintIce.activeInHierarchy)
        {
            _tutorialHintIce.SetActive(false);
            _tutorialHintOj.SetActive(true);
        }
    }

    public void OnOjAdded()
    {
        if (_state == GameState.Tutorial && _tutorialHintOj.activeInHierarchy)
        {
            _tutorialHintOj.SetActive(false);
            _tutorialHintVodka.SetActive(true);
        }
    }

    public void OnVodkaAdded()
    {
        if (_state == GameState.Tutorial && _tutorialHintVodka.activeInHierarchy)
        {
            _tutorialHintVodka.SetActive(false);
            _tutorialHintKnife.SetActive(true);
        }
    }

    public void OnWedgeAdded()
    {
        if (_state == GameState.Tutorial && _tutorialHintKnife.activeInHierarchy)
        {
            _tutorialHintKnife.SetActive(false);
            _tutorialHintServe.SetActive(true);
        }
    }
}
