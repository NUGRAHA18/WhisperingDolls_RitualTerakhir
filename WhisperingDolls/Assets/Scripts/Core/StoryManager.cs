using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] GameObject narratorPanel;
    [SerializeField] TMP_Text narratorText;

    [Header("Settings")]
    [SerializeField] float typewriterSpeed = 0.03f;
    [SerializeField] float holdDuration    = 3.5f;

    readonly Queue<string> queue = new Queue<string>();
    bool isDisplaying;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void ShowNarration(string text)
    {
        queue.Enqueue(text);
        if (!isDisplaying)
            StartCoroutine(ProcessQueue());
    }

    IEnumerator ProcessQueue()
    {
        isDisplaying = true;

        while (queue.Count > 0)
        {
            yield return StartCoroutine(TypeOut(queue.Dequeue()));
            yield return new WaitForSecondsRealtime(holdDuration);
        }

        if (narratorPanel != null) narratorPanel.SetActive(false);
        isDisplaying = false;
    }

    IEnumerator TypeOut(string text)
    {
        if (narratorPanel != null) narratorPanel.SetActive(true);
        if (narratorText != null)  narratorText.text = "";

        foreach (char c in text)
        {
            if (narratorText != null) narratorText.text += c;
            yield return new WaitForSecondsRealtime(typewriterSpeed);
        }
    }
}
