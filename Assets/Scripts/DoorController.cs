using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DoorController : MonoBehaviour
{
    private bool DoorOpen = false;

    // Start is called before the first frame update
    void Start() {

    

        GameEvents.Instance.switchOn += OnDoorwayOpen;
    }

    // Update is called once per frame
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
                    transform.DOMoveY(transform.position.y + 5, 1f);
                    break;
                }
            }
        }

        Debug.Log("yolo");

    }

 
}

