using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Singleton(SingletonAttribute.Type.ExistsInScene)]
public class GameOverMenuController : Singleton<GameOverMenuController>
{
    [SerializeField]
    private LiquidFillButton _replayButton;

    [SerializeField]
    private TextMeshProUGUI _totalDrinksText;

    [SerializeField]
    private TextMeshProUGUI _perfectDrinksText;

    [SerializeField]
    private TextMeshProUGUI _shotsPouredText;

    [SerializeField]
    private TextMeshProUGUI _totalTipsText;

    public override void Initialize()
    {
        Hide();
    }

    public void Show()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void SetReplayButtonCallback(Delegates.onButtonPressedDel del)
    {
        _replayButton.SetOnButtonFilled(del);
    }

    public void ConfigureAndShow()
    {
        GameData data = GameManager.Instance.gameData;

        _totalDrinksText.SetText(data.totalDrinks.ToString());
        _perfectDrinksText.SetText(data.perfectDrinks.ToString());
        _shotsPouredText.SetText(data.totalShots.ToString());
        _totalTipsText.SetText(data.totalTips.ToString("C", CultureInfo.CurrentCulture));

        Show();
    }
}
