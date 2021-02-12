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
    public AK.Wwise.Event openSound;
    #endregion

    #region Private Fields
    private bool opening = false;
    private SpriteRenderer spriteRender;
    private Vector3 startPosition;
    private Tween tweenRunning;
    #endregion

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        startPosition = this.transform.position;
    }

    protected override void SwitchOn()
    {
        if (!opening)
        {
            opening = true;
            if (tweenRunning != null)
                tweenRunning.Kill();

            Vector3 destination = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            switch (openingDirection)
            {
                case Direction.HAUT:
                    destination.y += spriteRender.bounds.size.y;
                    break;
                case Direction.BAS:
                    destination.y -= spriteRender.bounds.size.y;
                    break;
                case Direction.GAUCHE:
                    destination.x -= spriteRender.bounds.size.x;
                    break;
                case Direction.DROITE:
                    destination.x += spriteRender.bounds.size.x;
                    break;
            }

            tweenRunning = transform.DOMove(destination, openingTime)
                .OnComplete(StopSoundActivation)
                .OnKill(StopSoundActivation);

            openSound.Post(gameObject);
        }
    }

    protected override void SwitchOff()
    {
        if (opening)
        {
            
            if (tweenRunning != null)
                tweenRunning.Kill();
            opening = false;

            tweenRunning = transform.DOMove(startPosition, openingTime)
                .OnComplete(StopSoundActivation)
                .OnKill(StopSoundActivation);

            openSound.Post(gameObject);
        }
    }

    private void StopSoundActivation()
    {
        openSound.Stop(gameObject, 200);
    }
}

