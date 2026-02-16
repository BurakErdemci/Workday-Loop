using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DataEntryTask : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField targetInputField; 
    public TMP_InputField playerInputField; 
    public TMP_Text instructionDisplay;  
    public TMP_Text timerDisplay; 
    public Button finishButton;

    [Header("Settings")]
    public float timeLimit = 30f;

    private MissionManagerNew manager;
    private string correctData;
    private float currentTimer;
    private bool isRunning = false;
    private bool isSabotageActive = false;
    
    public AudioSource dataAudioSource;
    public AudioClip pasteSound;
    public AudioClip submitSound;

    public void StartGame(MissionManagerNew mngr, MissionData data)
    {
        manager = mngr;
        correctData = data.customText;
        playerInputField.text = "";
        targetInputField.text = correctData;
        currentTimer = timeLimit;
        isRunning = true;
        isSabotageActive = DayCycleManager.currentDay >= 2 && !DayCycleManager.isSystemAbandoned;

        
        instructionDisplay.text = "VERİLERİ ALANA KOPYALAYIP YAPIŞTIRIN";
        instructionDisplay.fontStyle = FontStyles.Normal;
    
        finishButton.onClick.RemoveAllListeners();
        finishButton.onClick.AddListener(SubmitTask);
    }

    void Update()
    {
        if (!isRunning) return;

        currentTimer -= Time.unscaledDeltaTime;
        timerDisplay.text = Mathf.CeilToInt(currentTimer).ToString();

        if (currentTimer <= 0)
        {
            SubmitTask(); 
        }
    }

    public void SubmitTask()
    {
        dataAudioSource.PlayOneShot(submitSound, 0.7f);
        if (!isRunning) return;
        isRunning = false;

        int finalScore = 0;
        string playerInput = playerInputField.text.Trim();

        if (isSabotageActive)
        {
            
            if (string.IsNullOrEmpty(playerInput))
            {
                finalScore = 1;
                Debug.Log("<color=cyan>[DATA TASK]</color> Sabotaj Başarılı: Oyuncu veriyi işlemedi.");
            }
            else
            {
                finalScore = 0;
                Debug.Log("<color=white>[DATA TASK]</color> Sabotaj Başarısız: Oyuncu dürüstçe kopyalamayı seçti.");
            }
        }
        else
        {
            
            finalScore = 0;
        }

        manager.OnTaskCompleted(finalScore);
    }
}