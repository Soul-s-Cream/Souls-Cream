using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWindowScript : MonoBehaviour             //a attacher aux sprites de vitre bris�e dans la sc�ne finale
{
    private void Start()
    {
        GameEvents.Instance.BrisDeEcranVoid += activeSprite;
    }
    public void activeSprite(GameObject[] spritCassure)
    {

        BrokenWindowScript BW;


        foreach (GameObject sprit in spritCassure)
        {
            BW = gameObject.GetComponent<BrokenWindowScript>();
            if (BW != null && BW == this && this.GetComponent<Renderer>().enabled == false)
            {
                this.GetComponent<Renderer>().enabled = true;
                break;
            }
        }


    }
}
