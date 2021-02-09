using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TriggerType
{
    OnClick,
    OnPointerEnter,
    OnToggleOn,
    OnToggleOff
}

[System.Serializable]
public class SoundBehavior
{
    [Tooltip("D�finir l'�v�nement qui d�clenche le son")]
    public TriggerType triggerType;
    [Tooltip("L'�v�nement lanc� sur WWise lorsque la condition de d�clenchement est atteinte")]
    public AK.Wwise.Event soundEvent;

    public void PlaySound(GameObject gameObject)
    {
        Debug.Log("Play Sound");
        soundEvent.Post(gameObject);
    }
}

[RequireComponent(typeof(AkGameObj))]
public class UISound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
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
        //On r�cup�re chacun des comportements du son, et pour chaque on associe l'�v�nement correspondant
        if (behaviors != null && behaviors.Count != 0)
        {
            foreach (SoundBehavior behavior in behaviors)
            {
                switch (behavior.triggerType)
                {
                    case TriggerType.OnClick: onClick += behavior.PlaySound; break;
                    case TriggerType.OnPointerEnter: onPointerEnter += behavior.PlaySound; break;
                    case TriggerType.OnToggleOn: onToggleOn += behavior.PlaySound; isToggle = true ; break;
                    case TriggerType.OnToggleOff: onToggleOff += behavior.PlaySound; isToggle = true; break;
                }
            }
        }

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

    public void OnPointerClick(PointerEventData eventData)
    {
        //Si l'objet cliqu� est un Toggle, alors on envoi l'�v�nement associ� � son nouvel �tat.
        if (isToggle)
        {
            if (toggle.isOn)
                onToggleOn?.Invoke(this.gameObject);
            else
                onToggleOff?.Invoke(this.gameObject);
        }
        else
            onClick?.Invoke(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke(this.gameObject);
    }


}
