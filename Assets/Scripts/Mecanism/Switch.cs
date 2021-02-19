using UnityEngine;

public abstract class Switch : Mecanism
{
    #region Public Fields
    [Tooltip("Les différents Mecanism de la scène qui doivent être activé")]
    public Mecanism[] triggers;
    #endregion

    #region Private Fields
    /// <summary>
    /// État du switch
    /// </summary>
    protected bool isOn = false;
    /// <summary>
    /// Nombre de signaux d'entrée actifs
    /// </summary>
    protected int nbInputs = 0;
    [HideInInspector]
    [SerializeField]
    [Tooltip("Le nombre d'entrées nécessaires pour que l'interrupteur émette un signal")]
    [Range(2, 5)]
    /// <summary>
    /// Seuil requis de signaux actifs pour activer le switch
    /// </summary>
    public int nbInputsRequirement = 1;
    #endregion

    /// <summary>
    /// Ajout un signal d'entrée au bouton. 
    /// Si le nombre de signaux d'entrée atteint est égal au nombre requis, alors on envoit un signal d'activation.
    /// Si le nombre de signaux d'entrée atteint devient un nombre inférieur au nombre requis après l'avoir égalé, envoit un signal de désactivation
    /// </summary>
    /// <param name="stateInput">L'état du signal d'entrée à considérer</param>
    protected void AddInput(bool stateInput)
    {
        //Si on ajoute un signal actif...
        if (stateInput)
        {
            //... et qu'en ajoutant ce signal, le nombre devient égal au nombre de signaux requis,
            // alors on émet un signal d'activation dans le cas où le switch n'est pas actif
            if (nbInputs + 1 == nbInputsRequirement && !isOn)
            {
                //Si on est connecté au serveur, on émet alors un RPC. Sinon, on lance un signal classique
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
            //... et que retirant ce signal, le nombre devient inférieur au nombre de signaux requis,
            // alors on émet un signal de désactivation
            if (nbInputs - 1 < nbInputsRequirement && isOn)
            {
                //Si on est connecté au serveur, on émet alors un RPC. Sinon, on lance un signal classique
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
    /// Le comportement lorsque le bouton émet une activation. Peut être appelé en RPC.
    /// </summary>
    [PunRPC]
    protected void SwitchOn()
    {
        GameEvents.Instance.TriggerSwitchOn(triggers);
        isOn = true;
    }

    /// <summary>
    /// Le comportement lorsque le bouton émet une désactivation. Peut être appelé en RPC.
    /// </summary>
    [PunRPC]
    protected void SwitchOff()
    {
        GameEvents.Instance.TriggerSwitchOff(triggers);
        isOn = false;
    }


}
