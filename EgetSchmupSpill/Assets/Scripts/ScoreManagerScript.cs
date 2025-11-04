using TMPro;
using UnityEngine;

public class ScoreManagerScript : MonoBehaviour
{
    public static ScoreManagerScript instance;

    public void Start()
    {
        instance = this;
        UpdateUI();
    }

    public int currentScure;
    public TextMeshProUGUI scoreText;
    public void AddScore()
    {
        currentScure += 50;
        UpdateUI();
    }

    public void UpdateUI()
    {
        scoreText.text = "Score : " + currentScure.ToString();

    }
}
