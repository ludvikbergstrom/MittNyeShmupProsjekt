using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    Rigidbody2D rb;
    public float projSpeed = 5.0f;
    public AudioClip projectileSound;
    public float volume = 1.0f;
    Vector2 moveValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AudioSource.PlayClipAtPoint(projectileSound, transform.position, volume);
    }


    void FixedUpdate()
    {
        moveValue = Vector2.up;
        rb.linearVelocity = moveValue * projSpeed;

        if (transform.position.y < -6 ||  transform.position.y > 6)
        {
            Destroy(gameObject);
        }
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
                shield.DamageAt(shield.transform.InverseTransformPoint(hitPoint), 16);
            }
        }
        
        Destroy(gameObject);
    }
}
