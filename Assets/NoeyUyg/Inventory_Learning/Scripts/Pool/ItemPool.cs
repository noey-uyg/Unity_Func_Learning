using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemPool : Singleton<ItemPool>
{
    [SerializeField] private Inventory_Item _inventoryItemPrefab;
    private ObjectPool<Inventory_Item> _inventoryItemPool;

    private const int maxSize = 10000;
    private const int initSize = 100;

    protected override void OnAwake()
    {
        _inventoryItemPool = new ObjectPool<Inventory_Item>(CreateInventoryItem, ActivateInventoryItem, DisableInventoryItem, DestroyInventoryItem, false, initSize, maxSize);
    }

    private Inventory_Item CreateInventoryItem()
    {
        return Instantiate(_inventoryItemPrefab);
    }

    private void ActivateInventoryItem(Inventory_Item raw)
    {
        raw.gameObject.SetActive(true);
    }

    private void DisableInventoryItem(Inventory_Item raw)
    {
        raw.gameObject.SetActive(false);
    }

    private void DestroyInventoryItem(Inventory_Item raw)
    {
        Destroy(raw);
    }

    public Inventory_Item GetInventoryItem()
    {
        Inventory_Item raw = null;

        if (_inventoryItemPool.CountActive >= maxSize)
        {
            raw = CreateInventoryItem();
        }
        else
        {
            raw = _inventoryItemPool.Get();
        }

        return raw;
    }

    public void ReleaseInventoryItem(Inventory_Item raw)
    {
        _inventoryItemPool.Release(raw);
    }
}
