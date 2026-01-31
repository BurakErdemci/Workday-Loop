using UnityEngine;
using TMPro;
using DG.Tweening;

public class InputHintManager : MonoBehaviour
{
    public static InputHintManager Instance { get; private set; }

    [Header("UI Ayarları")]
    public CanvasGroup hintCanvasGroup; 
    private bool hasBeenShownPermanently = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        hasBeenShownPermanently = PlayerPrefs.GetInt("HintShown", 0) == 1;
        hintCanvasGroup.alpha = 0; 
    }

    public void ShowHint()
    {
        if (hasBeenShownPermanently || hintCanvasGroup.alpha > 0) return;

        hintCanvasGroup.DOFade(1f, 0.5f).SetUpdate(true);
    }

    public void HideHintPermanently()
    {
        if (hasBeenShownPermanently) return;

        hasBeenShownPermanently = true;
        PlayerPrefs.SetInt("HintShown", 1); 
        PlayerPrefs.Save();

        hintCanvasGroup.DOFade(0f, 0.5f).SetUpdate(true);
    }
}