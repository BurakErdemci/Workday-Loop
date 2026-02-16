using UnityEngine;

public class InteractableTask : MonoBehaviour
{
    [Header("UI & Manager References")]
    public GameObject missionIcon;
    public MissionManagerNew missionManager;

    private MissionData currentDayData;
    private bool hasTaskActive = false;
    private bool isPlayerNearby = false;

    private void Start()
    {
        if (missionManager == null)
            missionManager = Object.FindFirstObjectByType<MissionManagerNew>();

        if (missionIcon != null) missionIcon.SetActive(false);
    }

    public void AssignMission(MissionData data)
    {
        currentDayData = data;
        hasTaskActive = true;

        if (isPlayerNearby && missionIcon != null) missionIcon.SetActive(true);
    }

    void Update()
    {
        if (currentDayData != null && hasTaskActive && isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E) && !MissionManagerNew.IsInUI)
            {
                if (missionManager != null)
                {
                    missionManager.OpenUI(currentDayData);
                    hasTaskActive = false;
                    if (missionIcon != null) missionIcon.SetActive(false);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (hasTaskActive && missionIcon != null) missionIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (missionIcon != null) missionIcon.SetActive(false);
        }
    }
}