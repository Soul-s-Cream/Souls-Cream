using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DoorController : MonoBehaviour
{

    public float directionPorte = 5;
    private bool DoorOpen = false;
    void Start()
    {
        GameEvents.Instance.switchOn += OnDoorwayOpen;
    }
    private void OnDoorwayOpen(List<GameObject> gameObjects)
    {
        DoorController DC;
        if (!DoorOpen)
        {
            DoorOpen = true;
            foreach (GameObject gameObject in gameObjects)
            {
                DC = gameObject.GetComponent<DoorController>();
                if (DC != null && DC == this)
                {
                    transform.DOMoveY(transform.position.y + directionPorte, 1f);
                    break;
                }
            }
        }
    }
}

