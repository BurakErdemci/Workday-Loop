using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private int currentPoint = 0;
    
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        
        
        if (waypoints.Length > 0)
            transform.position = waypoints[0].position;
    }

    void Update()
    {
        
        if (DialogueManager.IsDialogueActive || waypoints.Length == 0) 
        {
            if (anim != null) anim.SetBool("IsMoving", false);
            return;
        }

        MoveAndAnimate();
    }

    void MoveAndAnimate()
    {
        Vector2 targetPos = waypoints[currentPoint].position;
        Vector2 currentPos = transform.position;

        
        float distance = Vector2.Distance(currentPos, targetPos);

        if (distance > 0.1f)
        {
           
            Vector2 direction = (targetPos - currentPos).normalized;

            
            transform.position = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);

            
            if (anim != null)
            {
                anim.SetFloat("MoveX", direction.x);
                anim.SetFloat("MoveY", direction.y);
                
               
                anim.SetFloat("LastMoveX", direction.x);
                anim.SetFloat("LastMoveY", direction.y);
                
                anim.SetBool("IsMoving", true);
            }
        }
        else
        {
           
            if (anim != null) anim.SetBool("IsMoving", false);
            currentPoint = (currentPoint + 1) % waypoints.Length;
        }
    }
}