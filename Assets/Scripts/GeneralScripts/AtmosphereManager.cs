using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class AtmosphereManager : MonoBehaviour
{
    [Header("1. Aşama: Gaslighting (Skor >= 2)")]
    public List<GameObject> normalObjects;  
    public List<GameObject> changedObjects;  
    public List<AudioSource> gaslightingSilence; 

    [Header("2. Aşama: Salvation Rutin Kırılması (Gün 5+)")]
    public List<AudioSource> salvationSilence; 

    void Start()
    {
        ApplyAtmosphere();
    }

    public void ApplyAtmosphere()
    {
        int score = DayCycleManager.totalComplianceScore;
        int day = DayCycleManager.currentDay;
        bool isSalvation = !DayCycleManager.isSystemAbandoned;

       
        if (isSalvation && score >= 2)
        {
            
            foreach (var obj in normalObjects) if(obj != null) obj.SetActive(false);
            foreach (var obj in changedObjects) if(obj != null) obj.SetActive(true);
            
           
            foreach (var src in gaslightingSilence) if(src != null) src.DOFade(0, 2f).OnComplete(() => src.Stop());
            
            Debug.Log("[ATMOSPHERE] Skor bazlı bozulma aktif.");
        }
        else
        {
            foreach (var obj in normalObjects) if(obj != null) obj.SetActive(true);
            foreach (var obj in changedObjects) if(obj != null) obj.SetActive(false);
        }

      
        if (isSalvation && day >= 5)
        {
           
            foreach (var src in salvationSilence) if(src != null) src.DOFade(0, 1.5f).OnComplete(() => src.Stop());
            
            Debug.Log("[ATMOSPHERE] Salvation yolu: Rutin sesleri susturuldu.");
        }
    }
}