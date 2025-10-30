using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandlingScript : MonoBehaviour
{
    public float spawnRate = 0.1f;
    public float moveSpeed = 0.1f;

    private float spawnY;
    private float spawnX;
    private float heightChange = 1.0f;
    private float widthChange = 0.4f;

    public GameObject enemyPrefab;

    private List<GameObject> enemyClones = new List<GameObject>();

    private bool spawningDone = false;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < 5; i++)
        {
            spawnY = transform.position.y + heightChange * i;
            spawnX = transform.position.x;

            for (int j = 0; j < 11; j++)
            {
                GameObject newClone = Instantiate(enemyPrefab, new Vector2(spawnX, spawnY), transform.rotation);
                enemyClones.Add(newClone);
                spawnX -= widthChange;

                yield return new WaitForSeconds(spawnRate);
            }
        }

        spawningDone = true;

        //  Start movement coroutine after spawning finishes
        StartCoroutine(MoveEnemiesForever());
    }

    IEnumerator MoveEnemiesForever()
    {
        while (true)
        {
            yield return StartCoroutine(MoveEnemies());
            yield return new WaitForSeconds(moveSpeed); // optional delay between full cycles
        }
    }

    IEnumerator MoveEnemies()
    {
        foreach (GameObject enemy in enemyClones)
        {
            EnemyMovementScript movement = enemy.GetComponent<EnemyMovementScript>();

            if (EnemyMovementScript.goLeft)
                movement.MoveLeft();
            else
                movement.MoveRight();

            yield return new WaitForSeconds(moveSpeed);
        }
    }
}
