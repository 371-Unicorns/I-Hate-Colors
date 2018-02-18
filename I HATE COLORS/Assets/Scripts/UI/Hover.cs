using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handels sprite of selected tower following mouse cursor, resulting in a hover effect.
/// </summary>
public class Hover : Singleton<Hover>
{
    /// <summary>
    /// SpriteRenderer which displays currently selected tower.
    /// </summary>
    private SpriteRenderer spriteRenderer, rangeIndicatorRenderer;

    /// <summary>
    /// Prevent instance of this class, since it's a Singleton.
    /// </summary>
    private Hover() { }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rangeIndicatorRenderer = transform.Find("RangeIndicator").GetComponent<SpriteRenderer>();

        Deactivate();
    }

    void Update()
    {
        /// Change hovers position to current mouse position.
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);          // Z = 0, because main camera's Z = -10.
    }

    /// <summary>
    /// Activate hovering effect by passing sprite to show.
    /// </summary>
    /// <param name="sprite">Sprite to display.</param>
    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
        this.spriteRenderer.enabled = true;
        this.rangeIndicatorRenderer.enabled = true;
    }

    /// <summary>
    /// Disable hovering effect.
    /// </summary>
    public void Deactivate()
    {
        this.spriteRenderer.enabled = false;
        this.rangeIndicatorRenderer.enabled = false;
    }

    public bool IsActive()
    {
        return this.spriteRenderer.enabled;
    }
}
