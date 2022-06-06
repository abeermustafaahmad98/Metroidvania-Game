using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }

    void ApplyDamage()
    {
        PlayerHealthController.instance.TakeDamage(1);
    }
}
