using System.Collections.Generic;
using UnityEngine;

public class DailyMissionDealer : MonoBehaviour
{
    public List<MissionData> allPossibleMissions;
   
    public List<InteractableTask> allDesks; 
    public GameObject dayOverPanel;

    void Start()
    {
        UpdateMissionForOurDesk();
    }

 
    public void UpdateMissionForOurDesk()
    {
        int currentDay = DayCycleManager.currentDay;
        int taskIndex = DayCycleManager.tasksCompleted; 

      
        List<MissionData> dailyPool = allPossibleMissions.FindAll(m => m.dayNumber == currentDay);

        if (allDesks.Count > 0)
        {
            InteractableTask ourDesk = allDesks[0]; 

            
            if (taskIndex >= 2 || taskIndex >= dailyPool.Count)
            {
                ourDesk.gameObject.SetActive(false);
                Debug.Log("[SİSTEM] Günlük kota doldu. Masa kilitlendi. Eve git.");
               // dayOverPanel.SetActive(true);

            }
            else
            {
              
                ourDesk.AssignMission(dailyPool[taskIndex]);
                ourDesk.gameObject.SetActive(true);
            
               
                Debug.Log($"[SİSTEM] {currentDay}. Gün, {taskIndex + 1}. Görev masaya kuruldu: " + dailyPool[taskIndex].name);
            }
        }
    }
}