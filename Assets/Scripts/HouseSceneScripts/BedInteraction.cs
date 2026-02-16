using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    private bool isPlayerNearby = false;

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            
            int done = DayCycleManager.tasksCompleted;
            
            Debug.Log($"[BED CHECK] RAM'deki Görev Sayısı: {done}/2");

            if (done >= 2)
            {
                DayCycleManager.Instance.FinishDayAndSleep();
            }
            else
            {
                Debug.Log($"İşlerini bitirmeden yatağa giremezsin. Yapılan: {done}/2");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}