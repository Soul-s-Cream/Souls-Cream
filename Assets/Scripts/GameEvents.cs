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

    public delegate void AbilityEvent(Player player, ScreamType scream);
    public event AbilityEvent newScreamAbility;
    public event AbilityEvent loseScreamAbility;
    public event AbilityEvent changeScreamAbilitySelected;

    public event Action BasicEventExemple;
    public delegate void SwitchEvent(Mecanism[] mecanism);
    public event SwitchEvent switchOn;
    public event SwitchEvent switchOff;

    public delegate void SwitchEventBox(Crate box);
    public event SwitchEventBox switchBox;

    public delegate void BrisDeEcranEvent(GameObject[] cassur); // permet d'afficher les sprites de vitre cassée à la fin du jeu
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
    /// <param name="box">Objet Crate qui doit être activé</param>
    public void TriggerSwitchBoxOn(Crate box)
    {
        switchBox?.Invoke(box);
    }

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="brokenWindow"></param>
    public void BDR(GameObject[] brokenWindow)
    {
        if (brokenWindow != null)
        {
            BrisDeEcranVoid(brokenWindow);
        }
    }

    /// <summary>
    /// Appelle l'événement 'changeScreamAbilitySelected'
    /// </summary>
    /// <param name="player">Le joueur qui change de cri</param>
    /// <param name="scream">Le nouveau cri selectionné</param>
    public void TriggerChangeSelectedScreamEvent(Player player, ScreamType scream)
    {
        changeScreamAbilitySelected?.Invoke(player, scream);
    }

    /// <summary>
    /// Appelle l'événement 'newScreamAbility'
    /// </summary>
    /// <param name="player">Le joueur qui a appris le cri</param>
    /// <param name="newScream">Le nouveau cri appris</param>
    public void TriggerNewScreamEvent(Player player, ScreamType newScream)
    {
        newScreamAbility?.Invoke(player, newScream);
    }

    /// <summary>
    /// Appelle l'événement 'loseScreamAbility'
    /// </summary>
    /// <param name="player">Le joueur qui a désappris le cri</param>
    /// <param name="loseScream">Le cri qui a été désappris</param>
    public void TriggerLoseScreamEvent(Player player, ScreamType loseScream)
    {
        loseScreamAbility?.Invoke(player, loseScream);
    }
    #endregion
}


