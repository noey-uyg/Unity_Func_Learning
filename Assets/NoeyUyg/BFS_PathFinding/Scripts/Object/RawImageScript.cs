using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RawImageScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private RawImage _image;

    private RectTransform _rectTransform;
    private RectSize _rectSize;
    private Pos _pos;

    public RectTransform GetRectTransform { get { return _rectTransform; } }
    public Pos Pos { get { return _pos; } }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // �̹��� ũ�� ����
    public void SetRectSize(float x , float y, int originX, int originY)
    {
        _rectSize = new RectSize(x, y);
        _pos = new Pos(originX, originY);
        _rectTransform.sizeDelta = new Vector2(x, y);
    }

    // �� Ÿ�� ����
    public void SetRoadType(RoadType roadType)
    {
        _pos.roadType = roadType;
    }

    // �÷� ����
    public void SetColor(Color color)
    {
        _image.color = color;
    }

    // �ؽ�Ʈ ����
    public void SetText(string text)
    {
        _text.text = text;
    }
}
