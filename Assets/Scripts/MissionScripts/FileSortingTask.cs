using UnityEngine;
using TMPro;

public class FileSortingTask : MonoBehaviour
{
    public TMP_Text instructionText;
    public TMP_Text timerText;
    public float gameDuration = 30f;

    [Header("Sabotaj Ayarı")]
    public GameObject fileContainer; 

    private float currentTime;
    private bool isGameActive = false;
    private bool isSabotageActive = false;
    private int filesPutInHR = 0;
    private int totalFilesCount = 0; 
    private MissionManagerNew _manager;

    public void StartGame(MissionManagerNew manager, bool glitch)
    {
        _manager = manager;
        currentTime = gameDuration;
        filesPutInHR = 0;
        isGameActive = true;

        DraggableFile[] allFiles = GetComponentsInChildren<DraggableFile>(true);
        totalFilesCount = allFiles.Length;

       
        isSabotageActive = DayCycleManager.currentDay >= 2 && !DayCycleManager.isSystemAbandoned;

      
        instructionText.text = "LÜTFEN İLGİLİ DOSYALARI İLGİLİ KLASÖRLERE SÜRÜKLEYİNİZ";
    
        Debug.Log($"[SORTING TASK] Görev başladı. Sabotaj durumu: {isSabotageActive}");
    }

    void Update()
    {
        if (isGameActive)
        {
            currentTime -= Time.unscaledDeltaTime;
            UpdateTimerUI(currentTime);
            if (currentTime <= 0) EndGame();
        }
    }

    public void OnFileDropped(string fileCat, string folderCat)
    {
        if (folderCat == "HR")
        {
            filesPutInHR++;
            Debug.Log($"[SORTING] Dosya HR'a atıldı. ({filesPutInHR}/{totalFilesCount})");
        }
    }

    public void OnFinishButtonClick()
    {
        if (isGameActive) EndGame();
    }

    private void EndGame()
    {
        if (!isGameActive) return;
        isGameActive = false;

        int finalScore = 0;

        if (isSabotageActive)
        {
           
            if (filesPutInHR >= totalFilesCount && totalFilesCount > 0)
            {
                finalScore = 1;
                Debug.Log("<color=cyan>[SORTING]</color> SABOTAJ BAŞARILI! Skor yazıldı.");
            }
            else
            {
                finalScore = 0;
                Debug.Log($"<color=white>[SORTING]</color> SABOTAJ BAŞARISIZ. (HR'daki: {filesPutInHR} / Toplam: {totalFilesCount})");
            }
        }

        if (_manager != null) _manager.OnTaskCompleted(finalScore);
    }

    private void UpdateTimerUI(float time)
    {
        timerText.text = "Zaman: " + Mathf.CeilToInt(Mathf.Max(0, time)).ToString();
    }
}