using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    Rigidbody2D rb;
    public float projSpeed = 5.0f;
    Vector2 moveValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        moveValue = Vector2.up;
        rb.linearVelocity = moveValue * projSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
