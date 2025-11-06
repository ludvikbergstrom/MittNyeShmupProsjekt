using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class ShieldScript : MonoBehaviour
{
    public int width = 64;   // Width of the shield in pixels (adjustable in Inspector)
    public int height = 32;  // Height of the shield in pixels (adjustable in Inspector)

    [Header("sound")]
    public AudioClip destroyedClip;

    Texture2D tex;
    SpriteRenderer sr;
    PolygonCollider2D polyCollider;

    InputAction attackAction;

    void Start()
    {
        attackAction = InputSystem.actions.FindAction("attack");
        sr = GetComponent<SpriteRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();

        // Create a brand new, fully green texture we can edit
        tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Color32[] pixels = new Color32[tex.width * tex.height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = new Color32(0, 255, 0, 255);

        tex.filterMode = FilterMode.Point;

        tex.SetPixels32(pixels);
        tex.Apply();

        // Make a sprite from it
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),
                                  new Vector2(0.5f, 0.5f), 100f);

        UpdateCollider();
    }

    public void DamageAt(Vector2 localPos, int radius)
    {

        // Convert from local position (in Unity units) to pixel coordinates
        float unitsToPixelsX = tex.width / sr.bounds.size.x;
        float unitsToPixelsY = tex.height / sr.bounds.size.y;

        int px = Mathf.RoundToInt((localPos.x + sr.bounds.extents.x) * unitsToPixelsX);
        int py = Mathf.RoundToInt((localPos.y + sr.bounds.extents.y) * unitsToPixelsY);

        int r2 = radius * radius;
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                int nx = px + x;
                int ny = py + y;
                if (nx >= 0 && nx < tex.width && ny >= 0 && ny < tex.height)
                {
                    if (x * x + y * y <= r2)
                        tex.SetPixel(nx, ny, new Color(0, 0, 0, 0)); // transparent
                }
            }
        }

        tex.Apply();
        if (!HasAnyVisiblePixels(tex))
        {
            AudioSource.PlayClipAtPoint(destroyedClip, transform.position, 1.0f);
            Destroy(gameObject);
        }
        UpdateCollider();
    }

    void UpdateCollider()
    {
        Destroy(polyCollider);
        polyCollider = gameObject.AddComponent<PolygonCollider2D>();
    }
    bool HasAnyVisiblePixels(Texture2D tex)
    {
        Color32[] pixels = tex.GetPixels32();
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].a > 0)
                return true; // Found a visible (non-transparent) pixel
        }
        return false; // All pixels are transparent
    }

}
