using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] float timeToExplode = -.5f;
    [SerializeField] float blastRange;
    [SerializeField] GameObject explosionFX;
    [SerializeField] LayerMask whatIsDestructible;

    void Update()
    {
        timeToExplode -= Time.deltaTime;

        if (timeToExplode <= 0)
        {
            if (explosionFX)
                Instantiate(explosionFX, transform.position, transform.rotation);

            Destroy(gameObject);

            // get every destructible in the blast radius in an array:
            Collider2D[] objectsToDestroy = Physics2D.OverlapCircleAll(point: transform.position, radius: blastRange, layerMask: whatIsDestructible);

            // destroy each of those objects:
            if (objectsToDestroy.Length > 0)
            {
                foreach (Collider2D c in objectsToDestroy)
                    Destroy(c.gameObject);
            }
        }
    }
}
