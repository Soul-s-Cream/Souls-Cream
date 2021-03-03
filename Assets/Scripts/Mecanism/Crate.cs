using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crate : Photon.PunBehaviour
{
    #region Public Fields    [Header("Behavior")]
    [Tooltip("The other side crate")]
    public Crate sisterCrate;
    [Range(0.001f, 0.5f)]
    [Tooltip("La masse de l'objet quand celui-ci est en contact avec un objet Humidity. Réduire pour que le Crate soit plus facile à déplacer")]
    public float movableMass = .5f;
    #endregion
    #region Private Fields
    /// <summary>
    /// Vecteur distance entre ce Crate et la Crate "soeur"
    /// </summary>

    private Vector2 distSisterCrate;
    /// <summary>
    /// Position de la Crate "soeur"
    /// </summary>
    private Vector2 posSisterCrate;
    /// <summary>
    /// Position initiale de la Crate "soeur" lors du démarrage de la scène
    /// </summary>
    private Vector2 posIniSisterCrate;
    /// <summary>
    /// Position de cet objet en worldSpace
    /// </summary>
    private Vector2 position;
    private new Rigidbody2D rigidbody;
    /// <summary>
    /// Les objets Humidity avec lesquels la Crate est en contact
    /// </summary>
    private List<Collider2D> humidityColliders = new List<Collider2D>();
    #endregion


    void Start()
    {
        //On récupère les composants nécessaires au fonctionnement de l'objet
        position = transform.position;
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        //On récupère les informations de la Crate "soeur"
        if (sisterCrate != null)
        {
            posSisterCrate = sisterCrate.transform.position;
            posIniSisterCrate = sisterCrate.transform.position;
        }
    }

    private void Update()
    {
        //On synchronise le Crate "soeur"
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
            //On replace correctement le Crate "soeur" si sa valeur en Y change
            if (Mathf.Abs(posSisterCrate.y - posIniSisterCrate.y) > 0)
            {
                position.y = transform.position.y;
            }
        }
        else
        {
            Debug.LogError("The Sister Crate haven't been defined!");
        }

        //Si jamais la boîte n'est pas en contact avec un objet Humidity, alors on lui donne une masse impossible à pousser.
        //Sinon on l'allège de sorte à pouvoir être déplaçable
        if (humidityColliders.Count == 0)
        {
            rigidbody.mass = 10000;
        }
        else
        {
            rigidbody.mass = movableMass;
        }
    }

    /// <summary>
    /// Permet de récupérer le contrôle en réseau de la Crate et de sa Crate "soeur".
    /// Attention : les messages en réseau sont doublé à cause du fonctionnement du script, redondant.
    /// </summary>
    public void GetOwnershipNetwork()
    {
        if (PhotonNetwork.connected)
        {
            this.photonView.RequestOwnership();
            this.sisterCrate.gameObject.GetPhotonView().RequestOwnership();
        }
    }

    #region Sadness Logic
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si la Crate est en contact avec un objet Humidity, l'ajoute à la liste humidityColliders
        if (collision.CompareTag(GameManager.Instance.humidityTag) || GameManager.Instance.humidityTag == "")
        {
            if (!humidityColliders.Contains(collision))
            {
                humidityColliders.Add(collision);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Si la Crate n'est plus en contact avec un objet Humidity, le retire de la liste humidityColliders
        if (collision.CompareTag(GameManager.Instance.humidityTag) || GameManager.Instance.humidityTag == "")
        {
            if (humidityColliders.Contains(collision))
            {
                humidityColliders.Remove(collision);
            }
        }
    }
    #endregion
}
