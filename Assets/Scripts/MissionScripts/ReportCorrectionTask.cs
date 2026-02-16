using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ReportCorrectionTask : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text timerText;
    public TMP_Text instructionText;
    public List<ReportErrorItem> allErrors; 

    [Header("Settings")]
    public float gameDuration = 15f; 
    private float currentTime;
    private bool isGameActive = false;
    private bool isSabotageActive = false;
    private int fixedCount = 0;
    private MissionManagerNew _manager;

    public void StartGame(MissionManagerNew manager, bool glitch)
    {
        _manager = manager;
        currentTime = gameDuration;
        fixedCount = 0;
        isGameActive = true;
        isSabotageActive = DayCycleManager.currentDay >= 2 && !DayCycleManager.isSystemAbandoned;

        
        instructionText.text = "Kırmızı hatalara basarak onları yeşil olarak düzeltiniz.";
    
        foreach (var error in allErrors) if(error != null) error.Setup(this);
    }

    void Update()
    {
        if (isGameActive)
        {
            currentTime -= Time.unscaledDeltaTime;
            if (timerText != null) timerText.text = "Zaman: " + Mathf.CeilToInt(currentTime);

            if (currentTime <= 0) EndTask();
        }
    }

    public void RegisterFix()
    {
        fixedCount++;
    }

    public void OnSubmitPressed()
    {
        EndTask();
    }

    private void EndTask()
    {
        if (!isGameActive) return;
        isGameActive = false;

        int finalScore = 0;
        int totalPossible = allErrors.Count;
        
        int errorsLeft = totalPossible - fixedCount;

        if (isSabotageActive)
        {
            
            if (errorsLeft >= 2)
            {
                finalScore = 1;
                Debug.Log($"<color=cyan>[REPORT TASK]</color> Sabotaj Başarılı: {errorsLeft} hata bilerek bırakıldı.");
            }
            else
            {
                finalScore = 0;
                Debug.Log($"<color=white>[REPORT TASK]</color> Sabotaj Başarısız: Sadece {errorsLeft} hata kaldı, yeterli değil.");
            }
        }
        else
        {
            
            finalScore = 0;
        }

        if (_manager != null) _manager.OnTaskCompleted(finalScore);
    }
}