using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Rigidbody2D rb;

    [SerializeField] private AudioClip deathSound;

    private AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Die()
    {
        audioSource.PlayOneShot(deathSound);

        playerMovement.SetInputLocked(true);
        playerMovement.ForceGroundedAnimState(); 
        playerAttack.SetInputLocked(true);

        rb.linearVelocity = Vector2.zero; // Stop the player's movement immediately

        anim.SetTrigger("isDead");
    }

    public void DeathAnimationComplete()
    {
        // GameManager.Instance.LoseLife();
    }
}