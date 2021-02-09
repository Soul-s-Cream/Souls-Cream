using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveableBox : Photon.PunBehaviour
{
    [Tooltip("The other side moveable box")]
    public MoveableBox sister;
    [Tooltip("Optionnal tag to detect humidity collisions")]
    public string humidityTag;

    private Vector2 distBoxSoeur;
    private Vector2 posBoxSoeur;
    private Vector2 posIniBoxSoeur;

    private Vector2 distIniBoxSoeur;
    private Vector2 position;

    private new Rigidbody2D rigidbody;

    private List<Collider2D> humidityColliders = new List<Collider2D>();

    void Start()
    {
        position = transform.position;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        if (sister != null)
        {
            posBoxSoeur = sister.transform.position;
            posIniBoxSoeur = sister.transform.position;
        }

        if (PhotonNetwork.connected)
        {
            this.photonView.RequestOwnership();
            this.sister.gameObject.GetPhotonView().RequestOwnership();
        }
    }

    private void Update()
    {
        if (sister != null)
        {
            posBoxSoeur = sister.transform.position;
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

        if (humidityColliders.Count == 0)
        {
            rigidbody.mass = 10000;
        }
        else
        {
            rigidbody.mass = 5;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(humidityTag) || humidityTag == "")
        {
            if (!humidityColliders.Contains(collision))
            {
                humidityColliders.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(humidityTag) || humidityTag == "")
        {
            if (humidityColliders.Contains(collision))
            {
                humidityColliders.Remove(collision);
            }
        }
    }
}
