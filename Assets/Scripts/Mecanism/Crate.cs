using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crate : Photon.PunBehaviour
{
    #region Public Fields
    [Tooltip("The other side crate")]
    public Crate sisterCrate;
    [Tooltip("Optionnal tag to detect humidity collisions")]
    [TagSelector]
    public string humidityTag;
    [Tooltip("Sound when moving")]
    public AK.Wwise.State stateMovingSound;
    [Tooltip("Sound when stopped moving")]
    public AK.Wwise.State stateStopSound;
    #endregion

    #region Private Fields
    private Vector2 distSisterCrate;
    private Vector2 posSisterCrate;
    private Vector2 posIniSisterCrate;

    private Vector2 distIniSisterCrate;
    private Vector2 position;

    private new Rigidbody2D rigidbody;

    private List<Collider2D> humidityColliders = new List<Collider2D>();
    #endregion

    void Start()
    {
        position = transform.position;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        if (sisterCrate != null)
        {
            posSisterCrate = sisterCrate.transform.position;
            posIniSisterCrate = sisterCrate.transform.position;
        }

        if (PhotonNetwork.connected)
        {
            this.photonView.RequestOwnership();
            this.sisterCrate.gameObject.GetPhotonView().RequestOwnership();
        }
    }

    private void Update()
    {
        if(Mathf.Abs(rigidbody.velocity.x) > 0.001f)
        {
            photonView.RPC("PlayMovingSound", PhotonTargets.All);
        }
        else
        {
            photonView.RPC("StopMovingSound", PhotonTargets.All);
        }

        if (sisterCrate != null)
        {
            posSisterCrate = sisterCrate.transform.position;
            if (Mathf.Abs (posSisterCrate.x - posIniSisterCrate.x) > 0)
            {
                distSisterCrate.x =  posSisterCrate.x - posIniSisterCrate.x;
                position.x += distSisterCrate.x; 
                posIniSisterCrate.x = posSisterCrate.x;
                transform.position = position;
            }
            if (Mathf.Abs(posSisterCrate.y - posIniSisterCrate.y) > 0)
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

    [PunRPC]
    public void PlayMovingSound()
    {
        stateMovingSound.SetValue();
    }

    [PunRPC]
    public void StopMovingSound()
    {
        stateStopSound.SetValue();
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
