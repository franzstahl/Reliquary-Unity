using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float lastAttackTime;
    [SerializeField] private AudioClip attackSound;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

   
   private void Update()
    {
        if (lastAttackTime + attackCooldown < Time.time && Input.GetMouseButtonDown(0))
        {
            lastAttackTime = Time.time;


        }
    }
}
