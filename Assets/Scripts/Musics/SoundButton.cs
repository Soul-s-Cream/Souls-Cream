using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TriggerType
{
    OnClick,
    OnPointerEnter
}

[RequireComponent(typeof(AkGameObj))]
public class SoundButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public AK.Wwise.Event soundEvent;
    [Tooltip("Définir l'événement qui déclenche le son")]
    public TriggerType triggerType;

    public void OnPointerClick(PointerEventData eventData)
    { 
        if(triggerType == TriggerType.OnClick)
        {
            PlaySound();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (triggerType == TriggerType.OnPointerEnter)
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        Debug.Log("Play Sound");
        soundEvent.Post(this.gameObject);
    }
}
