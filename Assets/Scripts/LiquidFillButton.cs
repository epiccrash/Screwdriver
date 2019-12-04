using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LiquidFillButton : MonoBehaviour
{
    private const float DropletsToFill = 15f;
    private const float LerpSpeed = 10f;

    private Delegates.onButtonPressedDel _onButtonFilled;
    private float _currentFillValue;

    [SerializeField]
    private TextMeshProUGUI _buttonText;

    [SerializeField]
    private Image _fillSprite;

    // Start is called before the first frame update
    void Start()
    {
        _currentFillValue = 0;
        _fillSprite.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Mathf.Approximately(Mathf.Min(1, _currentFillValue), _fillSprite.fillAmount))
        {
            _fillSprite.fillAmount = Mathf.Lerp(_fillSprite.fillAmount, Mathf.Min(1, _currentFillValue), Time.deltaTime * LerpSpeed);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        IngredientScript otherIngredient = other.gameObject.GetComponent<IngredientScript>();

        if (otherIngredient != null
            && _currentFillValue < 1
            && (otherIngredient.IngredientType == IngredientType.Cola || otherIngredient.IngredientType == IngredientType.ClubSoda))
        {
            _currentFillValue += 1f / DropletsToFill;

            if (_currentFillValue >= 1)
            {
                _onButtonFilled?.Invoke();
            }
        }
    }
}
