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
    #region Définition des événements
    public delegate void CustomEventTypeExample(GameObject obj, string aString);
    public event CustomEventTypeExample customEventExample;

    public delegate void GameEvent(GameObject gameObject);
    public event GameEvent PlayerReachEnd;

    public event Action BasicEventExemple;
    public delegate void SwitchEvent(List<GameObject> gameObjects);
    public event SwitchEvent switchOn;
    public event SwitchEvent switchOff;
    public delegate void SwitchEventBox(BoxController box);

    public event SwitchEventBox switchBox;

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

    /*public void DoorwayTriggerEnter()
    {
        Debug.Log("DoorWay");
        if (BasicEventExemple != null)
        {
            BasicEventExemple();
        }
    }*/
    public void SwitchTriggerOn(List<GameObject> gameObjects )
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
}


