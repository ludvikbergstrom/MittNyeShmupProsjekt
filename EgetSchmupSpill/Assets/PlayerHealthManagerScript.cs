using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealthManagerScript : MonoBehaviour
{
    public static PlayerHealthManagerScript instance;
    public GameObject Player;
    public int playerHealth = 3;
    public bool playerAlive = true;

    public void Start()
    {
        instance = this;
        UpdateUI();
    }

    
    public TextMeshProUGUI scoreText;

    public void UpdateUI()
    {
        scoreText.text = "Lives : " + playerHealth.ToString();

    }
}
