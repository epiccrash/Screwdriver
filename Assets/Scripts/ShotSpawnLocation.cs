using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawnLocation : MonoBehaviour
{
    [SerializeField]
    private GameObject _shotGlassPrefab;

    [SerializeField]
    private float _desiredFillAmount;

    [SerializeField]
    private float _tipAmount;

    [SerializeField]
    private RadialProgressBar _progressRing;

    private CupScript _currentCup;

    private bool _isWaitingForShot = true;
    public bool isWaitingForShot => _isWaitingForShot;

    private bool _isGameOver = false;

    private void Awake()
    {
        GameManager.Instance.OnGameOver.AddListener(OnGameOver);
    }

    private void OnGameOver()
    {
        _progressRing.gameObject.SetActive(false);
        _isGameOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isGameOver && _currentCup != null)
        {
            float amt = _currentCup.GetIngredientAmount(IngredientType.Vodka);
            float percentage = amt / _desiredFillAmount;

            _progressRing.SetIngredientAmount(Mathf.Min(1, percentage));

            if (percentage >= 1 || 1 - percentage <= 0.02)
            {
                TipScript.Instance.AddTip(_tipAmount);

                Destroy(_currentCup.gameObject);
                _currentCup = null;
                _isWaitingForShot = true;
                _progressRing.gameObject.SetActive(false);
            }
        }
    }

    public void SpawnNewGlass()
    {
        if (_currentCup == null)
        {
            GameObject newCup = Instantiate(_shotGlassPrefab, this.transform);

            Rigidbody rb = newCup.GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ
                | RigidbodyConstraints.FreezeRotation;

            newCup.transform.localPosition = new Vector3(0, 0.1f, 0);

            _currentCup = newCup.GetComponent<CupScript>();
            _isWaitingForShot = false;

            _progressRing.gameObject.SetActive(true);
            _progressRing.InitializeForNewIngredient(IngredientType.Vodka);
        }
    }
}
