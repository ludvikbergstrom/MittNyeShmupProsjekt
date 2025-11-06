using UnityEngine;

public class ShieldSpawnerScript : MonoBehaviour
{
    public GameObject shieldPrefab;
    public float shieldGap = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnShields();
    }


    public void SpawnShields()
    {
        for (int i = 0; i < 4; i++)
        {
            Instantiate(shieldPrefab, new Vector2(transform.position.x + shieldGap * i, transform.position.y), transform.rotation);
        }
    }
}
