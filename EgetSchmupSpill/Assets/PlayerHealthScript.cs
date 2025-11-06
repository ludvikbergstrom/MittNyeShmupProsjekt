using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public AudioClip playerDeathSound;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectileEnemy"))
        {
            PlayerHealthManagerScript.instance.playerHealth--;
            PlayerHealthManagerScript.instance.playerAlive = false;
            PlayerHealthManagerScript.instance.UpdateUI();
            AudioSource.PlayClipAtPoint(playerDeathSound, transform.position, 1.0f);
            Destroy(gameObject);
        }
    }
}
