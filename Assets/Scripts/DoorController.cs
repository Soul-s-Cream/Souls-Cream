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
    private void OnDoorwayOpen(GameObject[] gameObjects)
    {
        DoorController DC;
        if (!DoorOpen)
        {

            foreach (GameObject gameObject in gameObjects)
            {
                DC = gameObject.GetComponent<DoorController>();
                if (DC != null && DC == this)
                {
                    DoorOpen = true;
                    transform.DOMoveY(transform.position.y + directionPorte, 1f);
                    break;
                }
            }
        }
    }
}

