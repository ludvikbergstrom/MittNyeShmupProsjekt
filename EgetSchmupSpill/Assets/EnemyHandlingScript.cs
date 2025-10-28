using UnityEngine;

public class EnemyHandlingScript : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnY;
    private float spawnX;
    private float heightChange = 1.0f;
    private float widthChange = 0.4f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 5; i++) 
        {
            spawnY = transform.position.y - heightChange * i;
            spawnX = transform.position.x;

            for (int j = 0; j < 11; j++) 
            {
                Instantiate(enemyPrefab, new Vector2(spawnX, spawnY), transform.rotation);
                spawnX += widthChange;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
