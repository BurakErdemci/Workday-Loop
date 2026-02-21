using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal; 
using UnityEngine.SceneManagement;

public class AtmosphereManager : MonoBehaviour
{
    [Header("Işık Ayarları")]
    public Light2D globalLight; 
    public GameObject spotLightsGroup; 

    [Header("1. Aşama: Gaslighting (Skor >= 2)")]
    public List<GameObject> normalObjects;   
    public List<GameObject> changedObjects;  
    public List<GameObject> objectsToDisableAtScore2; 
    public List<AudioSource> gaslightingSilence; 

    [Header("2. Aşama: Salvation (Gün 5+)")]
    public List<GameObject> objectsToDisableInSalvation; 
    public List<AudioSource> salvationSilence; 
    
    [Header("Salvation Kamera Ayarı")]
    public float wideCameraSize = 7f; 
    public float zoomDuration = 2.5f;

    void Start()
    {
        ApplyAtmosphere();

        bool isSalvation = !DayCycleManager.isSystemAbandoned;
        string currentScene = SceneManager.GetActiveScene().name;

        if (DayCycleManager.currentDay >= 5 && isSalvation)
        {
            if (SceneController.Instance != null)
            {
               
                float delay = currentScene.Contains("Home") ? 15f : 0f;
                SceneController.Instance.StartSalvationMusicSequence(delay);
            }
        }
    }
    public void ApplyAtmosphere()
    {
        int score = DayCycleManager.totalComplianceScore;
        int day = DayCycleManager.currentDay;
        bool isSalvation = !DayCycleManager.isSystemAbandoned;

        float targetIntensity = 0.3f; 

        if (isSalvation && score >= 2)
        {
            targetIntensity = 0.5f; 

            foreach (var obj in normalObjects) if(obj != null) obj.SetActive(false);
            foreach (var obj in changedObjects) if(obj != null) obj.SetActive(true);
            foreach (var obj in objectsToDisableAtScore2) if(obj != null) obj.SetActive(false);
            foreach (var src in gaslightingSilence) if(src != null) src.DOFade(0, 2f).OnComplete(() => src.Stop());
            
            Debug.Log("[ATMOSPHERE] Skor 2+: Işık 0.5 yapıldı.");
        }
        else
        {
            foreach (var obj in normalObjects) if(obj != null) obj.SetActive(true);
            foreach (var obj in changedObjects) if(obj != null) obj.SetActive(false);
            foreach (var obj in objectsToDisableAtScore2) if(obj != null) obj.SetActive(true);
        }
        
        if (isSalvation && day >= 5)
        {
            targetIntensity = 1.1f; 
            var vcam = Object.FindFirstObjectByType<Unity.Cinemachine.CinemachineCamera>();
            if (vcam != null)
            {
                DOTween.To(() => vcam.Lens.OrthographicSize, x => vcam.Lens.OrthographicSize = x, wideCameraSize, zoomDuration)
                    .SetEase(Ease.OutCubic)
                    .SetUpdate(true);
            }

            if (spotLightsGroup != null) spotLightsGroup.SetActive(false);
            foreach (var obj in objectsToDisableInSalvation) if(obj != null) obj.SetActive(false);
            foreach (var src in salvationSilence) if(src != null) src.DOFade(0, 1.5f).OnComplete(() => src.Stop());
            
            Debug.Log("[ATMOSPHERE] Gün 5+: Işık 1.1 yapıldı (Final Evresi).");
        }

        if (globalLight != null)
        {
            DOTween.To(() => globalLight.intensity, x => globalLight.intensity = x, targetIntensity, 2f).SetUpdate(true);
        }
    }
}