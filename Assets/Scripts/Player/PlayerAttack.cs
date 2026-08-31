using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private bool canAttack = false;

    [SerializeField] private LayerMask hittableLayer;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private float attackRange;
    private float lastAttackTime;
    private bool inputLocked = false;
   
    private AudioSource audioSource;
    private Animator anim;
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script to check if the player is grounded

    public void SetInputLocked(bool locked)
    {
        inputLocked = locked;
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

   
   private void Update()
    {
        if (lastAttackTime + attackCooldown < Time.time && playerMovement.IsGrounded && canAttack && !inputLocked && Input.GetMouseButtonDown(0)) // Check if attack cooldown has passed, the player is grounded, and tleft mouse button is pressed
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
            audioSource.PlayOneShot(attackSound);

            DoAttack();
        }
    
    }

    private void DoAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPoint.position, attackRange, hittableLayer);

        foreach (Collider2D hit in hits)
        {
            IHittable hittable = hit.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.RegisterHit();
            }
        }
            
    }
}
