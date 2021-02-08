using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuriosityObject : MonoBehaviour
{
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

    public void Mask()
    {
        spriteRenderer.enabled = false;
    }
}
