using UnityEngine;
using DG.Tweening;

public class OfficeFocusAudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource officeAmbience; 
    public AudioSource focusDrone;     

    [Header("Volume Ayarları")]
    public float officeNormalVolume = 0.3f;
    public float focusTargetVolume = 0.15f;
    public float fadeDuration = 0.8f;

    private bool lastUIState = false;

    void Start()
    {
       
        if (officeAmbience != null) officeAmbience.volume = officeNormalVolume;
        if (focusDrone != null) 
        {
            focusDrone.volume = 0f;
            focusDrone.loop = true;
            focusDrone.Play();
        }
    }

    void Update()
    {
      
        bool currentUIState = MissionManagerNew.IsInUI;

        if (currentUIState != lastUIState)
        {
            if (currentUIState) StartFocusMode();
            else StopFocusMode();

            
            lastUIState = currentUIState; 
        }
    }

    void StartFocusMode()
    {
        Debug.Log("[AUDIO] Odaklanma Modu: Ofis sesi kısılıyor.");
        if (officeAmbience != null) officeAmbience.DOFade(0f, fadeDuration).SetUpdate(true);
        if (focusDrone != null) focusDrone.DOFade(focusTargetVolume, fadeDuration).SetUpdate(true);
    }

    void StopFocusMode()
    {
        Debug.Log("[AUDIO] Normal Mod: Ofis sesi geri geliyor.");
        if (officeAmbience != null) officeAmbience.DOFade(officeNormalVolume, fadeDuration).SetUpdate(true);
        if (focusDrone != null) focusDrone.DOFade(0f, fadeDuration).SetUpdate(true);
    }
    private void OnDestroy()
    {
        
        DOTween.Kill(officeAmbience);
        if(focusDrone != null) DOTween.Kill(focusDrone);
    }
}