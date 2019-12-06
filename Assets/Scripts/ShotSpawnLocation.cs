using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawnLocation : MonoBehaviour
{
    private const float CupFallStep = 0.05f;
    private const float FallEpsilon = 0.001f;
    private const float FullCupPercentageEpsilon = 0.02f;
    private const float CupFallAdjustment = 0.04f;

    [SerializeField]
    private GameObject _shotGlassPrefab;

    [SerializeField]
    private float _desiredFillAmount;

    [SerializeField]
    private float _tipAmount;

    [SerializeField]
    private RadialProgressBar _progressRing;

    [SerializeField]
    private ParticleSystem _cupSpawnEffect;

    private CupScript _currentCup;

    private bool _isWaitingForShot = true;
    public bool isWaitingForShot => _isWaitingForShot;

    private bool _isGameOver = false;
    private LayerMask _counterLayerMask;

    private void Awake()
    {
        GameManager.Instance.OnGameOver.AddListener(OnGameOver);
        _counterLayerMask = LayerMask.GetMask("StaticFurniture");
    }

    private void OnGameOver()
    {
        _progressRing.gameObject.SetActive(false);
        _isGameOver = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_isGameOver && _currentCup != null)
        {
            float amt = _currentCup.GetTotalAlcohol();
            float percentage = amt / _desiredFillAmount;

            _progressRing.SetIngredientAmount(Mathf.Min(1, percentage));

            if (percentage >= 1 || 1 - percentage <= FullCupPercentageEpsilon)
            {
                TipScript.Instance.AddTip(_tipAmount);

                Destroy(_currentCup.gameObject);
                _currentCup = null;
                _isWaitingForShot = true;
                _progressRing.gameObject.SetActive(false);

                GameManager.Instance.gameData.totalShots++;
            }
        }
    }

    public void SpawnNewGlass()
    {
        if (_currentCup == null)
        {
            GameObject newCup = Instantiate(_shotGlassPrefab, this.transform);
            newCup.transform.localPosition = new Vector3(0, _progressRing.transform.localPosition.y, 0);

            _currentCup = newCup.GetComponent<CupScript>();

            RaycastHit counterHit;

            if (Physics.Raycast(_currentCup.transform.position, Vector3.down, out counterHit, Mathf.Infinity, _counterLayerMask))
            {
                if (counterHit.rigidbody != null)
                {
                    Vector3 fallPosition = counterHit.point;
                    fallPosition.y += CupFallAdjustment;
                    _ = StartCoroutine(DropCup(fallPosition));

                    _isWaitingForShot = false;

                    PlaySpawnEffect();

                    _progressRing.gameObject.SetActive(true);
                    _progressRing.InitializeForNewIngredient(IngredientType.Vodka);
                }
            }
            else
            {
                Debug.LogError("Shot glass could not find the bar counter to fall onto. Bar/counter might not be on the 'StaticFurniture' layer.");
                Destroy(_currentCup.gameObject);
                _currentCup = null;
            }
        }
    }

    private void PlaySpawnEffect()
    {
        ParticleSystem effect = Instantiate(_cupSpawnEffect, _progressRing.transform);
        Destroy(effect, effect.main.duration);
    }

    IEnumerator DropCup(Vector3 targetPos)
    {
        // While we haven't fallen to the counter...
        while (_currentCup != null && Vector3.Distance(_currentCup.transform.position, targetPos) > FallEpsilon)
        {
            _currentCup.transform.position = Vector3.MoveTowards(_currentCup.transform.position, targetPos, CupFallStep);
            yield return null;
        }
    }
}
