using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackCooldown;
    private float lastAttackTime;
   
    private AudioSource audioSource;
    private Animator anim;
    private PlayerMovement playerMovement; // Reference to the PlayerMovement script to check if the player is grounded

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

   
   private void Update()
    {
        if (lastAttackTime + attackCooldown < Time.time && playerMovement.IsGrounded && Input.GetMouseButtonDown(0))
        {
            lastAttackTime = Time.time;
            anim.SetTrigger("Attack");
            audioSource.PlayOneShot(attackSound);

        }
    }
}
