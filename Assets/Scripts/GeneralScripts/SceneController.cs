using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.Cinemachine;
using System.Collections;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    public static SceneController Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject prefab = Resources.Load<GameObject>("CameraFade");
                if (prefab != null)
                {
                    GameObject obj = Instantiate(prefab);
                    _instance = obj.GetComponent<SceneController>();
                    obj.transform.SetParent(null);
                    DontDestroyOnLoad(obj);
                }
            }
            return _instance;
        }
    }

    [Header("Settings")]
    public GameObject playerPrefab;
    public SpriteRenderer faderSprite;
    public float fadeDuration = 1f;

    [Header("Portal Settings")]
    public bool isPortal = false;
    public string targetSceneName;

    private static string lastSceneName = "";
    private bool isPlayerNearby = false;
    private CinemachineCamera globalVcam;

    private void Awake()
    {
        if (isPortal) return;

        if (_instance == null)
        {
            _instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
            ValidateFader();
        }
        else if (_instance != this)
        {
            if (_instance.playerPrefab == null && this.playerPrefab != null)
                _instance.playerPrefab = this.playerPrefab;
            
            Destroy(gameObject);
        }
    }

    private void OnEnable() { if (!isPortal) SceneManager.sceneLoaded += OnSceneLoaded; }
    private void OnDisable() { if (!isPortal) SceneManager.sceneLoaded -= OnSceneLoaded; }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance != this || isPortal || scene.name.Contains("Menu")) return;

        DOTween.KillAll();
        StopAllCoroutines();
        StartCoroutine(SafeSetupRoutine());
    }

    IEnumerator SafeSetupRoutine()
    {
        yield return null;
        yield return null;

        Time.timeScale = 1f;
        
        ValidateFader();
        EnsureGlobalCamera();
        BindToCamera();
        HandlePlayerSpawn();

        if (faderSprite != null && faderSprite.gameObject != null)
        {
            faderSprite.DOKill();
            faderSprite.sortingOrder = 999;
            faderSprite.color = new Color(0, 0, 0, 1);
            faderSprite.DOFade(0f, fadeDuration).SetUpdate(true);
        }
    }

    private void EnsureGlobalCamera()
    {
        if (globalVcam == null)
        {
            globalVcam = Object.FindFirstObjectByType<CinemachineCamera>();

            if (globalVcam == null)
            {
                GameObject camPrefab = Resources.Load<GameObject>("CinemachineCamera");
                if (camPrefab != null)
                {
                    GameObject camObj = Instantiate(camPrefab);
                    globalVcam = camObj.GetComponent<CinemachineCamera>();
                    DontDestroyOnLoad(camObj);
                }
            }
            else
            {
                DontDestroyOnLoad(globalVcam.gameObject);
            }
        }
        if (globalVcam != null) globalVcam.Priority = 100;
    }

    private void ValidateFader()
    {
        if (faderSprite == null || faderSprite.gameObject == null)
        {
            faderSprite = GetComponentInChildren<SpriteRenderer>();
        }
    }

    private void BindToCamera()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null && faderSprite != null)
        {
            if (faderSprite.transform.parent != null) faderSprite.transform.SetParent(null);
            faderSprite.transform.SetParent(mainCam.transform);
            faderSprite.transform.localPosition = new Vector3(0, 0, 1);
        }
    }

    public void FadeAndLoad(string sceneName)
    {
        if (Instance != this) return;
        ValidateFader();

        string currentScene = SceneManager.GetActiveScene().name;
        string finalTarget = sceneName; 

       
        if (!DayCycleManager.isSystemAbandoned && DayCycleManager.currentDay >= 5)
        {
            
            if (finalTarget == "Scene_Road" && currentScene == "Scene_Home") finalTarget = "Scene_Office";
            else if (finalTarget == "Scene_Road" && currentScene == "Scene_Office") finalTarget = "Scene_Home";
        }

   
        if (DayCycleManager.currentDay >= 7 && finalTarget == "Scene_Office")
        {
            finalTarget = DayCycleManager.isSystemAbandoned ? "Scene_Final_Loop" : "Scene_SalvationEnding";
            Debug.Log("<color=magenta>[FINAL]</color> 7. Gün tespiti. Finale gidiliyor: " + finalTarget);
        }

       

        lastSceneName = currentScene;
        faderSprite.transform.SetParent(null);
        DontDestroyOnLoad(faderSprite.gameObject);
        faderSprite.DOKill();
        faderSprite.gameObject.SetActive(true);
        faderSprite.sortingOrder = 999;

        faderSprite.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            SceneManager.LoadScene(finalTarget);
        });
    }

    public void HandlePlayerSpawn()
    {
        if (playerPrefab == null) playerPrefab = Resources.Load<GameObject>("Player");
        if (playerPrefab == null) return;

        GameObject spawnPoint = FindCorrectSpawnPoint();
        if (spawnPoint == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            player = Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
            DontDestroyOnLoad(player);
        }
        else
        {
            player.transform.position = spawnPoint.transform.position;
            if (player.TryGetComponent(out Rigidbody2D rb)) rb.linearVelocity = Vector2.zero;
        }

        if (globalVcam != null)
        {
            globalVcam.Follow = player.transform;
            globalVcam.ForceCameraPosition(player.transform.position, Quaternion.identity);
            globalVcam.OnTargetObjectWarped(player.transform, player.transform.position - globalVcam.transform.position);
        }
    }

    private GameObject FindCorrectSpawnPoint()
    {
        string cur = SceneManager.GetActiveScene().name;
        if (cur.Contains("Home") && (string.IsNullOrEmpty(lastSceneName) || lastSceneName.Contains("Menu") || lastSceneName == cur))
            return GameObject.Find("SpawnPoint_Default");
        
        GameObject sp = GameObject.Find("Spawn_from_" + lastSceneName);
        return sp != null ? sp : GameObject.Find("SpawnPoint_Default");
    }

    private void Update()
    {
        if (isPortal && isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (Instance != null)
            {
                
                Instance.FadeAndLoad(targetSceneName);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) { if (isPortal && other.CompareTag("Player")) isPlayerNearby = true; }
    private void OnTriggerExit2D(Collider2D other) { if (isPortal && other.CompareTag("Player")) isPlayerNearby = false; }
}