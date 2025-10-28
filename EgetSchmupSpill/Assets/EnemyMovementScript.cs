using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{

    Rigidbody2D rb;
    public float enemySpeed = 5.0f;
    Vector2 moveValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        moveValue = Vector2.left;
        rb.linearVelocity = moveValue * enemySpeed;
    }
}