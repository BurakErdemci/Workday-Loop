using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    public HouseFogManager manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.CompareTag("Player") && manager != null) 
        {
            
            manager.HandleRoomFade(GetComponent<Collider2D>(), true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && manager != null) 
        {
            manager.HandleRoomFade(GetComponent<Collider2D>(), false);
        }
    }
}