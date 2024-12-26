using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory_EquipSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemType _slotType;
    private Inventory_Item _equipItem;
    private Transform _thisTransform;
    private Inventory_Item _interItem;

    private void Start()
    {
        _thisTransform = GetComponent<Transform>();    
    }

    public void OnDrop(PointerEventData eventData)
    {
        Inventory_Item dragItem = eventData.pointerDrag.GetComponent<Inventory_Item>();
        _interItem = dragItem;
        if (_interItem != null)
        {
            if (_interItem.ItemAttr.itemType == _slotType) // 슬롯이 같으면 장착
            {
                AddItem(_interItem);
            }
            else
            {
                Reposition();
            }
        }
    }

    // 아이템 장착
    private void AddItem(Inventory_Item item) 
    {
        if(_equipItem == null || _equipItem == item) // 장착 아이템이 없거나, 현재 아이템일 때
        {
            _equipItem = item;
            item.ThisTransform.SetParent(_thisTransform);
            item.ThisTransform.localPosition = Vector3.zero;
            item.SetItemUsable(true);
            item.SetEquipSlot(this);
            Inventory_inventory.Instance.SetText(item.ItemAttr.itemType, item.ItemAttr.ability);
            _interItem = null; // 상호작용 중인 아이템 해제
        }
        else // 장착된 아이템이 있을 때
        {
            Inventory_inventory.Instance.ShowSelectPopupControl(true, _interItem.ItemAttr.ability >= _equipItem.ItemAttr.ability);
            Inventory_inventory.Instance.SetYesOrNoButtonAction(ReleaseAfterAddItem, Reposition);
        }
    }

    public void ReleaseAfterAddItem()
    {
        ReleaseItem();
        AddItem(_interItem);
    }

    public void Reposition()
    {
        _interItem.ThisTransform.SetParent(_interItem.StartParent);
        _interItem.ThisTransform.localPosition = Vector3.zero;
        _interItem = null; // 상호작용 중인 아이템 해제
    }

    // 아이템 해제
    public void ReleaseItem() 
    {
        _equipItem.ThisTransform.SetParent(Inventory_inventory.Instance.ItemListParent);
        _equipItem.SetItemUsable(false);
        Inventory_inventory.Instance.SetText(_equipItem.ItemAttr.itemType, 0);
        _equipItem = null;
    }
}
