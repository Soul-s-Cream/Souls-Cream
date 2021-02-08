using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoxController : Photon.PunBehaviour
{
    public GameObject BoxSoeur;
    private bool BoxMoveOn = false;
    private Vector2 distBoxSoeur;
    private Vector2 posBoxSoeur;
    private Vector2 posIniBoxSoeur;

    private Vector2 distIniBoxSoeur;
    private Vector2 position;



    void Start()
    {
        GameEvents.Instance.switchBox += BoxMove;
        position = transform.position;
        if (BoxSoeur != null)
        {
            posBoxSoeur = BoxSoeur.transform.position;
            posIniBoxSoeur = BoxSoeur.transform.position;
        }

    }

    private void Update()
    {
        if (BoxSoeur != null)
        {
            posBoxSoeur = BoxSoeur.transform.position;
            if (Mathf.Abs (posBoxSoeur.x - posIniBoxSoeur.x) >= 0.001f)
            {
                distBoxSoeur.x =  posBoxSoeur.x - posIniBoxSoeur.x;
                position.x += distBoxSoeur.x; 
                posIniBoxSoeur.x = posBoxSoeur.x;
                transform.position = position;
            }
            if (Mathf.Abs(posBoxSoeur.y - posIniBoxSoeur.y) >= 0.001f)
            {
                position.y = transform.position.y;
            }
        }
    }

    private void BoxMove(BoxController box)
    {
        if (box== this) 
        {
            Rigidbody2D rg;
             
            if(PhotonNetwork.connected)
            {
                this.photonView.RequestOwnership();
                this.BoxSoeur.GetPhotonView().RequestOwnership();
            }

            if (!BoxMoveOn)
            {
                BoxMoveOn = true;
                rg = gameObject.GetComponent<Rigidbody2D>();
                rg.constraints = RigidbodyConstraints2D.None;

                rg.constraints = RigidbodyConstraints2D.FreezeRotation /*| RigidbodyConstraints2D.FreezePositionY*/;
            }

        }
    }
}
