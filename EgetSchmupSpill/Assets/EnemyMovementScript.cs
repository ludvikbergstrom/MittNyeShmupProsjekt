using UnityEngine;

public class EnemyMovementScript : MonoBehaviour
{

    public float enemyStepLenght = 0.02f;
    public static bool goLeft = false;


    void Start()
    {

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


        
    }

    Vector2 newPosition;
    public void MoveLeft()
    {
        newPosition = new Vector2 (transform.position.x - enemyStepLenght,transform.position.y); 
        transform.position = newPosition;
    }

    public void MoveRight()
    {
        newPosition = new Vector2(transform.position.x + enemyStepLenght, transform.position.y);
        transform.position = newPosition;
    }

}