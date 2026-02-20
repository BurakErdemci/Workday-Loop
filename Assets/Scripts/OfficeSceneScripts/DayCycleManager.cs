using UnityEngine;
using System.Collections;
using TMPro;

public class DayCycleManager : MonoBehaviour
{
    public static DayCycleManager Instance;

    [Header("Day Stats")]
    public static int currentDay = 1;
    public static int tasksCompleted = 0;
    public static int totalComplianceScore = 0;
    public int maxDays = 6;

    public static bool isSystemAbandoned = false; 
    
  
    public static bool hasWokenUpToday = false; 
   
    public static bool isDivineSilenceActive = false; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (transform.parent != null) transform.SetParent(null); 
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    public void OnTaskFinished(int score)
    {
        tasksCompleted++;
        totalComplianceScore += score;
        Debug.Log($"<color=white>[DAY MANAGER]</color> Görev bitti. Günlük: {tasksCompleted}/2 | Toplam Skor: {totalComplianceScore}");

        if (tasksCompleted >= 2)
        {
            StartCoroutine(EndWorkDayUI());
        }
    }

    IEnumerator EndWorkDayUI()
    {
        MissionManagerNew mm = Object.FindFirstObjectByType<MissionManagerNew>();
        if (mm != null && mm.dayOverPanel != null)
        {
            mm.dayOverPanel.transform.SetAsLastSibling();
            mm.dayOverPanel.SetActive(true);
            Debug.Log("dayoverpanel açıldı");
            TMP_Text txt = mm.dayOverPanel.GetComponentInChildren<TMP_Text>();
            if (txt != null) txt.text = "MESAİ BİTTİ.\nEVE DÖN.";
        }
        yield return new WaitForSeconds(3f);
        if (mm != null && mm.dayOverPanel != null) mm.dayOverPanel.SetActive(false);
    }

    public void FinishDayAndSleep()
    {
        
        hasWokenUpToday = false;

        if (currentDay == 4)
        {
            if (totalComplianceScore < 5)
            {
                isSystemAbandoned = true;
                Debug.Log("<color=orange>[SİSTEM]</color> LOOP yoluna girildi.");
            }
            else
            {
                isSystemAbandoned = false;
                Debug.Log("<color=green>[SİSTEM]</color> SALVATION yoluna girildi.");
            }
        }
        
        if (currentDay == 6)
        {
            if (isSystemAbandoned)
            {
                currentDay = 7;
                tasksCompleted = 0;
                Debug.Log("<color=cyan>[DAY MANAGER]</color> Son sabah uyanışı... Gün 7.");
                SceneController.Instance.FadeAndLoad("Scene_Home");
            }
            else
            {
                currentDay = 7;
                tasksCompleted = 0;
                Debug.Log("<color=cyan>[DAY MANAGER]</color> Son sabah uyanışı... Gün 7.");
                SceneController.Instance.FadeAndLoad("Scene_Home");
            }
        }
        else
        {
            currentDay++;
            tasksCompleted = 0;
            SceneController.Instance.FadeAndLoad("Scene_Home");
        }
    }

    public static void ResetAllData()
    {
        currentDay = 1;
        tasksCompleted = 0;
        totalComplianceScore = 0;
        isSystemAbandoned = false;
        
        // REİS EKLEME: New Game dendiğinde burayı da sıfırlıyoruz
        hasWokenUpToday = false; 
        isDivineSilenceActive = false;

        Debug.Log("<color=white>[SYSTEM]</color> Tüm oyun verileri sıfırlandı. Yeni denek bekleniyor.");
    }
}