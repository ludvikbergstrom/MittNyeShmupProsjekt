using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public AudioClip menuMusic;

    private void Start()
    {
        AudioSource.PlayClipAtPoint(menuMusic, Camera.main.transform.position, 2.0f);

    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
