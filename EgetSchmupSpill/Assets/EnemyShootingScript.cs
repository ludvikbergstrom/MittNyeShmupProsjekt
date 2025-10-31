using UnityEngine;

public class EnemyShootingScript : MonoBehaviour
{
    public float fireRate = 1.0f;
    float nextFire;

    void Start()
    {
    }


    void Update()
    {

        if (nextFire <= Time.time)
        {
            Shoot();
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
