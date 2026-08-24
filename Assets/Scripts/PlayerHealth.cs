using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMovement;
    private PlayerAttack playerAttack;
    private Rigidbody2D rb; 

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Die()
    {
        // Locked input
        if (playerMovement != null)
        {
            playerMovement.SetInputLocked(true);
        }

        if (playerAttack != null)
        {
            playerAttack.enabled = false;
        }

        anim.SetTrigger("isDead");
    }

    public void DeathAnimationComplete()
    {
        // GameManager.Instance.LoseLife();
    }
}