using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Le type de d�clenchement que l'objet attend. Il h�rite du type byte pour pouvoir �tre serialized pour Photon
/// </summary>
public enum TriggerType : byte
{
    OnClick,
    OnPointerEnter,
    OnToggleOn,
    OnToggleOff
}

/// <summary>
/// Un objet qui d�termine le son � jouer en fonction d'un type d'�v�nement
/// </summary>
[System.Serializable]
public class SoundBehavior
{
    [Tooltip("D�finir l'�v�nement qui d�clenche le son")]
    public TriggerType triggerType;
    [Tooltip("L'�v�nement lanc� sur WWise lorsque la condition de d�clenchement est atteinte")]
    public AK.Wwise.Event soundEvent;

    /// <summary>
    /// Post un �v�nement WWise content l'Event renseign� en attribut
    /// </summary>
    /// <param name="gameObject">Le GameObject depuis lequel est post� l'Event WWise. N�cessaire pour le logique de ce dernier</param>
    public void PlaySound(GameObject gameObject)
    {
        Debug.Log("Play Sound");
        soundEvent.Post(gameObject);
    }
}

/// <summary>
/// G�re l'ensemble du comportement sonore de l'objet UI auquel il est rattach�.
/// </summary>
[RequireComponent(typeof(AkGameObj))]
public class UISound : Photon.PunBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [Header("Liste des comportements & sons associ�s")]
    [SerializeField]
    public List<SoundBehavior> behaviors;

    #region Private Fields
    private Toggle toggle;
    private bool isToggle = false;
    private delegate void SoundEvent(GameObject gameObject);
    private event SoundEvent onClick;
    private event SoundEvent onPointerEnter;
    private event SoundEvent onToggleOn;
    private event SoundEvent onToggleOff;
    #endregion

    private void Awake()
    {
        //On r�cup�re chacun des comportements du son, et pour chaque on associe l'�v�nement correspondant.
        //Si jamais OnToggleOn ou OnToggleOff est pr�sent dans le comportement, alors on va chercher l'objet Toggle
        if (behaviors != null && behaviors.Count != 0)
        {
            foreach (SoundBehavior behavior in behaviors)
            {
                switch (behavior.triggerType)
                {
                    case TriggerType.OnClick: onClick += behavior.PlaySound; break;
                    case TriggerType.OnPointerEnter: onPointerEnter += behavior.PlaySound; break;
                    case TriggerType.OnToggleOn: onToggleOn += behavior.PlaySound; isToggle = true; break;
                    case TriggerType.OnToggleOff: onToggleOff += behavior.PlaySound; isToggle = true; break;
                }
            }
        }

        //Si jamais l'objet est un Toggle, on en chercher le composant de m�me nom. Si jamais il n'existe pas ou n'est pas interactif,
        //alors on ne consid�re plus l'objet comme un Toggle (pour r�guler correctement le comportement)
        if (isToggle)
        {
            toggle = this.GetComponent<Toggle>();
            if (toggle == null || !toggle.interactable)
            {
                isToggle = false;
                Debug.LogWarning("Behavior is configurated like the UI object is a Toggle, but it's not. Please implement Toggle Component or make it Interactable.");
            }
        }
    }

    void BehaviorResolution(TriggerType triggerType)
    {
        switch (triggerType)
        {
            case TriggerType.OnClick: onClick?.Invoke(this.gameObject); break;
            case TriggerType.OnPointerEnter: onPointerEnter?.Invoke(this.gameObject); break;
            case TriggerType.OnToggleOn: onToggleOn?.Invoke(this.gameObject); break;
            case TriggerType.OnToggleOff: onToggleOff?.Invoke(this.gameObject); break;
        }
    }

    #region UnityEngine.EventSystems Callbacks
    public void OnPointerClick(PointerEventData eventData)
    {
        //Si l'objet cliqu� est un Toggle, alors on envoi l'�v�nement associ� � son nouvel �tat.
        if (isToggle)
        {
            if (toggle.isOn)
                BehaviorTrigger(TriggerType.OnToggleOn);
            else
                BehaviorTrigger(TriggerType.OnToggleOff);
        }
        else
            BehaviorTrigger(TriggerType.OnClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BehaviorTrigger(TriggerType.OnPointerEnter);
    }
    #endregion

    #region Network Compatibility RPC
    /// <summary>
    /// D�termine si le son doit �tre envoy� en RPC � tous les clients, ou simplement �tre jou� en local
    /// </summary>
    /// <param name="triggerType">Le type d'�v�nement pour le comportement</param>
    void BehaviorTrigger(TriggerType triggerType)
    {
        if (PhotonNetwork.connected && this.photonView != null)
            photonView.RPC("BehaviorResolutionRPC", PhotonTargets.All, (byte)triggerType);
        else
            BehaviorResolution(triggerType);
    }

    /// <summary>
    /// Fonction RPC d�clench� quand le son doit �tre jou� pour tous les clients
    /// </summary>
    /// <param name="triggerType"></param>
    [PunRPC]
    void BehaviorResolutionRPC(byte triggerType)
    {
        //On r�ceptionne la donn�e envoy�, que l'on reconverti en TriggerType pour la logique de l'objet...
        TriggerType trigger = (TriggerType)triggerType;
        //... puis on r�sout le comportement de l'objet en local
        BehaviorResolution(trigger);
    }

    /// <summary>
    /// R�solution du comportement sonore en local
    /// </summary>
    /// <param name="triggerType">L'�v�nement de comportement d�clench�</param>
    #endregion

}
