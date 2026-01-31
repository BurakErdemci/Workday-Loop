using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class GlitchManager : MonoBehaviour
{
    [System.Serializable]
    public class GlitchEntry
    {
        public MissionData.MissionType type; //
        [TextArea(2, 5)]
        public string instruction;
    }

    public static GlitchManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    [Header("UI References")]
    public GameObject glitchPanel;
    public TextMeshProUGUI glitchText;

    [Header("Settings")]
    public float glitchDisplayDuration = 3f;

    [Header("Glitch Instructions")]
    public List<GlitchEntry> glitchInstructions;

    private Coroutine hideTimer;

    public void ShowGlitch(MissionData.MissionType type)
    {
        if (glitchPanel == null) return;

        string message = GetInstructionByType(type);

        if (!string.IsNullOrEmpty(message))
        {
            glitchText.text = message;
            glitchPanel.SetActive(true);

            if (hideTimer != null) StopCoroutine(hideTimer);
            hideTimer = StartCoroutine(HideAfterDelay(glitchDisplayDuration));
        }
    }

    private string GetInstructionByType(MissionData.MissionType type)
    {
        foreach (var entry in glitchInstructions)
        {
            if (entry.type == type) return entry.instruction;
        }
        return null;
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        HideGlitch();
    }

    public void HideGlitch()
    {
        if (glitchPanel != null) glitchPanel.SetActive(false);
        if (hideTimer != null)
        {
            StopCoroutine(hideTimer);
            hideTimer = null;
        }
    }
}