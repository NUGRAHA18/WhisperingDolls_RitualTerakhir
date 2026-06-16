using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Stamina Bar")]
    [SerializeField] Slider staminaSlider;

    [Header("Item Counter")]
    [SerializeField] TMP_Text evilDollText;
    [SerializeField] TMP_Text fuelText;
    [SerializeField] TMP_Text goodDollText;

    [Header("Interact Prompt")]
    [SerializeField] TMP_Text interactPromptText;

    [Header("Scene References")]
    [SerializeField] PlayerController playerController;
    [SerializeField] PlayerInteraction playerInteraction;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        InventoryManager.Instance.OnInventoryChanged += RefreshInventoryDisplay;
        RefreshInventoryDisplay();
    }

    void OnDestroy()
    {
        if (InventoryManager.Instance != null)
            InventoryManager.Instance.OnInventoryChanged -= RefreshInventoryDisplay;
    }

    void Update()
    {
        if (staminaSlider != null && playerController != null)
            staminaSlider.value = playerController.Stamina / playerController.MaxStamina;

        if (interactPromptText != null && playerInteraction != null)
        {
            string prompt = playerInteraction.CurrentPrompt;
            bool hasPrompt = !string.IsNullOrEmpty(prompt);
            interactPromptText.gameObject.SetActive(hasPrompt);
            if (hasPrompt) interactPromptText.text = prompt;
        }
    }

    void RefreshInventoryDisplay()
    {
        if (evilDollText != null)
            evilDollText.text = $"Boneka Jahat: {InventoryManager.Instance.CountOfType(ItemType.DollEvil)}/{RitualManager.RequiredEvilDolls}";
        if (fuelText != null)
            fuelText.text = $"Kayu Bakar: {InventoryManager.Instance.CountOfType(ItemType.Fuel)}/{RitualManager.RequiredFuel}";
        if (goodDollText != null)
            goodDollText.text = $"Boneka Baik: {InventoryManager.Instance.CountOfType(ItemType.DollGood)}/4";
    }
}
