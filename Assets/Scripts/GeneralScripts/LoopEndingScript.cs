using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoopEndingScript : MonoBehaviour
{
    [Header("Paneller ve Yazılar")]
    public CanvasGroup[] panels; 
    public CanvasGroup statementGroup; 
    public TextMeshProUGUI endStatementText;
    public CanvasGroup creditsGroup; 
    [Header("Geri Dönüş Butonu")]
    public Button returnMenuButton;
    public CanvasGroup buttonGroup;

    [Header("Ses Kaynakları")]
    public AudioSource officeHum; 
    public AudioSource sfxSource;      
    public AudioSource loopSfxSource;  
    public AudioSource clockTicking; 

    [Header("Ses Klipleri")]
    public AudioClip typingSFX;
    public AudioClip elevatorSFX;
    public AudioClip walkSFX;
    public AudioClip sighSFX;
    public AudioClip switchOffSFX;

    [Header("Ayarlar")]
    public float panelFadeTime = 2.0f; 
    public float displayDuration = 3.5f;
    public string mainMenuSceneName = "Scene_MainMenu";

    void Start()
    {
        
        foreach (var p in panels) p.alpha = 0;
        statementGroup.alpha = 0;
        creditsGroup.alpha = 0;
        if(buttonGroup != null) buttonGroup.alpha = 0;
        if(clockTicking != null) clockTicking.Stop();
        
        if (returnMenuButton != null)
            returnMenuButton.onClick.AddListener(ReturnToMenu);

        StartCoroutine(PlayLoopSequence());
    }

    IEnumerator PlayLoopSequence()
    {
        officeHum.Play();
        officeHum.volume = 0.15f;

        
        loopSfxSource.clip = typingSFX;
        loopSfxSource.loop = true;
        loopSfxSource.Play();
        panels[0].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);

        panels[0].DOFade(0f, panelFadeTime);
        panels[1].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);

        panels[1].DOFade(0f, panelFadeTime);
        loopSfxSource.Stop(); 
        sfxSource.PlayOneShot(elevatorSFX);
        panels[2].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);

        panels[2].DOFade(0f, panelFadeTime);
        officeHum.DOFade(0, 2f); 
        loopSfxSource.clip = walkSFX;
        loopSfxSource.Play(); 
        panels[3].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);

        panels[3].DOFade(0f, panelFadeTime);
        loopSfxSource.Stop(); 
        sfxSource.PlayOneShot(sighSFX);
        panels[4].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);

        panels[4].DOFade(0f, panelFadeTime);
        panels[5].DOFade(1f, panelFadeTime);
        yield return new WaitForSeconds(displayDuration);

       
        sfxSource.PlayOneShot(switchOffSFX);
        foreach (var p in panels) p.alpha = 0;
        yield return new WaitForSeconds(3f);

        
        if(clockTicking != null) clockTicking.Play();
        endStatementText.text = "VE HER ŞEY AYNI ŞEKİLDE DEVAM ETTİ.";
        statementGroup.DOFade(1f, 3f);
        yield return new WaitForSeconds(6f);

     
        statementGroup.DOFade(0f, 2f);
        yield return new WaitForSeconds(2f);

        creditsGroup.DOFade(1f, 3f);
        if(buttonGroup != null) buttonGroup.DOFade(1f, 3f);

        
        creditsGroup.transform.DOLocalMoveY(4000f, 120f).SetEase(Ease.Linear).OnComplete(() =>
        {
            ReturnToMenu();
        });
    }

    void ReturnToMenu()
    {
        DOTween.KillAll();
        DayCycleManager.ResetAllData();
        SceneManager.LoadScene(mainMenuSceneName);
    }
}