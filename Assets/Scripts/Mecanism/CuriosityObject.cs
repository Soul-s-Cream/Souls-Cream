using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CuriosityObject : MonoBehaviour
{
    [Header("Enable/Disable the sprite renderer when the player use the curiosity scream")]

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
    }

    public void Reveal()
    {
        spriteRenderer.enabled = true;
        boxCollider.enabled = true;
    }
}
