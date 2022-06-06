using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector2 moveDir;

    [SerializeField] float bulletSpeed;
    [SerializeField] Rigidbody2D RB;
    [SerializeField] GameObject impactFX;
    [SerializeField] int damageDone;

    void Update() =>  RB.velocity = moveDir * bulletSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
            other.GetComponent<EnemyHealthController>().DamageEnemy(damage: damageDone);

        if (impactFX)
            Instantiate(impactFX, transform.position, Quaternion.identity); 

        Destroy(gameObject);
    }

    private void OnBecameInvisible() =>  Destroy(gameObject);
}
