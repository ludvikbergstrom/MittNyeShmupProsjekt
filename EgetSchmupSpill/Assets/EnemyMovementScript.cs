using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{

    Rigidbody2D rb;
    Vector2 moveValue;
    public float enemySpeed = 2.0f;
    public static bool goLeft = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveRight();
    }


    void FixedUpdate()
    {
        if (transform.position.x <= -3.3)
        {
            goLeft = false;
        }
        if (transform.position.x >= 3.3)
        {
            goLeft = true;
        }

        if (goLeft)
        {
            MoveLeft();
        }
        else 
        {
            MoveRight();
        }


        rb.linearVelocity = moveValue * enemySpeed;
    }

    void MoveLeft()
    {
        moveValue = Vector2.left;
    }

    void MoveRight()
    {
        moveValue = Vector2.right;
    }
}