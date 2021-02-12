using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Le type de déclenchement que l'objet attend. Il hérite du type byte pour pouvoir être serialized pour Photon
/// </summary>
public enum TriggerType : byte
{
    OnClick,
    OnPointerEnter,
    OnToggleOn,
    OnToggleOff
}

/// <summary>
/// Un objet qui détermine le son à jouer en fonction d'un type d'événement
/// </summary>
[System.Serializable]
public class SoundBehavior
{
    [Tooltip("Définir l'événement qui déclenche le son")]
    public TriggerType triggerType;
    [Tooltip("L'événement lancé sur WWise lorsque la condition de déclenchement est atteinte")]
    public AK.Wwise.Event soundEvent;

    /// <summary>
    /// Post un événement WWise content l'Event renseigné en attribut
    /// </summary>
    /// <param name="gameObject">Le GameObject depuis lequel est posté l'Event WWise. Nécessaire pour le logique de ce dernier</param>
    public void PlaySound(GameObject gameObject)
    {
        Debug.Log("Play Sound");
        soundEvent.Post(gameObject);
    }
}

/// <summary>
/// Gère l'ensemble du comportement sonore de l'objet UI auquel il est rattaché.
/// </summary>
[RequireComponent(typeof(AkGameObj))]
public class UISound : Photon.PunBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [Header("Liste des comportements & sons associés")]
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
        //On récupère chacun des comportements du son, et pour chaque on associe l'événement correspondant.
        //Si jamais OnToggleOn ou OnToggleOff est présent dans le comportement, alors on va chercher l'objet Toggle
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

        //Si jamais l'objet est un Toggle, on en chercher le composant de même nom. Si jamais il n'existe pas ou n'est pas interactif,
        //alors on ne considère plus l'objet comme un Toggle (pour réguler correctement le comportement)
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
        //Si l'objet cliqué est un Toggle, alors on envoi l'événement associé à son nouvel état.
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
    /// Détermine si le son doit être envoyé en RPC à tous les clients, ou simplement être joué en local
    /// </summary>
    /// <param name="triggerType">Le type d'événement pour le comportement</param>
    void BehaviorTrigger(TriggerType triggerType)
    {
        if (PhotonNetwork.connected && this.photonView != null)
            photonView.RPC("BehaviorResolutionRPC", PhotonTargets.All, (byte)triggerType);
        else
            BehaviorResolution(triggerType);
    }

    /// <summary>
    /// Fonction RPC déclenché quand le son doit être joué pour tous les clients
    /// </summary>
    /// <param name="triggerType"></param>
    [PunRPC]
    void BehaviorResolutionRPC(byte triggerType)
    {
        //On réceptionne la donnée envoyé, que l'on reconverti en TriggerType pour la logique de l'objet...
        TriggerType trigger = (TriggerType)triggerType;
        //... puis on résout le comportement de l'objet en local
        BehaviorResolution(trigger);
    }

    /// <summary>
    /// Résolution du comportement sonore en local
    /// </summary>
    /// <param name="triggerType">L'événement de comportement déclenché</param>
    #endregion

}
