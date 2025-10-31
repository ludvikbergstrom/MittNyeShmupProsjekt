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
    private bool goLeft = false;

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
                if (i != 0) {clone.GetComponent<EnemyShootingScript>().enabled = false;}
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
            // 1️ Check if any enemy hits an edge
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

            // 2️ Move all enemies
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

            // 3️ Wait before next movement step
            yield return new WaitForSeconds(moveSpeed);
        }
    }

    public void UpdateBottomShooters()
    {
        Debug.Log("oppdater hvem som skyter");
    }


        /*
        public void UpdateBottomShooters()
        {

            // Now, find the bottom-most enemy in each column
            Dictionary<float, GameObject> bottomEnemies = new Dictionary<float, GameObject>();      //vi lager et biblotekt for de laveste fiendene. nøkkelen er x-pos og verdien er fienden

            foreach (GameObject enemy in enemyClones)
            {
                if (enemy == null) continue;

                float xPos = Mathf.Round(enemy.transform.position.x * 10f) / 10f; // round x to avoid float imprecision
                if (!bottomEnemies.ContainsKey(xPos))
                {
                    bottomEnemies[xPos] = enemy;
                }
                else
                {
                    // If this one is lower (smaller y), replace it
                    if (enemy.transform.position.y < bottomEnemies[xPos].transform.position.y)
                    {
                        bottomEnemies[xPos] = enemy;
                    }
                }
            }

            // Enable shooting only for bottom enemies
            foreach (GameObject bottomEnemy in bottomEnemies.Values)
            {
                if (bottomEnemy == null) continue;
                bottomEnemy.GetComponent<EnemyShootingScript>().enabled = true;
            }
        */

    }

}
