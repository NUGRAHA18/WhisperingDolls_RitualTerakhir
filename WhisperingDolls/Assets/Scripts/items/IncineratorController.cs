using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IncineratorController : MonoBehaviour, IInteractable
{
    [SerializeField] ParticleSystem flameEffect;

    bool ritualDone;

    public string InteractPrompt
    {
        get
        {
            if (ritualDone) return "Ritual telah selesai...";
            if (RitualManager.Instance == null) return "Tungku Ritual";

            if (RitualManager.Instance.CanPerformRitual())
                return "[E] Lakukan Ritual Pembakaran";

            int dollsLeft = RitualManager.RequiredEvilDolls - RitualManager.Instance.EvilDollsInInventory;
            int fuelLeft  = RitualManager.RequiredFuel - RitualManager.Instance.FuelInInventory;
            return $"Tungku — butuh {dollsLeft} boneka jahat lagi, {fuelLeft} kayu bakar lagi";
        }
    }

    public void Interact(PlayerInteraction interactor)
    {
        if (ritualDone) return;

        if (!RitualManager.Instance.CanPerformRitual())
        {
            StoryManager.Instance?.ShowNarration(
                "Kamu masih belum mengumpulkan semua yang dibutuhkan...\n" +
                "4 boneka jahat dan 2 kayu bakar — ritual tidak bisa dimulai.");
            return;
        }

        ritualDone = true;
        RitualManager.Instance.PerformRitual();

        if (flameEffect != null) flameEffect.Play();

        StoryManager.Instance?.ShowNarration(
            "Nyala api menelan keempat boneka itu.\nBisikan-bisikan itu perlahan... memudar.");

        EndingManager.Instance?.ShowGoodEnding();
    }
}
