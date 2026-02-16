using UnityEngine;
using UnityEngine.UI;

public class ReportErrorItem : MonoBehaviour
{
    public Color fixedColor = Color.green;
    public bool isFixed = false;
    public AudioSource errorSource;
    public AudioClip clickSound; 

    private Button button;
    private Image image;
    private ReportCorrectionTask controller;
    
    

    public void Setup(ReportCorrectionTask master)
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        controller = master;

        isFixed = false;
        image.color = Color.red;
        button.onClick.AddListener(OnClicked);

        Debug.Log($"[ERROR ITEM] {gameObject.name} initialized and ready.");
    }

    void OnClicked()
    {
        if (isFixed) return; 

        Debug.Log($"[ERROR ITEM] {gameObject.name} clicked! Changing to Green.");

        isFixed = true;
        image.color = fixedColor;
        if (errorSource != null) errorSource.PlayOneShot(clickSound, 0.6f);

        controller.RegisterFix();
    }
}