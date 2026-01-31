using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

[System.Serializable]
public class RoomData
{
    public string roomName;
    public Collider2D roomTrigger;
    public SpriteRenderer fogSprite;
    public AudioSource roomAudio;
}

public class HouseFogManager : MonoBehaviour
{
    public List<RoomData> rooms;
    public float fadeDuration = 1.2f; 

    [Header("Volume Ayarları")]
    public float activeVolume = 0.05f;  
    public float muffledVolume = 0.001f; 

    [Header("Filter Ayarları")]
    public float activeFrequency = 22000f; // Net ses (Filtre yok gibi)
    public float muffledFrequency = 600f;  // Boğuk ses

    public void HandleRoomFade(Collider2D hitTrigger, bool entering)
    {
        foreach (var room in rooms)
        {
            if (room.roomTrigger == hitTrigger)
            {
                // Sisi yönet
                float targetAlpha = entering ? 0f : 1f;
                if(room.fogSprite != null) room.fogSprite.DOFade(targetAlpha, fadeDuration);

               
                if (room.roomAudio != null)
                {
                    room.roomAudio.DOKill();
                    
                    
                    float targetVol = entering ? activeVolume : muffledVolume;
                    room.roomAudio.DOFade(targetVol, fadeDuration).SetUpdate(true);

                  
                    AudioLowPassFilter filter = room.roomAudio.GetComponent<AudioLowPassFilter>();
                    if (filter != null)
                    {
                        float targetFreq = entering ? activeFrequency : muffledFrequency;
                       
                        DOTween.To(() => filter.cutoffFrequency, x => filter.cutoffFrequency = x, targetFreq, fadeDuration).SetUpdate(true);
                    }
                }
                break;
            }
        }
    }
    
}