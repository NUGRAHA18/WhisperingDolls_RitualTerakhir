using UnityEngine;

// Taruh komponen ini di setiap objek yang bisa dipungut pemain (boneka, bahan bakar, kunci)
[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour, IInteractable
{
    [SerializeField] ItemData itemData;

    public string InteractPrompt => $"[E] Ambil {itemData?.itemName ?? "Item"}";

    public void Interact(PlayerInteraction interactor)
    {
        if (itemData == null)
        {
            Debug.LogWarning("[PickupItem] ItemData belum di-assign di Inspector! GameObject: " + gameObject.name);
            return;
        }

        bool added = InventoryManager.Instance.AddItem(itemData);
        if (added)
            gameObject.SetActive(false); // disembunyikan, tidak dihancurkan agar lebih aman
    }
}
