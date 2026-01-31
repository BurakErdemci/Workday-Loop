using UnityEngine;

public class MissionManagerNew : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mailPanel;
    public GameObject sortingPanel;
    public GameObject correctionPanel;
    public GameObject dataPanel;
    public GameObject dayOverPanel; 
    public GameObject GlitchPanel;

    [Header("Task Scripts")]
    public MailWritingTask mailTask;
    public FileSortingTask sortingTask;
    public ReportCorrectionTask correctionTask;
    public DataEntryTask dataTask;

    [Header("Game Stats")]
    public DayCycleManager dayManager;
    public int totalComplianceScore = 0;

    //public GameObject GlitchManager;

    public static bool IsInUI = false;

    public void OpenUI(MissionData data)
    {
        if (DayCycleManager.tasksCompleted >= 2)
        {
            dayOverPanel.SetActive(true);
            Debug.LogWarning("Mesai bitti, daha fazla çalışamazsın.");
            return; 
        }

        IsInUI = true;
        Time.timeScale = 0f;
        CloseAllPanels();

        if (DayCycleManager.currentDay >= 2 && !DayCycleManager.isSystemAbandoned)
        {
            StartMission(data);
        }

        switch (data.type)
        {
            case MissionData.MissionType.Mail:
                if (mailPanel != null) mailPanel.SetActive(true);
                if (mailTask != null) mailTask.StartGame(this, data.isGlitchy);
                break;
            case MissionData.MissionType.Sorting:
                if (sortingPanel != null) sortingPanel.SetActive(true);
                if (sortingTask != null) sortingTask.StartGame(this, data.isGlitchy);
                break;
            case MissionData.MissionType.Correction:
                if (correctionPanel != null) correctionPanel.SetActive(true);
                if (correctionTask != null) correctionTask.StartGame(this, data.isGlitchy);
                break;
            case MissionData.MissionType.DataEntry:
                if (dataPanel != null) dataPanel.SetActive(true);
                if (dataTask != null) dataTask.StartGame(this, data);
                else Debug.LogError("DataTask script reference is missing on MissionManager!");
                break;
        }
    }

    public void StartMission(MissionData mission)
    {
        if (GlitchManager.Instance != null)
        {
            GlitchManager.Instance.ShowGlitch(mission.type);
        }
        else
        {
            Debug.LogWarning("Sahnede GlitchManager bulunamadı!");
        }
    }

    public void OnTaskCompleted(int mistakes)
    {
     
        totalComplianceScore += mistakes;
        Debug.Log($"[MISSION MANAGER] Task finished. Task Score: {mistakes}. Total compliance: {totalComplianceScore}. Notifying Day Manager...");

        if (DayCycleManager.Instance != null)
        {
      
            DayCycleManager.Instance.OnTaskFinished(mistakes);
        }
        else
        {
            Debug.LogError("[MISSION MANAGER] FATAL: DayCycleManager.Instance is NULL! Is the script on an object?");
        }
        DailyMissionDealer dealer = Object.FindFirstObjectByType<DailyMissionDealer>();
        if (dealer != null) dealer.UpdateMissionForOurDesk();


        CloseUI();
    }

    public void CloseUI()
    {
        IsInUI = false;
        Time.timeScale = 1f;
        CloseAllPanels();
    }

    private void CloseAllPanels()
    {
        if (mailPanel != null) mailPanel.SetActive(false);
        if (sortingPanel != null) sortingPanel.SetActive(false);
        if (correctionPanel != null) correctionPanel.SetActive(false);
        if (dataPanel != null) dataPanel.SetActive(false);
       // if (dayOverPanel != null) dayOverPanel.SetActive(false);
       // if (GlitchPanel != null) GlitchPanel.SetActive(false);
    }
}