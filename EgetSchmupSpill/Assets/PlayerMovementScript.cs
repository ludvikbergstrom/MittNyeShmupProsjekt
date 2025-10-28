using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{

    InputAction moveAction;
    Rigidbody2D rb;
    public float playerSpeed = 5.0f;
    Vector2 moveValue;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("move");
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        moveValue = new Vector2(moveValue.x, 0.0f);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveValue * playerSpeed;
    }
}
