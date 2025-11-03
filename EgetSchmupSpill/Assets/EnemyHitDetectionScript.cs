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
        Destroy(gameObject);
        enemyHandler.StartCoroutine(enemyHandler.DelayedUpdateBottomShooters());


    }

}
