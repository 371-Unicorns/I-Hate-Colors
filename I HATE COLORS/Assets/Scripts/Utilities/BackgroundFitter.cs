using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scales background to fit current screen situation.
/// Based on https://kylewbanks.com/blog/create-fullscreen-background-image-in-unity2d-with-spriterenderer.!--
/// 
/// Author: David Askari
/// </summary>
public class BackgroundFitter : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            scale *= cameraSize.x / spriteSize.x;
        }
        else
        { // Portrait
            scale *= cameraSize.y / spriteSize.y;
        }
        transform.position = Vector2.zero; // Optional
        transform.localScale = scale;
    }
}
