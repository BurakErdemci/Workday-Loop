using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentPoint = 0;

    void Update()
    {
        if (DialogueManager.IsDialogueActive) return;

        if (waypoints.Length == 0) return;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, waypoints[currentPoint].position) < 0.1f)
        {
            currentPoint = (currentPoint + 1) % waypoints.Length;
        }
    }
}