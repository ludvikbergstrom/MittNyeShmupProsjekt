using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ProjectileEnemy"))
        {
            PlayerHealthManagerScript.instance.playerHealth--;
            PlayerHealthManagerScript.instance.playerAlive = false;
            PlayerHealthManagerScript.instance.UpdateUI();
            Destroy(gameObject);
        }
    }
}
