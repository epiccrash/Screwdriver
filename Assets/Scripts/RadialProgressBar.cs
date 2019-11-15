using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadialProgressBar : MonoBehaviour
{
    private const float LerpSpeed = 10;

    // Treat these like constants, DO NOT CHANGE
    private static Color32 IncorrectAmtColor = new Color32(235, 158, 52, 255);
    private static Color32 CorrectAmtColor = new Color32(28, 184, 28, 255);

    [SerializeField]
    private TextMeshProUGUI _ingredientText;

    [SerializeField]
    private TextMeshProUGUI _tooMuchText;

    [SerializeField]
    private Image _progressBar;

    private float _progressPercentage;

    private void Update()
    {
        if (!Mathf.Approximately(Mathf.Min(1, _progressPercentage), _progressBar.fillAmount))
        {
            _progressBar.fillAmount = Mathf.Lerp(_progressBar.fillAmount, Mathf.Min(1, _progressPercentage), Time.deltaTime * LerpSpeed);
        }
    }

    public void InitializeForNewIngredient(IngredientType ingredient)
    {
        _progressBar.fillAmount = 0;
        _progressBar.color = IncorrectAmtColor;

        _ingredientText.text = ingredient.ToString();
        _tooMuchText.enabled = false;
    }

    public void SetIngredientAmount(float percentage)
    {
        _progressPercentage = percentage;

        if (_progressPercentage <= 1)
        {
            _tooMuchText.enabled = false;

            if (Mathf.Abs(_progressPercentage - 1) <= GameConstants.DrinkPerfectionPercentageEpsilon)
            {
                _progressBar.color = CorrectAmtColor;
            }
            else
            {
                _progressBar.color = IncorrectAmtColor;
            }
        }
        else if (_progressPercentage >= 1 + GameConstants.DrinkPerfectionPercentageEpsilon)
        {
            _progressBar.color = IncorrectAmtColor;
            _tooMuchText.enabled = true;
        }
        else
        {
            _progressBar.color = IncorrectAmtColor;
        }
    }
}
