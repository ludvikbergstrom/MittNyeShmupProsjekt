using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Linq;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class EnemyHandlingScript : MonoBehaviour
{
    [Header("Enemy prefabs")]
    public GameObject enemyPrefabOne;
    public GameObject enemyPrefabTwo;
    public GameObject enemyPrefabThree;

    [Header("Spawn Settings")]
    public int rows = 5;
    public int columns = 11;
    public float spawnRate = 0.1f;
    public float widthChange = 0.5f;
    public float heightChange = 1.0f;
    public float spawnHeightChange = 0.0f;

    [Header("Movement Settings")]
    public float stepX = 0.2f;      // horizontal step
    public float stepY = 0.5f;      // vertical step when hitting edge
    public float moveSpeed = 0.5f;  // time between steps
    public float leftEdge = -3.3f;
    public float rightEdge = 3.3f;
    public float movePause = 0.1f;
    public float lowestY = -3.0f;

    [Header("Shoot Settings")]
    public float fireRate = 1.0f;

    public List<GameObject> enemyClones = new List<GameObject>();
    private bool goLeft = false;

    private int round = 1;
    private bool startSpawn = false;
    private int enemiesLeft;

    public ShieldSpawnerScript shieldScript;

    void Start()
    { 
        StartCoroutine(SpawnEnemies(enemyPrefabOne));
    }

    void Update()
    {
        enemiesLeft = rows * columns;


        foreach (GameObject enemy in enemyClones)
        {
            if (enemy == null)
            {
                enemiesLeft--;
                continue;
            }

            if (!PlayerHealthManagerScript.instance.playerAlive)
            {
                enemy.GetComponent<EnemyAnimationScript>().PauseAnimation();
            }
            if (PlayerHealthManagerScript.instance.playerAlive)
            {
                enemy.GetComponent<EnemyAnimationScript>().ResumeAnimation();
            }
        }

        
        if (enemiesLeft <= 0)
        {
            round++;
            startSpawn = true;
        }

        if (round == 2 && startSpawn)
        {
            rows = 2;
            columns = 4;
            stepX = 0.4f;
            widthChange = 1.5f;
            heightChange = 2.0f;
            spawnHeightChange = 1.0f;
            lowestY = -2.5f;
            movePause = 0.25f;

            StartCoroutine(SpawnEnemies(enemyPrefabTwo));
            shieldScript.SpawnShields();
            startSpawn = false;
        }

        if (round == 3 && startSpawn)
        {
            rows = 1;
            columns = 1;
            widthChange = 1.7f;
            spawnHeightChange = 2.0f;
            lowestY = -2.0f;
            movePause = 0.4f;
            fireRate = 0.9f;

            StartCoroutine(SpawnEnemies(enemyPrefabThree));
            shieldScript.SpawnShields();
            startSpawn = false;
        }


    }
    IEnumerator SpawnEnemies(GameObject enemyPrefab)
    {
        enemyClones.Clear();
        float startX = transform.position.x;
        float startY = transform.position.y + spawnHeightChange;

        for (int i = 0; i < rows; i++)
        {
            float spawnY = startY + heightChange * i;
            float spawnX = startX;

            for (int j = 0; j < columns; j++)
            {
                GameObject clone = Instantiate(enemyPrefab, new Vector2(spawnX, spawnY), Quaternion.identity);
                if (i != 0) { clone.GetComponent<EnemyShootingScript>().enabled = false; }
                EnemyInfoScript info = clone.GetComponent<EnemyInfoScript>();
                info.columnIndex = j;
                enemyClones.Add(clone);

                spawnX += widthChange;
                yield return new WaitForSeconds(spawnRate);
            }
        }
        
        // Start the movement coroutine after spawning finishes
        StartCoroutine(MoveEnemiesSpaceInvaders());
        UpdateBottomShooters();
        StartCoroutine(AssignShooter());
    }

    IEnumerator MoveEnemiesSpaceInvaders()
    {
        while (enemyClones.Count > 0)
        {
            // 1️ Check if any enemy hits an edge
            float currentLowestY = 6.0f;
            bool edgeHit = false;
            foreach (GameObject enemy in enemyClones)
            {
                if (enemy == null) continue;

                if (enemy.transform.position.y < currentLowestY)
                {
                    currentLowestY = enemy.transform.position.y;
                }

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
                if (currentLowestY >= lowestY)
                {
                    foreach (GameObject enemy in enemyClones)
                    {
                        if (enemy == null) continue;

                        enemy.transform.position += new Vector3(0, -stepY, 0); // move down
                        yield return new WaitWhile(() => !PlayerHealthManagerScript.instance.playerAlive);
                        yield return new WaitForSeconds(movePause);
                    }
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
                    yield return new WaitWhile(() => !PlayerHealthManagerScript.instance.playerAlive);
                    yield return new WaitForSeconds(movePause);
                }
            }

            // 3️ Wait before next movement step
            yield return new WaitForSeconds(moveSpeed);
        }
    }
    public IEnumerator DelayedUpdateBottomShooters()
    {
        yield return null; // wait one frame so destroyed enemy is finalized
        UpdateBottomShooters();
    }

    Dictionary<int, int> columnsLeft = new Dictionary<int, int>();
    public void UpdateBottomShooters()
    {
        Dictionary<int, GameObject> bottomEnemies = new Dictionary<int, GameObject>();

        columnsLeft = new Dictionary<int, int>();
        // Now, find the bottom-most enemy in each column
        foreach (GameObject enemy in enemyClones)
        {
            if (enemy == null)
            {
                continue;
            }
            EnemyInfoScript info = enemy.GetComponent<EnemyInfoScript>();
            
            if (columnsLeft.ContainsKey(info.columnIndex)) 
            {
                columnsLeft[info.columnIndex]++;
            }
            else
            {
                columnsLeft[info.columnIndex] = 1;
            }



            int col = info.columnIndex;

            if (!bottomEnemies.ContainsKey(col) ||
                enemy.transform.position.y < bottomEnemies[col].transform.position.y)
            {
                bottomEnemies[col] = enemy;
            }
            
        }
        
        // Enable shooting only for bottom enemies
        foreach (GameObject bottomEnemy in bottomEnemies.Values)
        {
            if (bottomEnemy == null) continue;
            bottomEnemy.GetComponent<EnemyShootingScript>().enabled = true;
        }
    }


    private IEnumerator AssignShooter()
    {
        while (true)
        {
            List<int> keys = columnsLeft.Keys.ToList();
            // Pick a random index
            int randomIndex = Random.Range(0, keys.Count);
            int randomColumn = keys[randomIndex];

            foreach (GameObject enemy in enemyClones)
            {
                if (enemy == null) continue;




                if (enemy.GetComponent<EnemyShootingScript>().enabled == true && enemy.GetComponent<EnemyInfoScript>().columnIndex == randomColumn)
                {
                    enemy.GetComponent<EnemyShootingScript>().Shoot();
                }

            }
            yield return new WaitWhile(() => !PlayerHealthManagerScript.instance.playerAlive);
            yield return new WaitForSeconds(fireRate);
        }
    }
}

