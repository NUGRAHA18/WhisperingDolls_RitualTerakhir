using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NarratorTrigger : MonoBehaviour
{
    [TextArea(3, 8)]
    [SerializeField] string narrativeText;

    [SerializeField] float delay      = 0f;
    [SerializeField] bool triggerOnce = true;

    bool triggered;

    void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggerOnce && triggered) return;

        triggered = true;

        if (delay > 0f)
            Invoke(nameof(ShowText), delay);
        else
            ShowText();
    }

    void ShowText() => StoryManager.Instance?.ShowNarration(narrativeText);
}
