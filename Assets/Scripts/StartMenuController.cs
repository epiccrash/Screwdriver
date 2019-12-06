using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Singleton(SingletonAttribute.Type.ExistsInScene)]
public class StartMenuController : Singleton<StartMenuController>
{
    [SerializeField]
    private LiquidFillButton _playButton;

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

    public void SetPlayButtonCallback(Delegates.onButtonPressedDel del)
    {
        _playButton.SetOnButtonFilled(del);
    }
}
