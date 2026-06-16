using System;
using UnityEngine;

public class RitualManager : MonoBehaviour
{
    public static RitualManager Instance { get; private set; }

    public const int RequiredEvilDolls = 4;
    public const int RequiredFuel = 2;

    public event Action OnAllDollsCollected;

    bool allDollsNarrated;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += CheckAllDollsMilestone;
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= CheckAllDollsMilestone;
    }

    public int EvilDollsInInventory => InventoryManager.Instance.CountOfType(ItemType.DollEvil);
    public int FuelInInventory     => InventoryManager.Instance.CountOfType(ItemType.Fuel);

    public bool CanPerformRitual() =>
        EvilDollsInInventory >= RequiredEvilDolls && FuelInInventory >= RequiredFuel;

    public void PerformRitual()
    {
        if (!CanPerformRitual()) return;

        for (int i = 0; i < RequiredEvilDolls; i++)
            InventoryManager.Instance.RemoveItem(InventoryManager.Instance.GetFirstOfType(ItemType.DollEvil));
        for (int i = 0; i < RequiredFuel; i++)
            InventoryManager.Instance.RemoveItem(InventoryManager.Instance.GetFirstOfType(ItemType.Fuel));

        Debug.Log("[Ritual] Ritual selesai!");
    }

    void CheckAllDollsMilestone()
    {
        if (allDollsNarrated) return;
        if (EvilDollsInInventory < RequiredEvilDolls) return;

        allDollsNarrated = true;
        StoryManager.Instance?.ShowNarration(
            "Keempat boneka itu sudah ada di tanganmu...\nSekarang pergi ke gereja. Selesaikan ritual.");
        OnAllDollsCollected?.Invoke();
    }
}
