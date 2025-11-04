using System.Collections;
using UnityEngine;

public class EnemyHitDetectionScript : MonoBehaviour
{
    private EnemyHandlingScript enemyHandler;

    private void Start()
    {
        enemyHandler = FindAnyObjectByType<EnemyHandlingScript>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            // Get where the projectile hit the shield in world space
            ContactPoint2D contact = collision.contacts[0];
            Vector2 hitPoint = contact.point;

            // Ask the shield to apply damage at that point
            ShieldScript shield = collision.gameObject.GetComponent<ShieldScript>();
            if (shield != null)
            {
                // Convert world position to shield-local in the DamageAt method
                shield.DamageAt(shield.transform.InverseTransformPoint(hitPoint), 32);
            }
        }
        else
        {
            Destroy(gameObject);
            ScoreManagerScript.instance.AddScore();
            enemyHandler.StartCoroutine(enemyHandler.DelayedUpdateBottomShooters());
        }
    }

}
