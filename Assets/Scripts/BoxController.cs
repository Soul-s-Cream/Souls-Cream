using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxController : MonoBehaviour
{
    private bool BoxMoveOn = false;
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!BoxMoveOn)
        {

        }

    }*/



     void Start()
     {
         GameEvents.Instance.switchBox += BoxMove;
     }
     private void BoxMove(List<GameObject> gameObjects)
     {
        BoxController BC;
         if (!BoxMoveOn)
         {
            BoxMoveOn = true;
            Debug.Log("ici ça marche");
            foreach (GameObject gameObject in gameObjects)
            {
                Debug.Log("ici ça ne marche plus");


                BC = gameObject.GetComponent<BoxController>();
                 if (BC != null && BC == this)
                 {
                     transform.DOMoveX(transform.position.x + 5, 1f);
                         break;
                 }
             }
         }
     }
}
