using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Inventory_Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _text;

    private ItemAttribute _itemAttr = new ItemAttribute();

    private Inventory_EquipSlot _equipSlot;

    private Transform _thisTransform;
    private Transform _startParent;
    private RectTransform _thisRectTransform;
    private Canvas _mainCanvas;
    
    public ItemAttribute ItemAttr { get { return _itemAttr; } }
    public Transform ThisTransform { get { return _thisTransform; } }
    public Transform StartParent { get { return _startParent; } }

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();
        _thisRectTransform = GetComponent<RectTransform>();
        _mainCanvas = GetComponentInParent<Canvas>();
    }

    // 우클릭 시 장착 해제
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_itemAttr.use && _equipSlot != null)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                _equipSlot.ReleaseItem();
                _equipSlot = null;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 원래 부모 저장
        _startParent = _thisTransform.parent;

        _thisTransform.SetParent(Inventory_inventory.Instance.DragParent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 마우스 이동량을 Canvas의 스케일 팩터로 나누어서 움직일 위치 계산
        Vector2 position = _thisRectTransform.anchoredPosition + eventData.delta / _mainCanvas.scaleFactor;

        // 드래그한 위치 적용
        _thisRectTransform.anchoredPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 슬롯을 식별하기 위해 Raycast 사용
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool droppedInSlot = false;

        foreach (RaycastResult result in results)
        {
            Inventory_EquipSlot slot = result.gameObject.GetComponent<Inventory_EquipSlot>();
            if (slot != null)
            {
                // 슬롯에 아이템 드랍
                slot.OnDrop(eventData);
                droppedInSlot = true;
                break;
            }
        }

        // 슬롯이 없으면 원래 위치로 재설정
        if (!droppedInSlot)
        {
            _thisTransform.SetParent(_startParent);
            _thisTransform.localPosition = Vector3.zero;
        }
    }

    public void ReleaseEquipSlot()
    {
        _equipSlot = null;
    }

    public void SetEquipSlot(Inventory_EquipSlot equipSlot)
    {
        if(_equipSlot == null)
            _equipSlot = equipSlot;
    }
    public void SetItemType(ItemType itemType)
    {
        _itemAttr.itemType = itemType;
    }

    public void SetItemAbility(int ability)
    {
        _itemAttr.ability = ability;
    }

    public void SetItemUsable(bool use)
    {
        _itemAttr.use = use;
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}
