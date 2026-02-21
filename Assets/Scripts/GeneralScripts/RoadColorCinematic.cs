using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class RoadColorCinematic : MonoBehaviour
{
    public Volume globalVolume;
    public Transform endPoint; 
    public float cinematicSpeed = 2.0f; 
    public Color targetOrange = new Color(1f, 0.6f, 0.2f, 1f); 

    private ColorAdjustments colorAdjust;
    private PlayerMovement playerMovement;
    private float startX;
    private bool isInitialized = false;

    IEnumerator Start()
    {
        GameObject playerObj = null;
        while (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            yield return null;
        }

        playerMovement = playerObj.GetComponent<PlayerMovement>();

       
        GameObject sp = GameObject.Find("Spawn_from_Scene_Home");
        if (sp == null) sp = GameObject.Find("SpawnPoint_Default");
        
   
        startX = (sp != null) ? sp.transform.position.x : playerObj.transform.position.x;

        if (globalVolume.profile.TryGet(out colorAdjust))
        {
            if (DayCycleManager.isRoadCinematicDone)
            {
                colorAdjust.saturation.Override(0f); 
                colorAdjust.colorFilter.Override(Color.white);
                if (playerMovement != null) playerMovement.moveSpeed = 5f; 
                this.enabled = false;
                yield break;
            }

            if (DayCycleManager.currentDay == 5 && !DayCycleManager.isSystemAbandoned)
            {
               
                colorAdjust.saturation.Override(-100f);
                colorAdjust.colorFilter.Override(Color.white);
                if (playerMovement != null) playerMovement.moveSpeed = cinematicSpeed;
                isInitialized = true;
            }
            else
            {
                colorAdjust.saturation.Override(-100f);
                this.enabled = false;
            }
        }
    }

    void Update()
    {
        if (globalVolume == null || colorAdjust == null || !isInitialized || playerMovement == null || endPoint == null) return;

        float totalDist = Mathf.Abs(endPoint.position.x - startX);
        float currentDist = Mathf.Abs(playerMovement.transform.position.x - startX);
        
        
        float progress = Mathf.Clamp01(currentDist / totalDist);

        colorAdjust.saturation.Override(Mathf.Lerp(-100f, 0f, progress));
        colorAdjust.colorFilter.Override(Color.Lerp(Color.white, targetOrange, progress));

        if (progress >= 0.99f)
        {
            DayCycleManager.isRoadCinematicDone = true;
            playerMovement.moveSpeed = 5f; 
        }
    }
}