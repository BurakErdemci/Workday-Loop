using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator; 
    
    Vector2 movement;

    void Update()
    {
        
        if (UnityEngine.EventSystems.EventSystem.current != null &&
            UnityEngine.EventSystems.EventSystem.current.alreadySelecting)
        {
            movement = Vector2.zero;
            UpdateAnimation(movement); 
            return;
        }

      
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        
        UpdateAnimation(movement);
    }

    void FixedUpdate()
    {
        
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdateAnimation(Vector2 moveDir)
    {
        if (animator == null) return;

        if (moveDir.magnitude > 0)
        {
            animator.SetFloat("MoveX", moveDir.x);
            animator.SetFloat("MoveY", moveDir.y);
        
            animator.SetFloat("LastMoveX", moveDir.x);
            animator.SetFloat("LastMoveY", moveDir.y);
        
            animator.SetBool("IsMoving", true);

           
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}