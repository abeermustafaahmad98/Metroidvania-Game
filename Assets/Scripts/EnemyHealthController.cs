using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] GameObject deathFX;
    [SerializeField] int maxHealth;
    [SerializeField] int health;

    private void OnEnable() => health = maxHealth;

    public void DamageEnemy(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (deathFX)
                Instantiate(original: deathFX, position: gameObject.transform.position, rotation: Quaternion.identity); 

            Destroy(gameObject);
        }
    }
}
