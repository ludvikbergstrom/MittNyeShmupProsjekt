using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerSpawnerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameOverScreen gameOverScreen;
    public float respawnTime = 2.0f;
    private bool spawnScheduled = false;

    void Start()
    {
        Invoke("SpawnPlayer", 0.2f);
    }

    private void Update()
    {
        if (!PlayerHealthManagerScript.instance.playerAlive &&
            PlayerHealthManagerScript.instance.playerHealth > 0 &&
            !spawnScheduled) //  Only run once per death
        {
            spawnScheduled = true;  // prevent multiple Invokes
            Invoke("SpawnPlayer", respawnTime);
        }
        if (PlayerHealthManagerScript.instance.playerHealth <= 0)
        {
            gameOverScreen.Setup(ScoreManagerScript.instance.currentScure);
        }
    }

    public void SpawnPlayer()
    {
        Instantiate(playerPrefab, transform.position, transform.rotation);
        PlayerHealthManagerScript.instance.playerAlive = true;
        spawnScheduled = false; // reset so it can spawn again next time
    }
}
