using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoutonScript : Photon.PunBehaviour
{
    public Mecanism[] triggers;
    private bool BoutonOn = false;

    [TagSelector]
    public string tagFilter = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(tagFilter))
        {
            if(!PhotonNetwork.connected)
            {
                SwitchOn();
            }
            else
            {
               this.photonView.RPC("SwitchOn", PhotonTargets.All);
            }
        }
    }
    
    [PunRPC]
    public void SwitchOn()
    {
        GameEvents.Instance.SwitchTriggerOn(triggers);
        BoutonOn = true;
        GetComponent<Animator>().SetBool("BoutonON", true);
    }
}
