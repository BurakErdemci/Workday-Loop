using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement; 
using DG.Tweening;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Video & Overlay (Everything)")]
    public VideoPlayer videoPlayer;
    public Image blinkOverlay; 
    public float blinkAtSecond = 4.5f;

    [Header("Audio")]
    public AudioSource menuMusic;

    [Header("UI Elements")]
    public CanvasGroup uiContainer; 
    public Button startButton;
    public Button exitButton;

    [Header("Scene Settings")]
    public string firstSceneName = "Scene_Home";

    private bool isBlinking = false;

    private void Start()
    {
      
        if (startButton != null) startButton.onClick.AddListener(OnStartClicked);
        if (exitButton != null) exitButton.onClick.AddListener(OnExitClicked);

     
        if (blinkOverlay != null)
        {
            blinkOverlay.color = Color.black;
            blinkOverlay.DOFade(0f, 1f).SetUpdate(true);
        }

        if (uiContainer != null) uiContainer.alpha = 1f;
    }

    private void Update()
    {
        
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            if (videoPlayer.time >= blinkAtSecond && !isBlinking)
            {
                StartCoroutine(BlinkRoutine());
            }
        }
    }

    private IEnumerator BlinkRoutine()
    {
        isBlinking = true;

    
        blinkOverlay.DOFade(1f, 0.4f).SetUpdate(true);

     
        yield return new WaitUntil(() => videoPlayer.time < 0.5f);

      
        blinkOverlay.DOFade(0f, 0.4f).SetUpdate(true);

        isBlinking = false;
    }

    public void OnStartClicked()
    {
        DayCycleManager.ResetAllData();
        StopAllCoroutines();
    
        uiContainer.interactable = false;
        uiContainer.blocksRaycasts = false;

        
        SceneController controller = SceneController.Instance;
        if (controller == null) controller = FindFirstObjectByType<SceneController>();

        blinkOverlay.DOFade(1f, 1f).SetUpdate(true).OnComplete(() =>
        {
            if (controller != null)
            {
                controller.FadeAndLoad(firstSceneName);
            }
            else
            {
                // Hiçbir şey yoksa kaba kuvvetle yükle
                SceneManager.LoadScene(firstSceneName);
            }
        });
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}