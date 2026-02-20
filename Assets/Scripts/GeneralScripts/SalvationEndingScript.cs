using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class SalvationEndingScript : MonoBehaviour
{
    [Header("Panel ve Yazılar")]
    public CanvasGroup[] panels; 
    public TextMeshProUGUI welcomeText;
    public CanvasGroup creditsGroup;

    [Header("Geri Dönüş Butonu")]
    public Button returnMenuButton; 
    public CanvasGroup buttonGroup; 

    [Header("Ses Kaynakları")]
    public AudioSource officeAmbience; 
    public AudioSource sfxSource;      
    public AudioSource tinnitusSource; 
    public AudioSource musicSource;    

    [Header("Ses Klipleri")]
    public AudioClip notificationSFX;
    public AudioClip drawerOpenSFX;
    public AudioClip gunGrabSFX;
    public AudioClip breathSFX;
    public AudioClip gunshotSFX;
    public AudioClip welcomeVoiceSFX;
    public AudioClip tinnitusSFX;

    [Header("Süre Ayarları")]
    public float initialDelay = 1.0f; 
    public float panelFadeTime = 1.0f; 
    public float displayDuration = 3.5f; 
    public float postGunshotSilence = 5.0f;
    public float welcomeToMusicDelay = 4.0f; 
    public string mainMenuSceneName = "Scene_MainMenu";

    void Start()
    {
        
        if (SceneController.Instance != null && SceneController.Instance.weightOfTheWorld != null)
        {
          
            SceneController.Instance.weightOfTheWorld.Stop();
            SceneController.Instance.weightOfTheWorld.DOKill(); 
            Debug.Log("<color=cyan>[FINAL]</color> Arka plan müziği susturuldu. Sinematik başlıyor.");
        }
        foreach (var p in panels) p.alpha = 0;
        welcomeText.alpha = 0;
        creditsGroup.alpha = 0;
        if(buttonGroup != null) buttonGroup.alpha = 0; 
        
        musicSource.Stop();
        musicSource.volume = 0;
        tinnitusSource.Stop();
        tinnitusSource.clip = tinnitusSFX;
        tinnitusSource.loop = true;

       
        if (returnMenuButton != null)
            returnMenuButton.onClick.AddListener(ReturnToMainMenu);
        
        StartCoroutine(PlayFinalSequence());
    }

    IEnumerator PlayFinalSequence()
    {
        officeAmbience.Play();
        officeAmbience.volume = 0.2f;
        yield return new WaitForSeconds(initialDelay);

       
        panels[0].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);
       
        panels[0].DOFade(0f, panelFadeTime);
        sfxSource.PlayOneShot(notificationSFX);
        panels[1].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);
        
        panels[1].DOFade(0f, panelFadeTime);
        sfxSource.PlayOneShot(drawerOpenSFX);
        panels[2].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);
       
        panels[2].DOFade(0f, panelFadeTime);
        sfxSource.PlayOneShot(gunGrabSFX);
        panels[3].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);
        
        panels[3].DOFade(0f, panelFadeTime);
        sfxSource.clip = breathSFX;
        sfxSource.Play();
        tinnitusSource.volume = 0;
        tinnitusSource.Play();
        tinnitusSource.DOFade(0.8f, displayDuration).SetEase(Ease.InQuad);
        officeAmbience.DOFade(0.02f, displayDuration);
        panels[4].DOFade(1f, panelFadeTime);

        yield return new WaitForSeconds(displayDuration + 1f);

      
        foreach (var p in panels) p.alpha = 0; 
        sfxSource.Stop(); 
        tinnitusSource.Stop(); 
        officeAmbience.Stop(); 
        sfxSource.PlayOneShot(gunshotSFX); 
        
        yield return new WaitForSeconds(postGunshotSilence);

        
        welcomeText.DOFade(1f, 2f);
        sfxSource.PlayOneShot(welcomeVoiceSFX);
        yield return new WaitForSeconds(welcomeToMusicDelay);

        
        musicSource.Play();
        musicSource.DOFade(1f, 3f); 
        yield return new WaitForSeconds(1.5f);

       
        welcomeText.DOFade(0f, 1.5f);
        creditsGroup.DOFade(1f, 3f);

      
        if(buttonGroup != null) buttonGroup.DOFade(1f, 3f);

      
        creditsGroup.transform.DOLocalMoveY(4500f, 120f).SetEase(Ease.Linear).OnComplete(() =>
        {
            
            ReturnToMainMenu();
        });
    }

    void ReturnToMainMenu()
    {
        
        DOTween.KillAll();
        DayCycleManager.ResetAllData();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}