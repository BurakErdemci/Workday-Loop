using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator; 
    
    Vector2 movement;

    [Header("Ses Ayarları")]
    public AudioSource footstepSource;
    public AudioClip footstepClip;
    public float stepInterval = 0.45f; 
    private float stepTimer;

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

       
        if (movement.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
           
            stepTimer = 0; 
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void PlayFootstep()
    {
       
        if (DayCycleManager.isDivineSilenceActive) return;

        if (footstepSource != null && footstepClip != null)
        {
            footstepSource.pitch = Random.Range(0.9f, 1.1f);
            footstepSource.PlayOneShot(footstepClip, 0.4f);
        }
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