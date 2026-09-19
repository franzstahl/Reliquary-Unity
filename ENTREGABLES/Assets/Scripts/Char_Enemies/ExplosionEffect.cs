using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public void AnimationComplete()
    {
        Destroy(gameObject);
    }
}
