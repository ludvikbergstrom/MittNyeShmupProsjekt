using UnityEngine;

public class EnemyShootingScript : MonoBehaviour
{

    public GameObject projectilePrefab;
    public void Shoot()
    {
        //Shoots bullets

        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}
