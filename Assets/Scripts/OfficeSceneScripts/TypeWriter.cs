using System.Collections;
using UnityEngine;
using TMPro;

public class Typewriter : MonoBehaviour
{
    public float typingSpeed = 0.05f;
    private TextMeshProUGUI textMesh;
    private string fullText;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void StartTyping(string message)
    {
        if (textMesh == null) textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = "";
        fullText = message;

        StopAllCoroutines(); 
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText.ToCharArray())
        {
            textMesh.text += letter;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }
}