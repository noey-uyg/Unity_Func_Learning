using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AppleGameSquare : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numText;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Transform _transform;
    private int _myNum;

    public int Num { get { return _myNum; } }
    public Transform GetTransform { get { return _transform; } }

    public void SetNum(int num)
    {
        _myNum = num;
        _numText.text = num.ToString();
    }

    public void SetRectSize(float x, float y)
    {
        _rectTransform.sizeDelta = new Vector2(x, y);
        _transform.localScale = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y);
    }
}
