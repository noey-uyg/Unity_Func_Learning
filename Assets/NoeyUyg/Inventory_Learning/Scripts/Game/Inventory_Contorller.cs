using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Contorller : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private GameObject _pressText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _inventory.SetActive(!_inventory.activeSelf);
            _pressText.SetActive(!_inventory.activeSelf);
        }
    }
}
