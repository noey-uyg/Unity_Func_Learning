using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Inventory_inventory : Singleton<Inventory_inventory>
{
    [SerializeField] private Color[] colors; // 아이템 색상 - 타입 번호 순서
    [SerializeField] private Transform _itemListParent;
    [SerializeField] private Transform _equipmentParent;
    [SerializeField] private Transform _dragParent;
    [SerializeField] private TextMeshProUGUI[] _texts; // 텍스트 - 타입 번호 순서
    [SerializeField] private GameObject _selectPopup;
    [SerializeField] private GameObject[] _selectPopupTexts;

    private Action _yesAction;
    private Action _noAction;

    public Transform ItemListParent { get { return _itemListParent; } }
    public Transform EquipmentParent { get { return _equipmentParent; } }
    public Transform DragParent { get { return _dragParent; } }

    private void Start()
    {
        for(int i = 0; i < _texts.Length; i++)
        {
            _texts[i].text = string.Format("{0} : {1}", (ItemType)i, 0);
        }
    }

    public void CreateItem()
    {
        // 아이템 생성
        Inventory_Item item = ItemPool.Instance.GetInventoryItem();
        item.transform.SetParent(_itemListParent);
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;

        // 아이템 속성 정의
        int randType = UnityEngine.Random.Range(0, (int)ItemType.LastInstance);
        int randAbility = UnityEngine.Random.Range(1, 11);
        ItemType itemType = (ItemType)randType;

        item.SetItemType(itemType);
        item.SetItemAbility(randAbility);
        item.SetColor(colors[randType]);
        item.SetText(itemType.ToString());
    }

    public void SetText(ItemType index, int ability)
    {
        _texts[(int)index].text = string.Format("{0} : {1}", index, ability);
    }

    public void ShowSelectPopupControl(bool value, bool up)
    {
        _selectPopup.SetActive(value);

        if (value)
        {
            if (up)
                _selectPopupTexts[0].SetActive(value);
            else
                _selectPopupTexts[1].SetActive(value);
        }
        else
        {
            for(int i = 0; i < _selectPopupTexts.Length; i++)
            {
                _selectPopupTexts[i].SetActive(value);
            }
        }
    }

    public void SetYesOrNoButtonAction(Action yesAction, Action noAction)
    {
        _yesAction = yesAction;
        _noAction = noAction;
    }

    public void YesButton()
    {
        _yesAction?.Invoke();
        _yesAction = null;
        _noAction = null;
        ShowSelectPopupControl(false, false);
    }

    public void NoButton()
    {
        _noAction?.Invoke();
        _noAction = null;
        _yesAction = null;
        ShowSelectPopupControl(false, false);
    }
}
