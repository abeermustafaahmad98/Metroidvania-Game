using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] Transform[] patrolPoints;
    [SerializeField] Rigidbody2D RB;
    [SerializeField] Animator enemyAnimator;
    [SerializeField] Transform groundCheck;

    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float waitTime;

    [Space]
    [SerializeField] LayerMask whatIsGround;

    int currentTargetedPoint;
    float waitCounter;
    bool isOnGround;

    private void OnEnable()
    {
        currentTargetedPoint = 0;
        waitCounter = waitTime;
        patrolPoints[0].parent.SetParent(null);
    }

    private void Update()
    {
        // the value saved in isOnGround will be determined by drawing a invisible circle which, when it'll overlap with
        // the ground, will return a true/false value:
        isOnGround = Physics2D.OverlapCircle(point: groundCheck.position, radius: .2f, layerMask: whatIsGround);

        float distance = Mathf.Abs(transform.position.x - patrolPoints[currentTargetedPoint].position.x);

        // if the enemy is .2f units away from the currently targeted patrol point
        if (distance > .2f)
        {
            // if the player is to the left of the PP:
            if (transform.position.x < patrolPoints[currentTargetedPoint].position.x)
            {
                RB.velocity = new Vector2(moveSpeed, RB.velocity.y);
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else
            {
                RB.velocity = new Vector2(-moveSpeed, RB.velocity.y);
                transform.localScale = Vector3.one;
            }

            if (transform.position.y < patrolPoints[currentTargetedPoint].position.y + .7f && isOnGround && distance < 4)
                RB.velocity = new Vector2(RB.velocity.x, jumpForce);
        }

        else
        {
            RB.velocity = new Vector2(0, RB.velocity.y);

            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                waitCounter = waitTime;
                currentTargetedPoint++;

                if (currentTargetedPoint >= patrolPoints.Length)
                    currentTargetedPoint = 0;
            }
        }

        // triggering animation according to speed:
        enemyAnimator.SetFloat("speed", Mathf.Abs(RB.velocity.x));
    }
}
