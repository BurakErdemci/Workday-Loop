using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcName;
    [TextArea(3, 10)]
    public string[] dialogueLines;
    private int currentLine = 0;
    private bool playerInRange = false;
    public GameObject interactPrompt;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null) interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactPrompt != null) interactPrompt.SetActive(false);
            DialogueManager.Instance.HideDialogue();
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !MissionManagerNew.IsInUI)
        {
            Interact();
        }
    }
    private Vector3 originalScale;
    void Start()
    {
        originalScale = transform.localScale;
    }
    public void Interact()
    {
        Transform player = Object.FindFirstObjectByType<PlayerMovement>().transform;

        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = originalScale;

        DialogueManager.Instance.ShowDialogue(npcName, dialogueLines[currentLine]);
        currentLine = (currentLine + 1) % dialogueLines.Length;
    }
}