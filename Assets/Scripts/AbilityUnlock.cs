using UnityEngine;

public class AbilityUnlock : MonoBehaviour
{
    [SerializeField] AbilityType abilityType;
    [SerializeField] GameObject pickupFX;
    [SerializeField] TMPro.TMP_Text unlockText;

    public enum AbilityType
    {
        none,
        all,
        double_jump,
        dash,
        ball_mode,
        drop_bombs
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // on triggering, the powerup will be unlocked in accordance with the set enum value:
            PlayerAbilityTracker abilityTracker = other.GetComponentInParent<PlayerAbilityTracker>();

            switch (abilityType)
            {
                case AbilityType.double_jump:
                    abilityTracker.doubleJumpUnlocked = true;
                    unlockText.SetText("DOUBLE JUMP UNLOCKED!");
                    break;

                case AbilityType.dash:
                    abilityTracker.dashUnlocked = true;
                    unlockText.SetText("DASH UNLOCKED!");
                    break;

                case AbilityType.ball_mode:
                    unlockText.SetText("BALL MODE UNLOCKED!");
                    abilityTracker.ballModeUnlocked = true;
                    break;

                case AbilityType.drop_bombs:
                    unlockText.SetText("BOMB DROP UNLOCKED!");
                    abilityTracker.dropBombsUnlocked = true;
                    break;

                case AbilityType.all:
                    abilityTracker.doubleJumpUnlocked = true;
                    abilityTracker.dashUnlocked = true;
                    abilityTracker.ballModeUnlocked = true;
                    abilityTracker.dropBombsUnlocked = true;
                    unlockText.SetText("ALL ABILITIES UNLOCKED!");
                    break;

                case AbilityType.none:
                    break;

                default:
                    break;
            }

            Instantiate(pickupFX, transform.position, Quaternion.identity);

            // detach the parent canvas from ITS parent (the pickup object) or it'll get desroyed with it:
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position; // but keep the transform in place (the PU object's pos.)

            // set display msg for powerup:
            unlockText.gameObject.SetActive(true);

            Destroy(gameObject);
            Destroy(unlockText.transform.parent.gameObject, 1);
        }
    }
}
