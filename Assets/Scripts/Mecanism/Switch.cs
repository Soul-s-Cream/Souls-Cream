using UnityEngine;

public abstract class Switch : Mecanism
{
    #region Public Fields
    [Tooltip("Les diff�rents Mecanism de la sc�ne qui doivent �tre activ�")]
    public Mecanism[] triggers;
    #endregion

    #region Private Fields
    /// <summary>
    /// �tat du switch
    /// </summary>
    protected bool isOn = false;
    /// <summary>
    /// Nombre de signaux d'entr�e actifs
    /// </summary>
    protected int nbInputs = 0;
    [HideInInspector]
    [SerializeField]
    [Tooltip("Le nombre d'entr�es n�cessaires pour que l'interrupteur �mette un signal")]
    [Range(2, 5)]
    /// <summary>
    /// Seuil requis de signaux actifs pour activer le switch
    /// </summary>
    public int nbInputsRequirement = 1;
    #endregion

    /// <summary>
    /// Ajout un signal d'entr�e au bouton. 
    /// Si le nombre de signaux d'entr�e atteint est �gal au nombre requis, alors on envoit un signal d'activation.
    /// Si le nombre de signaux d'entr�e atteint devient un nombre inf�rieur au nombre requis apr�s l'avoir �gal�, envoit un signal de d�sactivation
    /// </summary>
    /// <param name="stateInput">L'�tat du signal d'entr�e � consid�rer</param>
    protected void AddInput(bool stateInput)
    {
        //Si on ajoute un signal actif...
        if (stateInput)
        {
            //... et qu'en ajoutant ce signal, le nombre devient �gal au nombre de signaux requis,
            // alors on �met un signal d'activation dans le cas o� le switch n'est pas actif
            if (nbInputs + 1 == nbInputsRequirement && !isOn)
            {
                //Si on est connect� au serveur, on �met alors un RPC. Sinon, on lance un signal classique
                if (PhotonNetwork.connected)
                {
                    this.photonView.RPC("SwitchOn", PhotonTargets.All);
                }
                else
                {
                    SwitchOn();
                }
            }
            //On ajoute le signal aux signaux actifs;
            nbInputs++;
        }
        //Si on ajoute un signal inactif...
        else
        {
            //... et que retirant ce signal, le nombre devient inf�rieur au nombre de signaux requis,
            // alors on �met un signal de d�sactivation
            if (nbInputs - 1 < nbInputsRequirement && isOn)
            {
                //Si on est connect� au serveur, on �met alors un RPC. Sinon, on lance un signal classique
                if (PhotonNetwork.connected)
                {
                    this.photonView.RPC("SwitchOff", PhotonTargets.All);
                }
                else
                {
                    SwitchOff();
                }
            }
            //On retire le signal des signaux actifs;
            nbInputs--;
        }
    }

    /// <summary>
    /// Le comportement lorsque le bouton �met une activation. Peut �tre appel� en RPC.
    /// </summary>
    [PunRPC]
    protected void SwitchOn()
    {
        GameEvents.Instance.TriggerSwitchOn(triggers);
        isOn = true;
    }

    /// <summary>
    /// Le comportement lorsque le bouton �met une d�sactivation. Peut �tre appel� en RPC.
    /// </summary>
    [PunRPC]
    protected void SwitchOff()
    {
        GameEvents.Instance.TriggerSwitchOff(triggers);
        isOn = false;
    }


}
