using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class AlarmClock : MonoBehaviour
{
    [Header("Ses Ayarları")]
    public AudioSource alarmSource;

    [Header("Mahmurluk Ayarları")]
    public float slowSpeed = 1.5f;   
    public float normalSpeed = 5f;  
    public Volume globalVolume;      

    private bool isRinging = false;
    private bool isPlayerNearby = false;
    private PlayerMovement player;
    private DepthOfField dof;
    private Vector3 originalLocalPos;

    void Start()
    {
        originalLocalPos = transform.localPosition;
        if (globalVolume != null) globalVolume.profile.TryGet<DepthOfField>(out dof);

       
        bool isSalvationPath = !DayCycleManager.isSystemAbandoned;
        if (isSalvationPath && DayCycleManager.currentDay >= 5)
        {
            if (dof != null) dof.active = false; 
            StartCoroutine(WaitAndSetSpeed(normalSpeed)); 
            DayCycleManager.hasWokenUpToday = true; 
            Debug.Log("<color=magenta>[ALARM]</color> Salvation Yolu: 5. Gün alarm ve mahmurluk devre dışı.");
            this.enabled = false; 
            return;
        }

      
        if (DayCycleManager.hasWokenUpToday)
        {
            if (dof != null) dof.active = false;
            StartCoroutine(WaitAndSetSpeed(normalSpeed));
            this.enabled = false;
            return;
        }

     
        if (dof != null)
        {
            dof.active = true;
            dof.focusDistance.Override(0.1f);
        }

        StartCoroutine(WaitAndSetSpeed(slowSpeed));
        Invoke("StartRinging", 3f);
    }

    IEnumerator WaitAndSetSpeed(float targetSpeed)
    {
        player = null;
        while (player == null)
        {
            player = Object.FindFirstObjectByType<PlayerMovement>();
            yield return null; 
        }
        player.moveSpeed = targetSpeed;
    }

    void StartRinging()
    {
        isRinging = true;
        if (alarmSource != null) alarmSource.Play();
       
        transform.DOShakePosition(100f, 0.07f, 15).SetLoops(-1, LoopType.Restart).SetUpdate(true);
    }

    void Update()
    {
        if (isRinging && isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            StopAlarm();
        }
    }

    private void OnMouseDown()
    {
        if (isRinging) StopAlarm();
    }

    public void StopAlarm()
    {
        isRinging = false;
        DayCycleManager.hasWokenUpToday = true;

        if (alarmSource != null) alarmSource.Stop();
        
        transform.DOKill(); 
        transform.localPosition = originalLocalPos; 

        if (player != null) player.moveSpeed = normalSpeed;

        if (dof != null)
        {
            DOTween.To(() => dof.focusDistance.value, x => dof.focusDistance.value = x, 10f, 1.5f)
                .SetUpdate(true)
                .OnComplete(() => dof.active = false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isRinging) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}