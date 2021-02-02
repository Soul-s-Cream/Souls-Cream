using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private static GameEvents _instance;
    public static GameEvents Instance
    {
        get
        {
            return _instance;
        }
    }
    #region D�finition des �v�nements
    public delegate void CustomEventTypeExample(GameObject obj, string aString);
    public event CustomEventTypeExample customEventExample;

    public delegate void GameEvent(GameObject gameObject);
    public event GameEvent PlayerReachEnd;

    public event Action BasicEventExemple;
    public delegate void SwitchEvent(GameObject[] gameObjects);
    public event SwitchEvent switchOn;
    //public event SwitchEvent switchOff;

    public delegate void SwitchEventBox(BoxController box);
    public event SwitchEventBox switchBox;

    public delegate void AnimIconesCrisEvent(GameObject icone);
    public event AnimIconesCrisEvent IconeAnimSelected;
    public event AnimIconesCrisEvent IconeAnimUnselected;

    public delegate void BrisDeEcranEvent(GameObject cassur);
    public event BrisDeEcranEvent BrisDeEcranVoid; 


    #endregion


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);

    }
    
    public void LaunchPlayerReachEnd(GameObject gameObject)
    {
        PlayerReachEnd(gameObject);
    }
    public void SwitchTriggerOn(GameObject[] gameObjects )
    {
        if (switchOn != null)
        {
            switchOn(gameObjects);
        }
    }

    public void SwitchBoxOn(BoxController box)
    {
        if (switchBox != null)
        {
            switchBox(box);
        }
    }

    public void SwitchIconeUp(GameObject Icone)
    {
        if (Icone != null)
        {
            IconeAnimSelected(Icone);
        }
    }
    public void SwitchIconeDown(GameObject Icone)
    {
        if (Icone != null)
        {
            IconeAnimUnselected(Icone);
        }
    }
    public void BDR(GameObject brokenWindow)
    {
        if (brokenWindow != null)
        {
            IconeAnimUnselected(brokenWindow);
        }
    }
}


