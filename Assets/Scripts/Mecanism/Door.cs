using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Direction
{
    HAUT,
    BAS,
    GAUCHE,
    DROITE
}

[RequireComponent(typeof(SpriteRenderer))]
public class Door : Mecanism
{
    #region Public Field
    [Tooltip("Direction de l'ouverture de la porte")]
    public Direction openingDirection;
    [Range(0f, 5f)]
    [Tooltip("Durée de l'animation d'ouverture")]
    public float openingTime = 1f;
    #endregion

    #region Private Fields
    private bool DoorOpen = false;
    public SpriteRenderer spriteRender;
    #endregion

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
    }

    protected override void SwitchOn()
    {
        if (!DoorOpen)
        {
            DoorOpen = true;
            switch(openingDirection)
            {
                case Direction.HAUT:
                    transform.DOMoveY(transform.position.y + spriteRender.bounds.size.y, openingTime);
                    break;
                case Direction.BAS:
                    transform.DOMoveY(transform.position.y - spriteRender.bounds.size.y, openingTime);
                    break;
                case Direction.GAUCHE:
                    transform.DOMoveX(transform.position.x - spriteRender.bounds.size.x, openingTime);
                    break;
                case Direction.DROITE:
                    transform.DOMoveX(transform.position.x + spriteRender.bounds.size.x, openingTime);
                    break;
            }
        }
    }

    protected override void SwitchOff()
    {
        if (DoorOpen)
        {
            DoorOpen = false;

            switch (openingDirection)
            {
                case Direction.HAUT:
                    transform.DOMoveY(transform.position.y - spriteRender.bounds.size.y, openingTime);
                    break;
                case Direction.BAS:
                    transform.DOMoveY(transform.position.y + spriteRender.bounds.size.y, openingTime);
                    break;
                case Direction.GAUCHE:
                    transform.DOMoveX(transform.position.x + spriteRender.bounds.size.x, openingTime);
                    break;
                case Direction.DROITE:
                    transform.DOMoveX(transform.position.x - spriteRender.bounds.size.x, openingTime);
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}

