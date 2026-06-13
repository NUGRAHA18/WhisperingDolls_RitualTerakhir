using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] int maxSlots = 6;

    readonly List<ItemData> items = new List<ItemData>();

    public IReadOnlyList<ItemData> Items => items.AsReadOnly();
    public int Count => items.Count;
    public bool IsFull => items.Count >= maxSlots;

    // UI subscribe ke event ini untuk update tampilan slot
    public event Action OnInventoryChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public bool AddItem(ItemData item)
    {
        if (IsFull)
        {
            Debug.Log("[Inventory] Penuh! Tidak bisa mengambil " + item.itemName);
            return false;
        }

        items.Add(item);
        Debug.Log("[Inventory] Ditambah: " + item.itemName + " | Total: " + items.Count);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(ItemData item)
    {
        if (!items.Remove(item)) return false;

        Debug.Log("[Inventory] Dihapus: " + item.itemName + " | Total: " + items.Count);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(ItemData item) => items.Contains(item);

    public bool HasItemOfType(ItemType type) => items.Exists(i => i.itemType == type);

    public ItemData GetFirstOfType(ItemType type) => items.Find(i => i.itemType == type);

    public int CountOfType(ItemType type) => items.FindAll(i => i.itemType == type).Count;
}
