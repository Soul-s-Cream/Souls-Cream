using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Switch : Photon.PunBehaviour
{
    #region Public Fields
    [Tooltip("Les différents Mecanism de la scène qui doivent être activé")]
    public Mecanism[] triggers;
    [Tooltip("Le tag de l'objet accepté pour activer l'interrupteur")]
    [TagSelector]
    public string tagFilter = "";
    #endregion

    #region Private Fields
    private bool BoutonOn = false;
    private Animator animator;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagFilter))
        {
            if(PhotonNetwork.connected)
            {
                this.photonView.RPC("SwitchOn", PhotonTargets.All); 
            }
            else
            {
                SwitchOn();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(tagFilter))
        {
            if (PhotonNetwork.connected)
            {
                this.photonView.RPC("SwitchOff", PhotonTargets.All);
            }
            else
            {
                SwitchOff();
            }
        }
    }

    [PunRPC]
    public void SwitchOn()
    {
        GameEvents.Instance.TriggerSwitchOn(triggers);
        BoutonOn = true;
        animator.SetBool("BoutonON", true);
    }

    [PunRPC]
    public void SwitchOff()
    {
        GameEvents.Instance.TriggerSwitchOff(triggers);
        BoutonOn = false;
        animator.SetBool("BoutonON", false);
    }
}
