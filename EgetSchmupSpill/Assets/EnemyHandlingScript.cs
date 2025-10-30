using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandlingScript : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;
    public int rows = 5;
    public int columns = 11;
    public float spawnRate = 0.1f;
    public float widthChange = 0.5f;
    public float heightChange = 1.0f;

    [Header("Movement Settings")]
    public float stepX = 0.2f;      // horizontal step
    public float stepY = 0.5f;      // vertical step when hitting edge
    public float moveSpeed = 0.5f;  // time between steps
    public float leftEdge = -3.3f;
    public float rightEdge = 3.3f;
    public float movePause = 0.1f;

    private List<GameObject> enemyClones = new List<GameObject>();
    private bool goLeft = true;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        for (int i = 0; i < rows; i++)
        {
            float spawnY = startY + heightChange * i;
            float spawnX = startX;

            for (int j = 0; j < columns; j++)
            {
                GameObject clone = Instantiate(enemyPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                enemyClones.Add(clone);

                spawnX += widthChange;
                yield return new WaitForSeconds(spawnRate);
            }
        }

        // Start the movement coroutine after spawning finishes
        StartCoroutine(MoveEnemiesSpaceInvaders());
    }

    IEnumerator MoveEnemiesSpaceInvaders()
    {
        while (enemyClones.Count > 0)
        {
            // 1️⃣ Check if any enemy hits an edge
            bool edgeHit = false;
            foreach (GameObject enemy in enemyClones)
            {
                if (enemy == null) continue;

                if (goLeft && enemy.transform.position.x <= leftEdge)
                {
                    edgeHit = true;
                    break;
                }
                else if (!goLeft && enemy.transform.position.x >= rightEdge)
                {
                    edgeHit = true;
                    break;
                }
            }

            // 2️⃣ Move all enemies
            if (edgeHit)
            {
                foreach (GameObject enemy in enemyClones)
                {
                    if (enemy == null) continue;
                    enemy.transform.position += new Vector3(0, -stepY, 0); // move down
                    yield return new WaitForSeconds(movePause);
                }
                goLeft = !goLeft; // reverse direction
            }
            else
            {
                Vector3 moveStep = new Vector3(goLeft ? -stepX : stepX, 0, 0);
                foreach (GameObject enemy in enemyClones)
                {
                    if (enemy == null) continue;
                    enemy.transform.position += moveStep; // move sideways
                    yield return new WaitForSeconds(movePause);
                }
            }

            // 3️⃣ Wait before next movement step
            yield return new WaitForSeconds(moveSpeed);
        }
    }
}
