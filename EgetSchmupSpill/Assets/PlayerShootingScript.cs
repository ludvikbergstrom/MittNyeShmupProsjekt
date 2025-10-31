using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShootingScript : MonoBehaviour
{
    InputAction attackAction;
    public float fireRate = 1.0f;
    float nextFire;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackAction = InputSystem.actions.FindAction("attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (attackAction.IsPressed())
        {
            if (nextFire <= Time.time)
            {
                Shoot();
            }
        } 
    }

    public GameObject projectilePrefab;
    public void Shoot()
    {
        //Shoots bullets

        Instantiate(projectilePrefab, transform.position, transform.rotation);

        nextFire = Time.time + 1/fireRate;
    }
}
