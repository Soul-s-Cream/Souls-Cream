using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : Mecanism
{
    public float directionPorte = 5;
    private bool DoorOpen = false;

    protected override void SwitchOn()
    {
        if (!DoorOpen)
        {
            DoorOpen = true;
            transform.DOMoveY(transform.position.y + directionPorte, 1f);
        }
    }
}

