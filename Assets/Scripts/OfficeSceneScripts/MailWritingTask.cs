using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening; 

public class MailWritingTask : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text targetTextDisplay;
    public TMP_InputField playerInputField;
    public TMP_Text timerText;
    public TMP_Text buttonLabel;
    public Button actionButton; 

    [Header("Settings")]
    [TextArea(3, 5)]
    public string scriptToType = "TÜM PERSONELİN DİKKATİNE:\r\n\r\nŞirket portalında tanımlanmış olan zorunlu iş yeri uyum eğitimlerinin tamamlanması ve çalışan memnuniyet anketinin doldurulması ivedilikle gerekmektedir.";
    public float gameDuration = 30f;
    public string sabotageKeyword = "Welcome"; 
    public int minRequiredChars = 15; 
    
    [Header("Ses Ayarları")]
    public AudioSource mailAudioSource;
    public AudioClip keyPressSound; 
    public AudioClip sendClickSound; 
    
    private float currentTime;
    private bool isGameActive = false;
    private bool isProcessingFinish = false;
    private MissionManagerNew _missionManager;
    private bool isSabotageActive = false;

    void Start()
    {
        if (playerInputField != null)
        {
            playerInputField.onValueChanged.AddListener(delegate { PlayTypingSound(); });
        }
        scriptToType = scriptToType.Trim();
        ResetTaskUI();
    }
    
    void PlayTypingSound() {
        if (keyPressSound != null && mailAudioSource != null)
        {
           
            mailAudioSource.pitch = Random.Range(0.92f, 1.08f);
        
       
            mailAudioSource.PlayOneShot(keyPressSound, 0.4f);
        }
    }

    void Update()
    {
        if (isGameActive && !isProcessingFinish)
        {
            currentTime -= Time.unscaledDeltaTime;
            UpdateTimerUI(currentTime);

           
            if (actionButton != null)
            {
                actionButton.interactable = IsMailReadyToSend();
            }

            if (currentTime <= 0)
            {
                StartCoroutine(FinishSequence());
            }
        }
    }

  
    private bool IsMailReadyToSend()
    {
        bool hasSabotageWord = isSabotageActive && playerInputField.text.Contains(sabotageKeyword);
        
      
        bool hasEnoughLength = playerInputField.text.Length >= minRequiredChars;

        return hasSabotageWord || hasEnoughLength;
    }

    public void StartGame(MissionManagerNew manager, bool isGlitchy)
    {
        _missionManager = manager;
        isProcessingFinish = false;
        ResetTaskUI(); 
    
        
        isSabotageActive = DayCycleManager.currentDay >= 2 && !DayCycleManager.isSystemAbandoned;
    }
    private void ResetTaskUI()
    {
        isGameActive = false;
        currentTime = gameDuration;

        playerInputField.text = "";
        playerInputField.interactable = false;
        buttonLabel.text = "Yazmaya başla";
        targetTextDisplay.text = scriptToType;
        UpdateTimerUI(gameDuration);
        
        if(actionButton != null) actionButton.interactable = true; 
    }

    public void OnActionButtonClick()
    {
        if (isProcessingFinish) return;

        if (!isGameActive)
        {
           
            isGameActive = true;
            playerInputField.interactable = true;
            playerInputField.Select();
            playerInputField.ActivateInputField();
            buttonLabel.text = "Maili gönder";
            
           
            if (actionButton != null) actionButton.interactable = false;
        }
        else
        {
            
            if (IsMailReadyToSend())
            {
                StartCoroutine(FinishSequence());
            }
            else
            {
                
                actionButton.transform.DOShakePosition(0.5f, 10f).SetUpdate(true);
                Debug.Log("Mail içeriği yetersiz! En az " + minRequiredChars + " karakter yazmalısın.");
            }
        }
    }

    private IEnumerator FinishSequence()
    {
        isProcessingFinish = true;
        isGameActive = false;
        playerInputField.interactable = false;

        int finalScore = 0; 
        if (isSabotageActive && playerInputField.text.Contains(sabotageKeyword))
        {
            finalScore = 1;
        }

        buttonLabel.text = "Gönderildi!";
        targetTextDisplay.text = (finalScore == 1) ? "SYSTEM OVERRIDDEN." : "Mail sisteme gönderildi";
        mailAudioSource.PlayOneShot(sendClickSound, 0.7f);
        yield return new WaitForSecondsRealtime(1.5f);

        if (_missionManager != null)
        {
            _missionManager.OnTaskCompleted(finalScore);
        }
    }

    private void UpdateTimerUI(float time)
    {
        timerText.text = "Zaman: " + Mathf.CeilToInt(Mathf.Max(0, time)).ToString();
    }
}