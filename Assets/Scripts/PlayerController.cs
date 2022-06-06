using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Serialized fields:
    [Header("Physics")]
    [SerializeField] Rigidbody2D RB;
    [SerializeField] Transform groundcheck;
    [SerializeField] Animator standingSpriteAnimator;
    [SerializeField] Animator ballSpriteAnimator;

    [Space]
    [Header("Standing & Ball Mode")]
    [SerializeField] GameObject standing;
    [SerializeField] GameObject ball;

    [Space]
    [Header("After-image")]
    [SerializeField] SpriteRenderer playerSR;
    [SerializeField] SpriteRenderer afterImageSR;
    [SerializeField] Color afterImageColor;

    [Space]
    [SerializeField] BulletController bulletPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform bombPoint;

    [Header("Floats")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    [SerializeField] float afterImageLifeTime;
    [SerializeField] float timeBetweenAfterImages;
    [SerializeField] float dashWait;
    [SerializeField] float waitToBall;

    [Space]
    [SerializeField] LayerMask whatIsGround;

    // Private fields:
    PlayerAbilityTracker playerAbilityTracker;

    float dashCounter;
    float dashReachargeCounter;
    float afterImageCounter;
    float ballCounter;
    bool isOnGround;
    bool canDoubleJump;

    private void OnEnable() => playerAbilityTracker = GetComponent<PlayerAbilityTracker>();

    void Update()
    {
        #region Movement
        // ===================================== MOVING (LEFT/RIGHT) =====================================:
        //--> Dashing:
        if (dashReachargeCounter > 0)
            dashReachargeCounter -= Time.deltaTime;

        else
        {
            // if RMB is pressed, player is standing & dash ability has been unlocked:
            if (Input.GetButtonDown("Fire2") && standing.activeSelf && playerAbilityTracker.dashUnlocked)
            {
                dashCounter = dashTime;
                ShowAfterImage();
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            RB.velocity = new Vector2(dashSpeed * transform.localScale.x, RB.velocity.y);

            // --> Showing After-images:
            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
                ShowAfterImage();

            dashReachargeCounter = dashWait; // when the player has dashed once, don't let them dash again immediately.
                                             // instead, have a recharge timer in place
        }

        // --> Move normally if the player isn't dashing already:
        else
        {
            // Input.GetAxisRaw used to get immediate snappy movement (without gradual smoothing)
            float xMovement = Input.GetAxisRaw("Horizontal");
            RB.velocity = new Vector2(x: xMovement * moveSpeed, y: RB.velocity.y);

            // Flipping character when dir. is changed by adjusting its localScale:
            if (RB.velocity.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            else if (RB.velocity.x > 0)
                transform.localScale = Vector3.one;
        }

        // the value saved in isOnGround will be determined by drawing a invisible circle which, when it'll overlap with
        // the ground, will return a true/false value:
        isOnGround = Physics2D.OverlapCircle(point: groundcheck.position, radius: .2f, layerMask: whatIsGround);

        // ===================================== JUMPING =====================================
        // mapped to the space button                     // if the player has already jumped and double jumpe ability has been unlocked:
        if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && playerAbilityTracker.doubleJumpUnlocked)))
        {
            // if the player is on the ground, they can double jump:
            if (isOnGround)
                canDoubleJump = true;

            // but not in the air or they'll keep jumping infinitely:
            else
            {
                canDoubleJump = false;
                standingSpriteAnimator.SetTrigger("doubleJump");
            }

            RB.velocity = new Vector2(RB.velocity.x, jumpForce);
        }

        // ===================================== BALLSTANDING MODE =====================================:
        if (!ball.activeSelf)
        {
            float yMovement = Input.GetAxisRaw("Vertical");

            if (yMovement < -.9f && playerAbilityTracker.ballModeUnlocked)
            {
                ballCounter -= Time.deltaTime;

                if (ballCounter <= 0)
                {
                    // turn to ball...
                    ball.SetActive(true);
                    standing.SetActive(false);
                }
            }

            else
                ballCounter = waitToBall;
        }

        // if the player's already in ball mode:
        else
        {
            float yMovement = Input.GetAxisRaw("Vertical");

            if (yMovement > .9f)
            {
                ballCounter -= Time.deltaTime;

                if (ballCounter <= 0)
                {
                    // stand back up...
                    ball.SetActive(false);
                    standing.SetActive(true);
                }
            }

            else
                ballCounter = waitToBall;
        }

        // ===================================== UPDATING ANIMATIONS =====================================:
        if (standing.activeSelf)
        {
            // if the isOnGround bool in the script is true, it'll also set the animator's paramater with the same name to true,
            // and the jump anim will play:
            standingSpriteAnimator.SetBool("isOnGround", isOnGround);

            // if the abs. value of the velocity on the x-axis is > 0.1, the run anim. is played:
            standingSpriteAnimator.SetFloat("speed", Mathf.Abs(RB.velocity.x)); 
        }

        if (ball.activeSelf)
            ballSpriteAnimator.SetFloat("speed", Mathf.Abs(RB.velocity.x));
        #endregion

        #region Shooting
        if (Input.GetButtonDown("Fire1"))
        {
            if (standing.activeSelf)
            {
                // the bullet is going to be flipped WRT to the player's dir., as well:
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).moveDir = new Vector2(transform.localScale.x, 0);
                standingSpriteAnimator.SetTrigger("shotFired");
            }

            // if the player's in ball mode, the LMB will drop bombs:
            else if (ball.activeSelf && playerAbilityTracker.dropBombsUnlocked)
                Instantiate(bombPrefab, bombPoint.position, bombPoint.rotation);
        }
        #endregion
    }

    void ShowAfterImage()
    {
        SpriteRenderer afterImage = Instantiate(original: afterImageSR, position: transform.position, rotation: transform.rotation);
        afterImage.sprite = playerSR.sprite;
        afterImage.transform.localScale = transform.localScale;
        afterImage.color = afterImageColor;

        Destroy(afterImage.gameObject, afterImageLifeTime);
        afterImageCounter = timeBetweenAfterImages;
    }
}
