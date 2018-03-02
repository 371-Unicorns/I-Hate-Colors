using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handels sprite of selected tower following mouse cursor, resulting in a hover effect.
/// </summary>
public class Hover : MonoBehaviour
{
    /// <summary>
    /// SpriteRenderer which displays currently selected tower.
    /// </summary>
    private static SpriteRenderer spriteRenderer;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    public Hover() { }

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Deactivate();
    }

    public void Update()
    {
        /// Change hovers position to current mouse position.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);          // Z = 0, because main camera's Z = -10.
    }

    /// <summary>
    /// Activate hovering effect by passing sprite to show.
    /// </summary>
    /// <param name="sprite">Sprite to display.</param>
    public static void Activate(float range, Sprite sprite)
    {
        Hover.spriteRenderer.sprite = sprite;
        Hover.spriteRenderer.enabled = true;
    }

    /// <summary>
    /// Disable hovering effect.
    /// </summary>
    public static void Deactivate()
    {
        Hover.spriteRenderer.enabled = false;
    }

    public static bool IsActive()
    {
        return Hover.spriteRenderer.enabled;
    }

    public static Vector3 GetPosition() { return spriteRenderer.transform.position; }
}
