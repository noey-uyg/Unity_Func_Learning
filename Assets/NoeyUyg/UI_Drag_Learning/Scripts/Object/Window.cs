using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum InnerColor
{
    White,
    Red,
    Blue,
    Green,
    Gray,
    Yellow,
    LastInstance
}

public class Window : MonoBehaviour, IDragHandler, IBeginDragHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform _dragRect;
    [SerializeField] private RectTransform _top;
    [SerializeField] private RectTransform _inner;
    [SerializeField] private RawImage _innerImage;
    
    private Canvas _mainCanvas;
    private RectTransform _canvasRect;
    private RectTransform _ThisRectTransform;

    private bool drag;

    private void Start()
    {
        _ThisRectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
        _canvasRect = _mainCanvas.GetComponent<RectTransform>();
        SetInnerImageColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _ThisRectTransform.SetAsLastSibling();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        drag = false;

        // top이면 드래그 가능
        if(eventData.pointerEnter == _top.gameObject)
        {
            _ThisRectTransform.SetAsLastSibling();
            drag = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragRect == null || _mainCanvas == null)
            return;

        if (!drag)
            return;

        // 마우스 이동량을 Canvas의 스케일 팩터로 나누어서 움직일 위치 계산
        Vector2 position = _ThisRectTransform.anchoredPosition + eventData.delta / _mainCanvas.scaleFactor;

        // 화면 경계를 벗어나지 않도록 처리
        Vector2 minPosition = _canvasRect.rect.min - new Vector2(_inner.rect.x, _inner.rect.y - _top.rect.y);
        Vector2 maxPosition = _canvasRect.rect.max + new Vector2(_top.rect.x, _top.rect.y);

        // x 좌표 범위 제한
        position.x = Mathf.Clamp(position.x, minPosition.x, maxPosition.x);

        // y 좌표 범위 제한
        position.y = Mathf.Clamp(position.y, minPosition.y, maxPosition.y);

        // 드래그한 위치 적용
        _ThisRectTransform.anchoredPosition = position;
    }

    public void OnClose()
    {
        WindowPool.Instance.ReleaseWindow(this);
    }

    public void SetInnerImageColor()
    {
        int num = Random.Range(0, (int)InnerColor.LastInstance);

        switch ((InnerColor)num)
        {
            case InnerColor.White:
                _innerImage.color = Color.white;
                break;
            case InnerColor.Red:
                _innerImage.color = Color.red;
                break;
            case InnerColor.Blue:
                _innerImage.color = Color.blue;
                break;
            case InnerColor.Green:
                _innerImage.color = Color.green;
                break;
            case InnerColor.Gray:
                _innerImage.color = Color.gray;
                break;
            case InnerColor.Yellow:
                _innerImage.color = Color.yellow;
                break;
        }
    }
}
