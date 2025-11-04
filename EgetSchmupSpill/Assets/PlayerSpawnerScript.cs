using UnityEditor;
using UnityEngine;

public class PlayerSpawnerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    private bool spawnScheduled = false;

    void Start()
    {
        SpawnPlayer();
    }

    private void Update()
    {
        if (!PlayerHealthManagerScript.instance.playerAlive &&
            PlayerHealthManagerScript.instance.playerHealth > 0 &&
            !spawnScheduled) //  Only run once per death
        {
            spawnScheduled = true;  // prevent multiple Invokes
            Invoke("SpawnPlayer", 1.0f);
        }
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, transform.position, transform.rotation);
        PlayerHealthManagerScript.instance.playerAlive = true;
        spawnScheduled = false; // reset so it can spawn again next time
    }
}
