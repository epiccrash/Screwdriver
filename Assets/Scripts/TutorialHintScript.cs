using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialHintScript : MonoBehaviour
{
    private const float FloatDistance = 0.08f;
    private const float LerpSpeed = 2f;

    [SerializeField]
    private TextMeshProUGUI _hintText;

    [SerializeField]
    private GameObject _downArrow;

    [SerializeField]
    private Color _startColor;

    [SerializeField]
    private Color _endColor;

    private Vector3 _startPos;

    private Vector3 _endPos;

    private bool _isMovingDown;
    private float _moveTime;
    private Image _downArrowImg;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = _downArrow.transform.position;
        _endPos = new Vector3(_startPos.x, _startPos.y - FloatDistance, _startPos.z);
        _downArrowImg = _downArrow.GetComponent<Image>();

        _isMovingDown = true;
        _moveTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMovingDown)
        {
            _moveTime += Time.deltaTime;
            _downArrow.transform.position = Vector3.Lerp(_startPos, _endPos, _moveTime);

            if (_downArrow.transform.position == _endPos)
            {
                _isMovingDown = false;
                _moveTime = 0;
            }
        }
        else
        {
            _moveTime += Time.deltaTime;
            _downArrow.transform.position = Vector3.Lerp(_endPos, _startPos, _moveTime);

            if (_downArrow.transform.position == _startPos)
            {
                _isMovingDown = true;
                _moveTime = 0;
            }
        }

        _hintText.color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(Time.time, 1));
        _downArrowImg.color = Color.Lerp(_startColor, _endColor, Mathf.PingPong(Time.time, 1));
    }

    public void Show()
    {
        _hintText.enabled = true;
        _downArrow.SetActive(true);
    }

    public void Hide()
    {
        _hintText.enabled = false;
        _downArrow.SetActive(false);
    }

    public void SetText(string hintText)
    {
        _hintText.text = hintText;
    }
}
