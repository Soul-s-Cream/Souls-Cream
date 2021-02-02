using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWindowScript : MonoBehaviour
{
    private void Start()
    {
        GameEvents.Instance.BrisDeEcran += activeSprite;
    }
    public void activeSprite(GameObject spritCassure)
    {
        if (spritCassure == this.gameObject)
        {
            GetComponent<Renderer>().enabled = true;

        }
    }
}
