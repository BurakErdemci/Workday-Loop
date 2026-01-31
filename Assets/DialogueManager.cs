using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static bool IsDialogueActive = false;
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;
    public float typingSpeed = 0.05f;

    public static DialogueManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowDialogue(string npcName, string message)
    {
        IsDialogueActive = true;
        dialoguePanel.SetActive(true);
        nameText.text = npcName;
        StopAllCoroutines();
        StartCoroutine(TypeMessage(message));
    }

    IEnumerator TypeMessage(string message)
    {
        messageText.text = "";
        foreach (char letter in message.ToCharArray())
        {
            messageText.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    public void HideDialogue()
    {
        IsDialogueActive = false;
        dialoguePanel.SetActive(false);
    }
}