using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    #region Singleton Instance
    private static GameEvents _instance;
    public static GameEvents Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    #region Events Definitions
    public delegate void CustomEventTypeExample(GameObject obj, string aString);
    public event CustomEventTypeExample customEventExample;

    public delegate void GameEvent(GameObject gameObject);
    public event GameEvent playerReachEnd;

    public event Action BasicEventExemple;
    public delegate void SwitchEvent(Mecanism[] mecanism);
    public event SwitchEvent switchOn;
    public event SwitchEvent switchOff;

    public delegate void SwitchEventBox(Crate box);
    public event SwitchEventBox switchBox;

    public delegate void AnimIconesCrisEvent(GameObject icone);
    public event AnimIconesCrisEvent iconAnimSelected;
    public event AnimIconesCrisEvent iconAnimUnselected;

    public delegate void BrisDeEcranEvent(GameObject[] cassur);
    public event BrisDeEcranEvent BrisDeEcranVoid; 


    #endregion

    private void Awake()
    {
        #region Singleton Pattern
        if (_instance == null)
            _instance = this;
        else if (_instance != this)
            Destroy(this);
        #endregion
    }

    #region Events Public Calls

    /// <summary>
    /// Appelle l'événement PlayerReachEnd
    /// </summary>
    /// <param name="gameObject">L'objet qui a fait l'appel de l'événement</param>
    public void TriggerPlayerReachEnd(GameObject gameObject)
    {
        playerReachEnd(gameObject);
    }

    /// <summary>
    /// Appelle l'événement 'switchOn'
    /// </summary>
    /// <param name="gameObjects">Array des Mecanisms qui doivent être activés</param>
    public void TriggerSwitchOn(Mecanism[] mecanisms)
    {
        switchOn?.Invoke(mecanisms);
    }

    /// <summary>
    /// Appelle l'événement 'switchOff'
    /// </summary>
    /// <param name="gameObjects">Array des Mecanisms qui doivent être désactivés</param>
    public void TriggerSwitchOff(Mecanism[] mecanisms)
    {
        switchOff?.Invoke(mecanisms);
    }

    /// <summary>
    /// Appelle l'événement 'switchBox'
    /// </summary>
    /// <param name="box">BoxController qui doit être activé</param>
    public void TriggerSwitchBoxOn(Crate box)
    {
        switchBox?.Invoke(box);
    }

    /// <summary>
    /// Appelle l'événement 'iconAnimSelected'
    /// </summary>
    /// <param name="icon">L'icone qui doit être selectionnée ???</param>
    public void TriggerSwitchIconeUp(GameObject icon)
    {
        if (icon != null)
        {
            iconAnimSelected(icon);
        }
    }

    /// <summary>
    /// Appelle l'événement 'iconAnimUnselected'
    /// </summary>
    /// <param name="icon">L'icone qui doit être déselectionnée ???</param>
    public void TriggerSwitchIconeDown(GameObject Icone)
    {
        if (Icone != null)
        {
            iconAnimUnselected(Icone);
        }
    }
    public void BDR(GameObject[] brokenWindow)
    {
        if (brokenWindow != null)
        {
            BrisDeEcranVoid(brokenWindow);
        }
    }
    #endregion
}


