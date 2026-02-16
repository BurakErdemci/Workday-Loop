using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [Header("Ayarlar")]
    public string interactionMessage = "Etkileşim için [E]";
    public bool hideHintAfterUse = true; 

    [Header("Olaylar")]
    public UnityEvent onInteract; 

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            onInteract.Invoke();

            if (hideHintAfterUse && InputHintManager.Instance != null)
            {
                InputHintManager.Instance.HideHintPermanently();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (InputHintManager.Instance != null)
            {
                InputHintManager.Instance.ShowHint();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}