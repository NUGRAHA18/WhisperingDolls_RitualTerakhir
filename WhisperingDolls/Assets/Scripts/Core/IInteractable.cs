// Interface yang harus diimplementasi oleh semua objek yang bisa di-interact pemain
public interface IInteractable
{
    string InteractPrompt { get; }
    void Interact(PlayerInteraction interactor);
}
