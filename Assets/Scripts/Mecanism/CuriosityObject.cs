using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CuriosityObject : MonoBehaviour
{
    [Header("Enable/Disable the sprite renderer when the player use the curiosity scream")]

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public void Reveal()
    {
        spriteRenderer.enabled = true;
    }
}
