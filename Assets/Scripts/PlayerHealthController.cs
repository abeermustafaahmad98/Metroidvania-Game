using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [HideInInspector]
    public int health;

    [SerializeField] int maxHealth;

    private void Awake()
    {
        if (!instance)
            instance = this;

        else
        {
            Destroy(instance);
            instance = this;
        }
    }

    private void Start() => health = maxHealth;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            //if (deathFX)
            //    Instantiate(original: deathFX, position: gameObject.transform.position, rotation: Quaternion.identity);

            health = 0;
            gameObject.SetActive(false);
        }
    }
}
