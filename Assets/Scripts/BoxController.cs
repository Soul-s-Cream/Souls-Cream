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
        Rigidbody2D rg; 
         if (!BoxMoveOn)
         {
            BoxMoveOn = true;
            foreach (GameObject gameObject in gameObjects)
            {

                
                BC = gameObject.GetComponent<BoxController>();
                rg = gameObject.GetComponent<Rigidbody2D>();
                 if (BC != null && BC == this)
                 {
                    rg.constraints = RigidbodyConstraints2D.None;
                    rg.constraints = RigidbodyConstraints2D.FreezeRotation;/*| RigidbodyConstraints2D.FreezePositionY*/
                     //transform.DOMoveX(transform.position.x + 5, 1f);
                         break;
                 }
             }
         }
     }
}
